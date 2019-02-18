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

    // netName: name of connected network, outLayerNodeIndex : index of connected node in it's layer, netLayer: layer in which connected network exists
    public void setLinkedNode(string netName, int outLayerNodeIndex, int netLayer)
    {
        // get last layer of connected network
        int outLayerIndex = parentCreature.networks[netLayer][netName].net.Count - 1;
        // use last layer index and index of connected node in the layer to get linked node
        node.linkedNode = parentCreature.networks[netLayer][netName].net[outLayerIndex][outLayerNodeIndex];
        node.linkedNetName = netName;
        node.linkedNodeIndex = outLayerNodeIndex;
        node.linkedNodeNetworkLayer = netLayer;
        node.parentCreature = parentCreature;
    }
}