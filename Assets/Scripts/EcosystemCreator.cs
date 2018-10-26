using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EcosystemCreator
{
    /// <summary>
    /// Ecosystem to be created.
    /// </summary>
    public Ecosystem ecosystem;
    /// <summary>
    /// Creates Lands to add to EcoSystem resourceOptions.
    /// </summary>
    public LandResourceCreator lrc;
    /// <summary>
    /// Edits map of lands.
    /// </summary>
    public MapEditor mapEditor;
    /// <summary>
    /// For generating founder creatures.
    /// </summary>
    public CreatureCreator creatureCreator;
    /// <summary>
    /// Creatures used to generate populations.
    /// </summary>
    public Dictionary<string, Creature> founderCreatures;
    /// <summary>
    /// Users founder and map to populate species.
    /// </summary>
    public SpeciesPopulator speciesPopulator;

    public EcosystemCreator(Ecosystem _ecosystem)
    {
        ecosystem = _ecosystem;
        mapEditor = new MapEditor(ecosystem.map);
    }

    public void setAbilityPointsPerCreature(int abilityPoints)
    {
        throw new System.NotImplementedException();
    }

    public void setCommBits(int numBits)
    {
        throw new System.NotImplementedException();
    }

    public void setDistinctPhenotypeNum(int numPhenotypes)
    {
        throw new System.NotImplementedException();
    }

    public void generateMap(int length, int width)
    {
        throw new System.NotImplementedException();
    }

    /// <param name="resourceName">Name of resource: used as key in dictionary.</param>
    public void addResource(string resourceName)
    {
        lrc = new LandResourceCreator(new ResourceStore(resourceName));
    }

    public void saveResource()
    {
        ecosystem.resourceOptions.Add(lrc.resourceStore.name, lrc.resourceStore);
    }

    public void setTimeUnitsPerTurn(int timeUnits)
    {
        ecosystem.timeUnitsPerTurn = timeUnits;
    }

    public void addToFounders(Creature founder)
    {
        founderCreatures.Add(founder.species, founder);
    }

    /// <summary>
    /// Saves founders to ecosystem's species dictionary.
    /// </summary>
    public void saveFounders()
    {
        throw new System.NotImplementedException();
    }

    public void populateSpecies(string founderSpecies)
    {
        speciesPopulator = new SpeciesPopulator(founderCreatures[founderSpecies], ecosystem.map);
    }
}