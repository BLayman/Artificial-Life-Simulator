using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SpeciesPopulator
{
    /// <summary>
    /// Stores Information about the population.
    /// </summary>
    public Population population = new Population();
    public List<List<Land>> map;
    public List<int[]> spotsTaken = new List<int[]>();

    public SpeciesPopulator(Creature founder, List<List<Land>> map)
    {
        population.founder = founder;
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

    /// <summary>
    /// Creates set number of creatures, sets their locations randomly across the map (to be added later). Adds them to population variable.
    /// </summary>
    /// <param name="size">Number of creatures.</param>
    public void populateRandom(int size)
    {
        population.creatures = new List<Creature>();
        Random rand = new Random();

        //TODO: handle this case better
        if(size > (map.Count * map[0].Count) * 3.0/4.0)
        {
            Console.WriteLine("population is too large for random initialization");
            return;
        }

        for (int i = 0; i < size; i++)
        {
            population.creatures.Add(population.generateMember());
            population.creatures[i].position = new int[2];
            int x;
            int y;
            do
            {
                y = rand.Next(0, map.Count);
                x = rand.Next(0, map[0].Count);
            } while (checkIfTaken(x, y));

            population.creatures[i].position[0] = x;
            population.creatures[i].position[1] = y;

        }
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