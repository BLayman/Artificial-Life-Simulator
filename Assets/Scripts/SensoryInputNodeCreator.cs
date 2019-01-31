using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// API for SensoryInputNodes.
/// </summary>
public class SensoryInputNodeCreator : NodeCreatorInterface
{
    public SensoryInputNode siNode;
    public int nodeLayer;
    private NetworkCreator parentNetCreator;

    public SensoryInputNodeCreator(SensoryInputNode _siNode, int _nodeLayer)
    {
        siNode = _siNode;
        nodeLayer = _nodeLayer;
    }

    public Node getNode()
    {
        return siNode;
    }

    public void setLandIndex(int index)
    {
        siNode.neighborLandIndex = index;
    }

    public void setSensedResource(string resource)
    {
        siNode.sensedResource = resource;
    }

}