// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PhenotypeNetwork : Network
{

    public Creature parentCreature;


    public void setInputNodes(bool[] phenotype, int neighborIndex)
    {
        //Debug.Log("copied first layer length: " + net[0].Count);
        // starts at 1 because of bias node
        for (int i = 1; i < net[0].Count; i++)
        {
            PhenotypeInputNode node = (PhenotypeInputNode) net[0][i];
            node.setNeightborIndex(neighborIndex);
            node.setPhenotype(phenotype);
            
        }
    }

    public PhenotypeNetwork getShallowCopy()
    {
        return (PhenotypeNetwork) this.MemberwiseClone();
    }

}