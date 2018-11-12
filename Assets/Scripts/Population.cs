using System.Collections;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Population
{
    public List<Creature> creatures;
    public Creature founder;
    public float weightStandardDev;
    public float abilityStandardDev;

    public Creature generateMember()
    {
        Creature c = founder.getCopy();
        // TODO: add variation to creature weights and abilities
        //founder.addVariationToWeights(weightStandardDev);

        return c;
    }
}
