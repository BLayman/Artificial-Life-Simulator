using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SpeciesPopulator
{
    /// <summary>
    /// Stores Information about the population.
    /// </summary>
    public Population population;


    public SpeciesPopulator(Creature _founder)
    {
        population.founder = _founder;
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
    /// Creates set number of creatures, randomly across the map. Adds them to population variable.
    /// </summary>
    /// <param name="size">Number of creatures.</param>
    public void populateRandom(int size)
    {
        population.creatures = new List<Creature>(size);
        // remember to set creature founder
        for (int i = 0; i < size; i++)
        {
            population.creatures[i] = population.generateMember();
        }
    }

    /// <summary></summary>
    /// <param name="standardDeviation"></param>
    public void SetAbilityStandardDeviation(float standardDeviation)
    {
        population.abilityStandardDev = standardDeviation;
    }
}