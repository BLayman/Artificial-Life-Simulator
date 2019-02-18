using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ConsumeFromLand : Action
{
    /// <summary>
    /// Index of neighboring land to access.
    /// </summary>
    public int neighborIndex;
    /// <summary>
    /// Name of resource to consume.
    /// </summary>
    public string resourceToConsume;

    public ConsumeFromLand() { }

    public ConsumeFromLand(int neighborIndex, string resourceToConsume)
    {
        this.neighborIndex = neighborIndex;
        this.resourceToConsume = resourceToConsume;
    }

    /// <summary>
    /// Accesses land via neighborIndex, and attemps to consume resource.
    /// </summary>
    public override void perform(Creature creature)
    {
        Land land = creature.neighborLands[neighborIndex];
        float consumed = land.attemptResourceConsumption(resourceToConsume, timeCost, creature.abilities[resourceToConsume].level);
        creature.storedResources[resourceToConsume].currentLevel += (int) Math.Round((double)consumed);
    }

    public Object clone()
    {
        return this.MemberwiseClone();
    }
}