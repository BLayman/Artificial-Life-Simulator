using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class InnerInputNode : Node
{
    public Node inNode;

    /// <summary>
    /// This method simply gets the value from inNode.
    /// </summary>
    public override void updateValue()
    {
        throw new NotImplementedException();
    }
}