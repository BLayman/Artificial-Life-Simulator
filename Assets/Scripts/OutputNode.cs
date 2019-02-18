using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Maps a the value of the node (as calculated by NonInputNode methods) to an action.
/// </summary>
public class OutputNode : NonInputNode
{
    public Action action;
    public Creature parentCreature;
    public string actionKey;

    public OutputNode() { }

    public OutputNode(Creature parentCreature, Network parentNet, int layer) : base(parentNet, layer)
    {

        activBehavior = new LogisticActivBehavior();
        this.parentCreature = parentCreature;
    }

    /// <summary>
    /// Adds its action to the action queue based on probability returned from the activation function.
    /// </summary>
    public void addActionIfActive()
    {
        parentCreature.actionQueue.Enqueue(action, action.priority);
    }

    // action is assigned using action pool of creature
    public void setAction(string actionKey)
    {
        action = parentCreature.actionPool[actionKey];
    }


    public OutputNode clone()
    {
        return (OutputNode)this.MemberwiseClone();
    }


}