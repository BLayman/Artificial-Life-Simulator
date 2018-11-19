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
    /// <summary>
    /// Creature this action belongs to.
    /// </summary>
    private Creature creature;

    /// <summary>
    /// Adds signal to creatures output comm signals. (to be iterated over and passed to neighbors).
    /// </summary>
    public override void perform(Creature creature)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assigns properties of signal
    /// </summary>
    public void assignSignalProps()
    {
        throw new System.NotImplementedException();
    }
}