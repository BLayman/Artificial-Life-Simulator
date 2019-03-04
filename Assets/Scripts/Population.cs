using System.Collections;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A population stores information about a list of creatures, including it's founder, and initial variation.
/// </summary>
public class Population
{
    public List<Creature> creatures;
    public List<Creature> offspring = new List<Creature>();
    public Creature founder;
    public float weightStandardDev;
    public float abilityStandardDev;
    public int size = 0;
    public int maxSize = 1000;

    public Creature generateMember()
    {
        size++;
        //Debug.Log("founder species: " + founder.species);
        Creature c = Copier.getCreatureCopy(founder);
        //Debug.Log("copy species: " + c.species);
        return c;
    }

    public Population shallowCopy()
    {
        return (Population)this.MemberwiseClone();
    }
}
