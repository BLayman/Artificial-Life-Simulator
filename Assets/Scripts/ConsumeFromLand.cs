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
    private string resourceToConsume;
    public Creature parentCreature;

    public ConsumeFromLand() { }

    public ConsumeFromLand(int neighborIndex, string resourceToConsume, Creature parentCreature)
    {
        this.neighborIndex = neighborIndex;
        this.resourceToConsume = resourceToConsume;
        this.parentCreature = parentCreature;
    }

    /// <summary>
    /// Accesses land via neighborIndex, and attemps to consume resource.
    /// </summary>
    public override void perform(Creature creature)
    {
        throw new NotImplementedException();
    }

    public Object clone()
    {
        return this.MemberwiseClone();
    }
}