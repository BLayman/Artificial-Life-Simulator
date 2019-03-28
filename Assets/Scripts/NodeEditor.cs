// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum NodeCreatorType { siNodeCreator, commNodeCreator, outputNodeCreator, innerInputNodeCreator, internalResNodeEditor }


/// <summary>
/// API for storing different kinds of specific node creators. ex. - could wrap a SensoryInputNodeCreator. Stored by NetworkCreator.
/// </summary>
public class NodeEditor
{
    NodeEditorInterface nodeCreator;
    public int nodeLayer;
    NodeCreatorType creatorType;
    NetworkEditor parentNetCreator;

    // user picks layer to create node in, initializing a node creator
    public NodeEditor(int _layer, NetworkEditor parentNetCreator)
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
                nodeCreator = new SensoryInputNodeEditor(new SensoryInputNode(parentNetCreator.parentCreatureCreator.creature), nodeLayer);
                break;
            case NodeCreatorType.commNodeCreator:
                nodeCreator = new CommNodeEditor(new CommInputNode(), nodeLayer);
                break;
            case NodeCreatorType.outputNodeCreator:
                nodeCreator = new OutputNodeEditor(new OutputNode(parentNetCreator.parentCreatureCreator.creature, parentNetCreator.network, nodeLayer), nodeLayer);
                break;
            case NodeCreatorType.innerInputNodeCreator:
                nodeCreator = new InnerInputNodeEditor(new InnerInputNode(), parentNetCreator.parentCreatureCreator.creature);
                break;
            case NodeCreatorType.internalResNodeEditor:
                nodeCreator = new InternalResInputNodeEditor(new InternalResourceInputNode(parentNetCreator.parentCreatureCreator.creature));
                break;
            default:
                Debug.LogError("unable to set node creator to that type.");
                break;
        }
        // now this NodeCreator will now be passed to a gameObject ( or referenced by a gameObject) 
        // that only displays node edits for it's type and only uses the variable for that kind of nodeCreator      
    }

    public NodeEditorInterface getNodeCreator()
    {
        switch (creatorType)
        {
            case NodeCreatorType.siNodeCreator:
                return (SensoryInputNodeEditor)nodeCreator;
            case NodeCreatorType.commNodeCreator:
                return (CommNodeEditor)nodeCreator;
            case NodeCreatorType.innerInputNodeCreator:
                return (InnerInputNodeEditor)nodeCreator;
            case NodeCreatorType.outputNodeCreator:
                return (OutputNodeEditor)nodeCreator;
            case NodeCreatorType.internalResNodeEditor:
                return (InternalResInputNodeEditor)nodeCreator;
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