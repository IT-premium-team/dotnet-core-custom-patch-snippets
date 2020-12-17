public class SpellPatchDTO
{
    public string PropertyName { get; set; }
    public object PropertyValue { get; set; }
}

// in spell controller
[HttpPatch("update-spell/{id}")]
[Authorize]
public async Task<IActionResult> UpdateSpell(int id, [FromBody] IEnumerable<SpellPatchDTO> patchDtos)
{
    if (id == default)
    {
        return BadRequest($"Cannot recognize spell id {id}");
    }

    try
    {
        var result = await spellService.UpdateSpell(id, patchDtos);
        return Ok(result);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}

// in spell service
private string Capitalize(string charSequence)
{
    return charSequence.Substring(0, 1).ToUpper() + charSequence.Substring(1);
}

public async Task<Spell> UpdateSpell(int it, IEnumerable<SpellPatchDTO> patchDTOs)
{
    var spell = await entities.GetAsync(id);
    var patchList = patchDTOs.ToList();

    if (patchList.Count() == 0)
    {
        return spell;
    }

    var eemType = spell.GetType();

    foreach (var patch in patchList)
    {
        var propertyName = Capitalize(patch.PropertyName);
        var property = eemType.GetProperty(propertyName);
        object propertyValue;
        switch (propertyName)
        {
            // converts number from front as emum
            case "Type": // "Unforgivable curses", "Protective spells" etc. (enum)
                propertyValue = (SpellType)Enum
                    .ToObject(typeof(SpellType), Convert.ToInt32(patch.PropertyValue));
                break;
            case "LastUsedDate":
                propertyValue = (DateTime)patch.PropertyValue;
                break;
            default:
                propertyValue = patch.PropertyValue;
                break;
        }
        if (property != null)
        {
            property.SetValue(spell, propertyValue);
        }
    }

    entities.Update(spell);
    entities.SaveChanges();

    return spell;
}