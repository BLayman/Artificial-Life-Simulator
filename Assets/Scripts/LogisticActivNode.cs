using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LogisticActivNode: NonInputNode
{
    public LogisticActivNode()
    {
        activBehavior = new LogisticActivBehavior();
    }
}