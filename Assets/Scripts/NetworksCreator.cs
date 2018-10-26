using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NetworksCreator
{

    /// <summary>
    /// Networks to be created and edited.
    /// </summary>
    private List<Dictionary<string, Network>> networks;

    public NodeCreator nodeCreator;

    public NetworksCreator(List<Dictionary<string, Network>> _networks)
    {
        networks = _networks;
        initializeNetworkStructure();
    }



    private void initializeNetworkStructure()
    {
        networks = new List<Dictionary<string, Network>>();
        networks.Add(new Dictionary<string, Network>());
        networks.Add(new Dictionary<string, Network>());
    }


    public void addNetsToLayer(int layerIndex, int numberOfNets, Network template)
    {
        int count = 1;
        for (int i = 0; i < numberOfNets; i++)
        {
            networks[layerIndex].Add(template.name + "_" +  count, template);
        }
    }


}