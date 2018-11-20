using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class Action
{
    public string name;
    public int priority;
    /// <summary>
    /// Stores which resources are spend, and the amount spent
    /// </summary>
    public System.Collections.Generic.Dictionary<string, float> resourceCosts;
    public int timeCost;

    /// <summary>
    /// Performs the action.
    /// </summary>
    public abstract void perform(Creature creature);
}