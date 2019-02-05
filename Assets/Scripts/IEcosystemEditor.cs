public interface IEcosystemEditor
{
    CreatureEditor addCreature();
    void addCurrentPopulationToEcosystem();
    void addCurrentPopulationToMap();
    void addResource(string resourceName);
    void addToFounders();
    SpeciesPopulator populateSpecies(string founderSpecies);
    void saveCurrentPopulation();
    void saveEditedMap();
    void saveFoundersToSpecies();
    void saveMap();
    void saveResource();
    void saveResourceOptions();
    void setAbilityPointsPerCreature(int abilityPoints);
    void setCommBits(int numBits);
    void setDistinctPhenotypeNum(int numPhenotypes);
    void setName(string name);
    void setTimeUnitsPerTurn(int timeUnits);
}