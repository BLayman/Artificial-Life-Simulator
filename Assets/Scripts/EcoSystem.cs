using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// This class contains all of the data about the state of the ecosystem including the populations, species templates, map, resources, and other general parameters.
/// </summary>
public class Ecosystem
{

    public string name = "default";
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

    public float timeUnitsPerTurn;

    public int count = 0;

    public int renewIntervalSteps = 10;

    public Color[] colors;



    /// <summary>
    /// run ecosystem for a certain number of time steps
    /// </summary>
    public void runSystem(int timeSteps)
    {
        for (int i = 0; i < timeSteps; i++)
        {
            // for a given time step:

            count++; // for keeping track of ecosystem age

            // for each population
            foreach (string species in populations.Keys)
            {
                Population population = populations[species];
                // if population has any members
                if (population.creatures.Count > 0)
                {
                    List<Creature> toRemove = new List<Creature>();
                    // for each creature in population
                    foreach (Creature creature in population.creatures)
                    {
                        // remove creature if dead
                        if (creature.isDead())
                        {
                            toRemove.Add(creature);
                        }
                        // otherwise start creature's turn
                        else
                        {
                            creature.startTurn();
                        }
                    }
                    // remove each dead creature from population
                    foreach (Creature deadCreature in toRemove)
                    {
                        population.creatures.Remove(deadCreature);
                    }
                }

                // renew land resources every renewIntervalSteps
                // TODO: spread this out over multiple steps somehow
                if (count % renewIntervalSteps == 0)
                {
                    for (int j = 0; j < map.Count; j++)
                    {
                        for (int k = 0; k < map[j].Count; k++)
                        {
                            foreach (ResourceStore res in map[j][k].propertyDict.Values)
                            {
                                res.renewResource();
                            }
                        }
                    }
                }
                

            }
        }
        // assuming user has set timeSteps to be 1 million or less
        if(count > int.MaxValue - 1000001)
        {
            Debug.Log("reseting ecosystem age");
            count = 0;
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


    public Ecosystem shallowCopy()
    {
        return (Ecosystem) this.MemberwiseClone();
    }


    public void updateTexture()
    {
        colors = new Color[map.Count * map[0].Count]; // reference update for each ecosystem

        Color creatureColor = Color.blue;
        //float st = Time.realtimeSinceStartup;
        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0; y < map[x].Count; y++)
            {
                if (map[x][y].creatureIsOn())
                {
                    colors[y * map.Count + x] = creatureColor;
                }
                else
                {
                    float proportionStored = map[x][y].propertyDict["grass"].getProportionStored();
                    Color resourceShade = new Color(proportionStored, proportionStored, proportionStored);
                    colors[y * map.Count + x] = resourceShade;
                }
            }
        }

        //float et = Time.realtimeSinceStartup;
        //Debug.Log("Time to update texture:" + (et - st));


    }

}