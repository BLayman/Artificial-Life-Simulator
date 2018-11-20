using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerInputNodeCreator : NodeCreatorInterface
{
    InnerInputNode node;
    Creature parentCreature;

    public InnerInputNodeCreator(InnerInputNode node, Creature parentCreature)
    {
        this.parentCreature = parentCreature;
        this.node = node;
    }

    public Node getNode()
    {
        return node;
    }

    public void setLinkedNode(string netName, int outLayerNodeIndex)
    {
        int outLayerIndex = parentCreature.networks[0][netName].net.Count - 1;
        node.linkedNode = parentCreature.networks[0][netName].net[outLayerIndex][outLayerNodeIndex]; 
    }
}