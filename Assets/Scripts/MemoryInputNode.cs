using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MemoryInputNode : Node
{
    /// <summary>
    /// How many timee steps previous did this occur on?
    /// </summary>
    private int stepsPrevious;
    private int layerOfNetworks;
    private int layerInsideNet;
    private int nodeIndexInLayer;

    /// <summary></summary>
    public override void updateValue()
    {
        throw new NotImplementedException();
    }
}