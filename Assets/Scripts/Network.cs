using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// A neural network of a creature, which consists of layers of nodes.
/// </summary>
public class Network
{
    public List<List<Node>> net = new List<List<Node>>();

    // used later to place network in the correct part of the creature's networks
    public string name;
    public int inLayer;

    /*
    public Network(string _name)
    {
        name = _name;
    }

    /// <summary>
    /// Connects two nodes in the network together
    /// </summary>
    public bool connect(int fromI, int fromj, int toI, int toJ)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Appends node sent as argument to first layer of net.
    /// </summary>
    public void addInput(Node inputNode)
    {
        throw new System.NotImplementedException();
    }

    public bool addHiddenLayer(int layerIndex, int numNodes)
    {
        throw new System.NotImplementedException();
    }

    public bool appendHiddenNode(int layerIndex)
    {
        throw new System.NotImplementedException();
    }

    public void addOutput(Node outputNode)
    {
        throw new System.NotImplementedException();
    }

    public bool disconnect(int fromI, int fromJ, int toI, int toJ)
    {
        throw new System.NotImplementedException();
    }

    public void assignInputs()
    {
        throw new System.NotImplementedException();
    }
    */

    /// <summary>
    /// Calls update value on each node in each layer
    /// </summary>
    public void feedForward()
    {
        // for every layer
        for (int i = 0; i < net.Count; i++)
        {
            // for every node
            for (int j = 0; j < net[i].Count; j++)
            {
                // update the value of that node
                net[i][j].updateValue();
            }
        }
    }

    public Network getShallowCopy()
    {
        return (Network)this.MemberwiseClone();
    }
}
