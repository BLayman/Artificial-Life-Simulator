using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FinalOutputNode: NonInputNode
{
    public Action action;
    public Creature parentCreature;

    public FinalOutputNode(Creature parentCreature)
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

    public void setAction(string actionKey)
    {
        action = parentCreature.actionPool[actionKey];
    }
}