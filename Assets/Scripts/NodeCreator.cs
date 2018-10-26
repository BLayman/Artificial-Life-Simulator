using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum NodeCreatorType { siNodeCreator, commNodeCreator }

public class NodeCreator
{
    NodeCreatorInterface nodeCreator;
    public int nodeLayer;
    NodeCreatorType creatorType;

    // user picks layer to create node in, initializing a node creator
    public NodeCreator(int _layer)
    {
        nodeLayer = _layer;
    }


    // puser picks sensory input node as the type they would like to create
    public void setCreator(NodeCreatorType type)
    {
        creatorType = type;

        switch (type)
        {
            case NodeCreatorType.siNodeCreator:
                // now node will be modified by siNodeCreator
                nodeCreator = new SensoryInputNodeCreator(new SensoryInputNode(), nodeLayer);
                break;
            case NodeCreatorType.commNodeCreator:
                nodeCreator = new CommNodeCreator(new CommInputNode(), nodeLayer);
                break;
            default:
                break;
        }
        // now this NodeCreator will now be passed to a gameObject ( or referenced by a gameObject) 
        // that only displays node edits for it's type and only uses the variable for that kind of nodeCreator      
    }

    public NodeCreatorInterface getNodeCreator()
    {
        switch (creatorType)
        {
            case NodeCreatorType.siNodeCreator:
                return (SensoryInputNodeCreator)nodeCreator;
            case NodeCreatorType.commNodeCreator:
                return (CommNodeCreator)nodeCreator;
            default:
                return null;
        }
    }

    public Node getCreatedNode()
    {
        return nodeCreator.getNode();
    }



}