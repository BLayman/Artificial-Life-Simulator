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
        phenotype = pheno;
    }

    public void setNeightborIndex(int i)
    {
        neighborIndex = i;
    }

    public override void updateValue()
    {

        if(phenotype[index] == false)
        {
            value = 0;
        }
        else
        {
            value = 1;
        }
    }
}