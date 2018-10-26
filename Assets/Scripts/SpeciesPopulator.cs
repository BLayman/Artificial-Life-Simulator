using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SpeciesPopulator
{
    private Creature founder;
    private List<List<Land>> map;
    /// <summary>
    /// List of creatures in the population.
    /// </summary>
    private List<Creature> population;

    public SpeciesPopulator(Creature _founder, List<List<Land>> _map)
    {
        founder = _founder;
        map = _map;
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
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Creates set number of creatures, randomly across the map. Adds them to population variable.
    /// </summary>
    /// <param name="size">Number of creatures.</param>
    public void populateRandom(int size)
    {
        throw new System.NotImplementedException();
    }

    /// <summary></summary>
    /// <param name="standardDeviation"></param>
    public void SetAbilityStandardDeviation(float standardDeviation)
    {
        throw new System.NotImplementedException();
    }
}