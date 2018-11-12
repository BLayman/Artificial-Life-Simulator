using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class BiasNode : Node
{

    public BiasNode(float bias)
    {
        value = bias;
    }

    /// <summary>
    /// sets initial value
    /// </summary>
    public void setBias()
    {
        throw new System.NotImplementedException();
    }

    public override void updateValue()
    {
        return;
    }
}