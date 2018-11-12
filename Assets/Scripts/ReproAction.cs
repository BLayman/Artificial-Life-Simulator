using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class ReproAction : Action
{
    /// <summary>
    /// Creature who created the action.
    /// </summary>
    private Creature creature;
    /// <summary>
    /// Index of neighbor that creature is requesting to reproduce with.
    /// </summary>
    private int targetNeighborIndex;

    public override void perform(Creature creature)
    {
        throw new NotImplementedException();
    }
}