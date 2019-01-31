using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum NodeCreatorType { siNodeCreator, commNodeCreator, outputNodeCreator, innerInputNodeCreator }


/// <summary>
/// API for storing different kinds of specific node creators. ex. - could wrap a SensoryInputNodeCreator. Stored by NetworkCreator.
/// </summary>
public class NodeCreator
{
    NodeCreatorInterface nodeCreator;
    public int nodeLayer;
    NodeCreatorType creatorType;
    NetworkCreator parentNetCreator;

    // user picks layer to create node in, initializing a node creator
    public NodeCreator(int _layer, NetworkCreator parentNetCreator)
    {
        nodeLayer = _layer;
        this.parentNetCreator = parentNetCreator;
    }


    // puser picks sensory input node as the type they would like to create
    public void setCreator(NodeCreatorType type)
    {
        creatorType = type;

        switch (type)
        {
            case NodeCreatorType.siNodeCreator:
                // now node will be modified by siNodeCreator
                nodeCreator = new SensoryInputNodeCreator(new SensoryInputNode(parentNetCreator.parentCreatureCreator.creature), nodeLayer);
                break;
            case NodeCreatorType.commNodeCreator:
                nodeCreator = new CommNodeCreator(new CommInputNode(), nodeLayer);
                break;
            case NodeCreatorType.outputNodeCreator:
                nodeCreator = new OutputNodeCreator(new OutputNode(parentNetCreator.parentCreatureCreator.creature, parentNetCreator.network, nodeLayer), nodeLayer);
                break;
            case NodeCreatorType.innerInputNodeCreator:
                nodeCreator = new InnerInputNodeCreator(new InnerInputNode(), parentNetCreator.parentCreatureCreator.creature);
                break;
            default:
                Debug.LogError("unable to set node creator to that type.");
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
            case NodeCreatorType.innerInputNodeCreator:
                return (InnerInputNodeCreator)nodeCreator;
            case NodeCreatorType.outputNodeCreator:
                return (OutputNodeCreator)nodeCreator;
            default:
                Debug.LogError("not able to get that node creator");
                return null;
        }
    }

    public Node getCreatedNode()
    {
        return nodeCreator.getNode();
    }



}