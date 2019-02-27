using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ReproAction : Action
{
    /// <summary>
    /// Creature who created the action.
    /// </summary>
    public Creature creature;
    /// <summary>
    /// Index of neighbor that creature is requesting to reproduce with.
    /// </summary>
    public int targetNeighborIndex;

    public override void perform(Creature creature)
    {
        throw new NotImplementedException();
    }

    public ReproAction shallowCopy()
    {
        return (ReproAction)this.MemberwiseClone();
    }
}