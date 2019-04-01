// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class InnerInputNode : Node
{
    public Node linkedNode;
    public string linkedNetName;
    public Creature parentCreature;
    public int linkedNodeIndex;
    public int linkedNodeNetworkLayer;
    public int outLayerIndex;
    public bool temp = false;


    public void setLinkedNode(Creature parent, int netLayer, string netName, int outLayer, int outLayerNodeIndex)
    {
        parentCreature = parent;
        linkedNodeNetworkLayer = netLayer;
        linkedNetName = netName;

        outLayerIndex = outLayer;
        linkedNodeIndex = outLayerNodeIndex;
        linkedNode = parentCreature.networks[linkedNodeNetworkLayer][linkedNetName].net[outLayerIndex][linkedNodeIndex];
    }

    /// <summary>
    /// This method simply gets the value from inNode.
    /// </summary>
    public override void updateValue()
    {
        value = linkedNode.value;
    }

    public InnerInputNode clone()
    {
        return (InnerInputNode) this.MemberwiseClone();
    }
}