using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class InternalResInputNodeEditor : NodeEditorInterface
{
    InternalResourceInputNode irNode;
    //public int nodeLayer; // still needed?

    public InternalResInputNodeEditor(InternalResourceInputNode _irNode) // , int _nodeLayer)
    {
        irNode = _irNode;
        //nodeLayer = _nodeLayer;
    }

    public Node getNode()
    {
        return irNode;
    }

    public void setSensedResource(string resource)
    {
        irNode.sensedResource = resource;
    }
}