using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CommNodeCreator : NodeCreatorInterface
{
    public CommInputNode commNode;
    public int nodeLayer;
    private NetworkCreator parentNetCreator;

    public CommNodeCreator(CommInputNode _commNode, int _nodeLayer)
    {
        commNode = _commNode;
        nodeLayer = _nodeLayer;
    }

    public Node getNode()
    {
        return commNode;
    }

    void setBitIndex(int bitIndex)
    {
        commNode.bitIndex = bitIndex;
    }

    void setCommProperty(string property)
    {
        commNode.commProperty = property;
    }

}