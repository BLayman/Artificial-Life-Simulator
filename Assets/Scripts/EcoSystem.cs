using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Ecosystem
{
    /// <summary>
    /// Lists of creatures, organized by species name in dictionary.
    /// </summary>
    public Dictionary<string, Population> populations = new Dictionary<string, Population>();
    /// <summary>
    /// map of land spaces
    /// </summary>
    public List<List<Land>> map = new List<List<Land>>();
    /// <summary>
    /// Length of map
    /// </summary>
    public int length;
    /// <summary>
    /// Width of map
    /// </summary>
    public int width;
    public int commBits;
    /// <summary>
    /// Pool of species (proto-creatures) that can be used to generate new populations.
    /// </summary>
    public Dictionary<string, Creature> species = new Dictionary<string, Creature>();
    /// <summary>
    /// Number of possible phenotypes a creature can take on.
    /// </summary>
    public int distictPhenotypes;
    /// <summary>
    /// Max number of ability points that each creature has to assign.
    /// </summary>
    public int abilityPointsPerCreature;
    /// <summary>
    /// List of resources that can be stored on Lands.
    /// </summary>
    public Dictionary<string, ResourceStore> resourceOptions = new Dictionary<string, ResourceStore>();

    public int timeUnitsPerTurn;

    /// <summary>
    /// run ecosystem for a certain number of time steps
    /// </summary>
    public void runSystem(int timeSteps)
    {
        for (int i = 0; i < timeSteps; i++)
        {
            foreach (string species in populations.Keys)
            {
                Population population = populations[species];
                if(population.creatures.Count > 0)
                {
                    List<Creature> toRemove = new List<Creature>();
                    foreach (Creature creature in population.creatures)
                    {
                        creature.startTurn();
                        if (creature.isDead())
                        {
                            toRemove.Add(creature);
                        }
                    }

                    foreach (Creature deadCreature in toRemove)
                    {
                        population.creatures.Remove(deadCreature);
                    }
                }
                
            }
        }
        
    }


    /// <summary>
    /// creates new species in dictionary based on given creature
    /// </summary>
    public void addSpecies(Creature creature)
    {
        species.Add(creature.species, creature);
    }

    public void createPopulation(int size, Creature species)
    {

    }
}