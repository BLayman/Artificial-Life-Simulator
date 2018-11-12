using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class FinalOutputNode: NonInputNode
{
    private Action action;

    public FinalOutputNode()
    {
        activBehavior = new LogisticActivBehavior();
    }

    public float activFunct(float input)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds its action to the action queue based on probability returned from the activation function.
    /// </summary>
    public void addActionIfActive()
    {
        throw new System.NotImplementedException();
    }
}