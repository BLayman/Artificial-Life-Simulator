// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;

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

    public int age = 0;

    public int renewIntervalSteps = 10;

    public Color[] colors;

    int renewRow = 0;

    int textureThreads = 4;

    bool[] threadsFinished;

    public bool allDead = false;

    public int statsInterval = 20; // update stats every 10 turns
    public int statsCount = 20;

    System.Object threadsFinishedLock = new System.Object();


    /// <summary>
    /// run ecosystem for a certain number of time steps
    /// </summary>
    public void runSystem(int timeSteps)
    {


        for (int i = 0; i < timeSteps; i++)
        {
            // for a given time step:

            age++; // for keeping track of ecosystem age

            bool notAllDead = false;
            // for each population
            foreach (string species in populations.Keys)
            {
                Population population = populations[species];
                // if population has any members
                if (population.creatures.Count > 0)
                {
                    //Debug.Log(population.creatures[0].mutationStandardDeviation);
                    notAllDead = true;
                    List<Creature> toRemove = new List<Creature>();
                    // for each creature in population
                    // TODO: convert creatures to linked list
                    for (int l = 0; l < population.creatures.Count; l++)
                    {
                        Creature creature = population.creatures[l];
                        // remove creature if dead at beginning of turn
                        if (creature.isDead())
                        {
                            toRemove.Add(creature);
                        }
                        // otherwise start creature's turn
                        else
                        {
                            creature.startTurn(this);
                        }
                        // remove creature if dead at end of turn
                        if (creature.isDead())
                        {
                            toRemove.Add(creature);
                        }
                    }

                    // remove each dead creature from population
                    // TODO: implement faster remove if possible
                    foreach (Creature deadCreature in toRemove)
                    {
                        population.creatures.Remove(deadCreature);
                        population.size--;
                    }
                    
                    population.creatures.AddRange(population.offspring);
                    population.offspring = new List<Creature>();

                }

                // renew all land resources every renewIntervalSteps
                // but spread out over every step

                bool renewAllAtOnce = false;
                int intervalRows = (int) Math.Round(map.Count / (double)renewIntervalSteps);
                if(intervalRows == 0) { renewAllAtOnce = true; }

                
                // if renew happens less often than the number of columns in the map, then renew everything at that interval
                if (renewAllAtOnce)
                {
                    if (age % renewIntervalSteps == 0)
                    {
                        for (int j = 0; j < map.Count; j++)
                        {
                            for (int k = 0; k < map[j].Count; k++)
                            {
                                foreach (ResourceStore res in map[j][k].propertyDict.Values)
                                {
                                    if (res.renewalAmt > 0)
                                    {
                                        res.renewResource();
                                    }
                                }
                            }
                        }
                    }
                }
                // renew a little each turn
                else
                {
                    int stopAt = renewRow + (intervalRows);
                    if (stopAt > map.Count)
                    {
                        // if renewRow has reached the end, reset it
                        if (renewRow == map.Count)
                        {
                            renewRow = 0;
                            stopAt = renewRow + intervalRows;
                        }
                        // if stop at has gone beyond the end of the map, and renewRow hasn't reached the end yet, then stop at the end
                        else
                        {
                            stopAt = map.Count;
                        }
                    }
                    for (; renewRow < stopAt; renewRow++)
                    {
                        for (int k = 0; k < map[renewRow].Count; k++)
                        {

                            foreach (ResourceStore res in map[renewRow][k].propertyDict.Values)
                            {

                                if (res.renewalAmt > 0)
                                {
                                    res.renewResource();
                                }
                            }
                        }
                    }
                }

                // calculate averages
                if(statsCount == statsInterval)
                {
                    populations[species].calculateWeightStats();
                    Debug.Log("overall variability for: " + species + " is " + populations[species].overallVariability);
                }
                
            }
            // calculate averages
            if (statsCount == statsInterval)
            {
                statsCount = 0;
            }
            statsCount++;

            if (!notAllDead)
            {
                allDead = true;
            }
        }

        //Debug.Log("age: " + age);

        foreach (Population pop in populations.Values)
        {
            //Debug.Log(pop.founder.species + " pop size: " + pop.size);

        }
        // assuming user has set timeSteps to be 1 million or less
        if (age > int.MaxValue - 1000001)
        {
            Debug.Log("reseting ecosystem age");
            age = 0;
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



    /** Texture stuff below **/

  
    public void updateTexture(string visibleResource)
    {
        colors = new Color[map.Count * map[0].Count]; // reference update for each ecosystem
        TextureUpdater.updateTexture(map, colors, visibleResource);

        //float et = System.DateTime.Now.Millisecond;
        //Debug.Log("Time to update texture:" + (et - st));
    }
    
    /** tried using multi-threading with textures, but can't find benefit **/

    /*
    public void startTextureThreads()
    {

        threadsFinished = new bool[textureThreads + 1];
        colors = new Color[map.Count * map[0].Count]; // reference update for each ecosystem
        int interval = (int)Math.Round(colors.Length / (double)textureThreads); // number of pixels processed by each thread
        int startAt = 0;
        int threadIndex = 0;
        int stopAt;

        while (startAt < colors.Length)
        {
             stopAt = startAt + (interval);

            if (stopAt > colors.Length)
            {
                stopAt = colors.Length;
            }
            //Debug.Log("starting thread: " + threadIndex);
            int indexCopy = threadIndex;
            int startCopy = startAt;
            int stopCopy = stopAt;
            Thread t = new Thread(() => { threadUpdateTexture(startCopy, stopCopy, indexCopy); });
            t.Start();
            threadIndex++;
            startAt = stopAt;
        }
        
    }


    public void threadUpdateTexture(int startIndex, int endIndex, int threadIndex)
    {
        Color creatureColor = Color.blue;
        //Debug.Log("thread called: " + threadIndex);
        //float st = Time.realtimeSinceStartup;
        for (int i = startIndex; i < endIndex; i++)
        {
            int y = (int)((double)i / map.Count);
            int x = i % map.Count;
            if (map[x][y].creatureIsOn())
            {
                colors[i] = creatureColor;
            }
            else
            {
                float proportionStored = map[x][y].propertyDict["grass"].getProportionStored();
                Color resourceShade = new Color(proportionStored, proportionStored, proportionStored);
                colors[i] = resourceShade;
            }
        }
        lock (threadsFinishedLock)
        {
            threadsFinished[threadIndex] = true;
            //Debug.Log("thread finished: " + threadIndex);

        }
    }

    public bool checkThreadsFinished()
    {
        bool finished = true;
        lock (threadsFinishedLock)
        {
            for (int i = 0; i < threadsFinished.Length - 1; i++)
            {
                //Debug.Log("thread: " + i + " finished: " + threadsFinished[i]);
                if (!threadsFinished[i])
                {
                    finished = false;
                }
            }
        }
        return finished;
    }
    */
}