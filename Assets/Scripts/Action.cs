using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class Action
{
    public string name;
    public int priority;
    /// <summary>
    /// Stores which resources are spent, and the amount spent
    /// </summary>
    public Dictionary<string, float> resourceCosts = new Dictionary<string, float>();
    public int timeCost;

    /// <summary>
    /// Performs the action.
    /// </summary>
    public abstract void perform(Creature creature);


    public Action getShallowCopy()
    {
        return (Action)this.MemberwiseClone();
    }
}