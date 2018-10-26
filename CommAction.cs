using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CommAction : Action
{
    /// <summary>
    /// CommSignal to be attached to target creature.
    /// </summary>
    private CommSignal signal;
    /// <summary>
    /// Index of neighbor comm signal is being sent to.
    /// </summary>
    private int targetNeighborIndex;

    public override void perform(Creature creature)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assigns properties of signal before it is sent: message, position (from neighbor), and phenotype (of sending creature).
    /// </summary>
    public void assignSignalProps()
    {
        throw new System.NotImplementedException();
    }
}