using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BiasNode : Node
{


    /// <summary>
    /// sets bias to a value
    /// </summary>
    public void setBias(float newBias)
    {
        value = newBias;
    }

    public override void updateValue()
    {
        return;
    }

    public BiasNode clone()
    {
        return (BiasNode) this.MemberwiseClone();
    }
}