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
    /// <summary>
    /// Stores populations of creatures.
    /// </summary>
    public Dictionary<string, List<Creature>> populations = new Dictionary<string, List<Creature>>();
    /// <summary>
    /// Tenative list of resource options.
    /// </summary>
    public Dictionary<string, ResourceStore> tentativeResourceOptions;
    /// <summary>
    /// Tentative map to be saved to ecosystem map when saveMap is called.
    /// </summary>
    public List<List<Land>> tentativeMap = new List<List<Land>>();

    // population created by SpeciesPopulator
    private List<Creature> currentPopulation;

    public EcosystemCreator(Ecosystem _ecosystem)
    {
        ecosystem = _ecosystem;
        tentativeResourceOptions = ecosystem.resourceOptions;
        founderCreatures = ecosystem.species;
    }

    public void setAbilityPointsPerCreature(int abilityPoints)
    {
        ecosystem.abilityPointsPerCreature = abilityPoints;
    }

    public void setCommBits(int numBits)
    {
        ecosystem.commBits = numBits;
    }

    public void setDistinctPhenotypeNum(int numPhenotypes)
    {
        ecosystem.distictPhenotypes = numPhenotypes;
    }



    /// <param name="resourceName">Name of resource: used as key in dictionary.</param>
    public void addResource(string resourceName)
    {
        lrc = new LandResourceCreator(new ResourceStore(resourceName));
    }

    /// <summary>
    /// Saves a newly created resource to resourceOptions.
    /// </summary>
    public void saveResource()
    {
        tentativeResourceOptions.Add(lrc.resourceStore.name, lrc.resourceStore);
    }

    /// <summary>
    /// Saves created resourceOptions to ecosystems actual resource options.
    /// </summary>
    public void saveResourceOptions()
    {
        ecosystem.resourceOptions = tentativeResourceOptions;
    }

    public void setTimeUnitsPerTurn(int timeUnits)
    {
        ecosystem.timeUnitsPerTurn = timeUnits;
    }

    public void addToFounders()
    {
        founderCreatures.Add(creatureCreator.creature.species, creatureCreator.creature);
    }

    /// <summary>
    /// Saves founders to ecosystem's species dictionary.
    /// </summary>
    public void saveFoundersToSpecies()
    {
        ecosystem.species = founderCreatures;
    }

    public SpeciesPopulator populateSpecies(string founderSpecies)
    {
        speciesPopulator = new SpeciesPopulator(founderCreatures[founderSpecies]);
        return speciesPopulator;
    }

    /// <summary>
    /// Saves population creatured by speciesPopulator.
    /// </summary>
    public void saveCurrentPopulation()
    {
        currentPopulation = speciesPopulator.population;
    }

    public void addCurrentPopulationToEcosystem()
    {
        ecosystem.populations.Add(currentPopulation[0].founder.species, currentPopulation);
    }

    public void saveEditedMap()
    {
        tentativeMap = mapEditor.map;
    }

    /// <summary>
    /// Saves tentative map to actual ecosystem map.
    /// </summary>
    public void saveMap()
    {
        ecosystem.map = tentativeMap;
    }

    public CreatureCreator addCreature()
    {
        creatureCreator = new CreatureCreator(new Creature(ecosystem.abilityPointsPerCreature), ecosystem.distictPhenotypes);
        return creatureCreator;
    }

    /// <summary>
    /// adds a population to the tentative map.
    /// </summary>
    public void addCurrentPopulationToMap()
    {

    }
}