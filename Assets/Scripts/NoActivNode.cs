using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class NoActivNode: NonInputNode
{
    public NoActivNode()
    {
        activBehavior = new EmptyActivBehavior();
    }
}