using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Network
{
    public List<List<Node>> net;

    public string name;
    public int inLayer;

    /*
    public Network(string _name)
    {
        name = _name;
    }
    */

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

    /// <summary>
    /// Calls update value on each node in each layer
    /// </summary>
    public void feedForward()
    {
        throw new System.NotImplementedException();
    }
}
