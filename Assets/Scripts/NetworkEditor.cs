// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// API for Network objects. Stored by EcosystemCreator.
/// </summary>
public class NetworkEditor
{
    public Network network;
    public NodeEditor nodeCreator;
    public CreatureEditor parentCreatureCreator;

    public NetworkEditor(Network net, CreatureEditor parentCreatureCreator)
    {
        network = net;
        this.parentCreatureCreator = parentCreatureCreator;
        initializeNetwork();
    }

    public void initializeNetwork()
    {
        // add two empty lists to the network (inputs and outputs)
        network.net.Add(new List<Node>());
        network.net.Add(new List<Node>());
        // bias node added to input nodes
        BiasNode bn = new BiasNode();
        bn.setBias(1);
        // TODO: remember to add a bias node whenever a hidden layer is created
        network.net[0].Add(bn);

    }

    public void insertNewLayer(int layerIndex)
    {
        network.net.Insert(layerIndex, new List<Node>());
    }

    public void setName(string name)
    {
        network.name = name;
    }

    public void setInLayer(int layer)
    {
        network.inLayer = layer;
    }

    public void saveNode()
    {
        network.net[nodeCreator.nodeLayer].Add(nodeCreator.getCreatedNode());
    }

    public NodeEditor addNode(int layer)
    {
        //Debug.Log("called addNode");
        nodeCreator = new NodeEditor(layer, this);
        return nodeCreator;
    }
}