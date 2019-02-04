using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// API for InnerInputNodes.
/// </summary>
public class InnerInputNodeEditor : NodeEditorInterface
{
    InnerInputNode node;
    Creature parentCreature;

    public InnerInputNodeEditor(InnerInputNode node, Creature parentCreature)
    {
        this.parentCreature = parentCreature;
        this.node = node;
    }

    public Node getNode()
    {
        return node;
    }

    public void setLinkedNode(string netName, int outLayerNodeIndex, int netLayer)
    {
        int outLayerIndex = parentCreature.networks[netLayer][netName].net.Count - 1;
        node.linkedNode = parentCreature.networks[netLayer][netName].net[outLayerIndex][outLayerNodeIndex];
        node.linkedNetName = netName;
        node.linkedNodeIndex = outLayerNodeIndex;
        node.linkedNodeNetworkLayer = netLayer;
        node.parentCreature = parentCreature;
    }
}