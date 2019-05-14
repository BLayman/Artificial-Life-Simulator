// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Class for using a founder species to generate a population on the map. Wraps Population class.
/// </summary>
public class SpeciesPopulator
{
    public static int creatureNum = 0; // for assigning each creature a unique number
    /// <summary>
    /// Stores Information about the population.
    /// </summary>
    public Population population = new Population();
    public List<List<Land>> map;
    public List<int[]> spotsTaken = new List<int[]>();

    public SpeciesPopulator(Creature founder, List<List<Land>> map)
    {
        population.founder = founder;
        founder.map = map;
        this.map = map;
    }

    /// <summary>
    /// Populate uniformly across map, using given probability.
    /// </summary>
    /// <param name="probability">Probability of making a creature in a given land. Must be between 0 and 1.</param>
    public void populateRandomUniform(float probability)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Set the standard deviation of creature network weights.
    /// </summary>
    public void setNetworkWeightStandardDeviation(float standardDeviation)
    {
        population.weightStandardDev = standardDeviation;
    }

    public void setMaxPopSize(int max)
    {
        population.maxSize = max;
    }

    /// <summary>
    /// Creates set number of creatures, sets their locations randomly across the map (to be added later). Adds them to population variable.
    /// </summary>
    /// <param name="size">Number of creatures.</param>
    public void populateRandom(int size)
    {
        population.creatures = new List<Creature>();
        System.Random rand = new System.Random();

        //TODO: handle this case better
        if (size > (map.Count * map[0].Count) * 3.0 / 4.0)
        {
            Console.WriteLine("population is too large for random initialization");
            return;
        }


        for (int i = 0; i < size; i++)
        {
            Creature addedCreature = population.generateMember();
            addedCreature.parentPopulation = population;
            population.creatures.Add(addedCreature);
            population.size++;
            addedCreature.iD = creatureNum.ToString();
            creatureNum++;
            addedCreature.addVariationToWeights(population.weightStandardDev);
            //population.creatures[i].printNetworks();
            int x;
            int y;

            do
            {
                y = rand.Next(0, map.Count);
                x = rand.Next(0, map[0].Count);
            } while (checkIfTaken(x, y));

            addedCreature.position[0] = x;
            addedCreature.position[1] = y;

            //Debug.Log("x " + x);
            //Debug.Log("y " + y);
            //addedCreature.updateNeighbors();
        }

        //Debug.Log("finished populating");

        population.size = size;
    }

    private bool checkIfTaken(int x, int y)
    {
        for (int i = 0; i < spotsTaken.Count; i++)
        {
            if (spotsTaken[i][0] == x && spotsTaken[i][1] == y)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary></summary>
    /// <param name="standardDeviation"></param>
    public void SetAbilityStandardDeviation(float standardDeviation)
    {
        population.abilityStandardDev = standardDeviation;
    }


}