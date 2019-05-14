// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// API for InnerInputNodes.
/// </summary>
public class InnerInputNodeEditor : NodeEditorInterface
{
    InnerInputNode iiNode;
    Creature parentCreature;

    public InnerInputNodeEditor(InnerInputNode node, Creature parentCreature)
    {
        this.parentCreature = parentCreature;
        this.iiNode = node;
    }

    public Node getNode()
    {
        return iiNode;
    }


    // netName: name of connected network, outLayerNodeIndex : index of connected node in it's layer, netLayer: layer in which connected network exists
    public void setLinkedNode(string netName, int outLayerNodeIndex, int netLayer)
    {
        if (!parentCreature.networks[netLayer].ContainsKey(netName))
        {
            Debug.LogError("invalid network name: " + netName);
        }
        // get last layer of connected network
        int outLayerIndex = parentCreature.networks[netLayer][netName].net.Count - 1;
        // use last layer index and index of connected node in the layer to get linked node
        iiNode.setLinkedNode(parentCreature, netLayer, netName, outLayerIndex, outLayerNodeIndex);
        
    }
}