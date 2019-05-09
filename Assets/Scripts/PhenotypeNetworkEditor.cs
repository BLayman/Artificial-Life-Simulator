using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PhenotypeNetworkEditor : NetworkEditor
{

    public PhenotypeNetworkEditor(PhenotypeNetwork net, CreatureEditor parentCreatureCreator) : base(net, parentCreatureCreator)
    {
    }

    public void createInputNodes()
    {
        // adds an input node for each phenotype bit
        for (int i = 0; i < parentCreatureCreator.creature.phenotype.Length + 4; i++)
        {
            //Debug.Log("created phenotype node " + i);
            network.net[0].Add(new PhenotypeInputNode(i, parentCreatureCreator.creature));
        }

    }


}