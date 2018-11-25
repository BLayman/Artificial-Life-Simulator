using System.Collections;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;


public class Population
{
    public List<Creature> creatures;
    public Creature founder;
    public float weightStandardDev;
    public float abilityStandardDev;
    public int size = 0;

    public Creature generateMember()
    {
        //Debug.Log("founder species: " + founder.species);
        Creature c = founder.getCopy();
        //Debug.Log("copy species: " + c.species);
        return c;
    }
}
