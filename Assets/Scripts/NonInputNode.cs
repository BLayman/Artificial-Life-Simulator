using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class NonInputNode : Node
{
    private List<Node> prevNodes;
    /// <summary>
    /// Holds an object that carries out an activation function.
    /// </summary>
    protected ActivationBehavior activBehavior;

    public NonInputNode()
    {
        // set to logistic activation by default
        activBehavior = new LogisticActivBehavior();
    }

    public override void updateValue()
    {
        throw new NotImplementedException();
    }

    public void appendPrevNode(Node newNode)
    {
        throw new System.NotImplementedException();
    }

    public void removePrevNode(int index)
    {
        throw new System.NotImplementedException();
    }

    private float sumPrevVals()
    {
        throw new System.NotImplementedException();
    }

    /// <param name="index">Index of previous node.</param>
    /// <param name="value">Value coming from previous node.</param>
    public void setPrevNodeVal(int index, float value)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Calls activation function of activBehavior.
    /// </summary>
    public float performActivBehavior(float input)
    {
        return activBehavior.activFunct(input);
    }

    /// <summary>
    /// Sets a new activation behavior, thus altering the activation function.
    /// </summary>
    public void setActivBehavior(ActivationBehavior behavior)
    {
        activBehavior = behavior;
    }
}