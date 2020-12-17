export interface SpellPatchDTO {
    propertyName: string;
    propertyValue: Object;
}

// main code
private async run(): Promise<void> {
    const patchDtos: SpellPatchDTO[] = [];
    patchDtos.push({
        propertyName: 'type',
        propertyValue: this.spell.type,
    });
    patchDtos.push({
        propertyName: 'name',
        propertyValue: this.spell.name,
    });
    
    if (onceInABlueMoon) {
        patchDtos.push({
            propertyName: 'lastUsedDate',
            propertyValue: this.spell.lastUsedDate,
        });
    }
    
    this.updateSpell(this.spell.id, patchDtos);
}

private async updateSpell(spellId: number, patchDtos: SpellPatchDTO[]): Promise<Spell> {
    return this.spellService
      .updateSpell(spellId, patchDtos)
      .toPromise();
}

// spell service
public updateSpell(spellId: number, patchDtos: SpellPatchDTO[]): Observable<Spell> {
    const url = `${this.apiServiceUrl}${this.endpoints.baseUrl + this.endpoints.nested.updateSpell}`;
    return this.httpService.patch(`${url}/${spellId}`, patchDtos);
}