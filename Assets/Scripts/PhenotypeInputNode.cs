using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PhenotypeInputNode : Node
{
    public Creature parentCreat;

    /// <summary>
    /// phenotype we are detecting
    /// </summary>
    public bool[] phenotype;

    public int neighborIndex;
    /// <summary>
    /// Index in network layer
    /// </summary>
    public int index;

    public PhenotypeInputNode(int _index, Creature parent)
    {
        index = _index;
        parentCreat = parent;
        phenotype = new bool[parent.phenotype.Length];
    }

    // called from creature class
    public void setPhenotype(bool[] pheno)
    {
        // if node is part of the phenotype encoding
        if(index < pheno.Length)
        {
            phenotype = pheno;
            if (phenotype[index] == false)
            {
                value = 0;
            }
            else
            {
                value = 1;
            }
        }
        // else node is part of the neighbor location encoding
        else
        {
            // subtract phenotype length from index to get new index starting at 0
            // also subtract 1 from neightbor index to make it [0,4] instead of [1,5]
            if(index - pheno.Length == neighborIndex - 1)
            {
                value = 1;
            }
            else
            {
                value = 0;
            }
        }
        
    }

    public void setNeightborIndex(int i)
    {
        neighborIndex = i;
    }

    public override void updateValue()
    {
        // value is a constant set in setPhenotype
        
    }
}