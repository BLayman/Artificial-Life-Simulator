using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NetworkCreator
{
    public Network network;
    public NodeCreator nodeCreator;

    public NetworkCreator(Network _net)
    {
        network = _net;
        initializeNetwork();
    }

    public void initializeNetwork()
    {
        // add two empty lists to the network (inputs and outputs)
        network.net = new List<List<Node>>();
        network.net.Add(new List<Node>());
        network.net.Add(new List<Node>());
        // bias node added to input nodes
        network.net[0].Add(new BiasNode(1));

    }

    public void setName(string name) {
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

}