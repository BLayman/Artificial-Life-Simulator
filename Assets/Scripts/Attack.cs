// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Attack : Action
{
    public string victimSpecies;
    public float baseResourceFractionTaken;
    public float backfireHealthLost;
    
    // don't attempt action if none of the neighboring creatures are applicable
    public override bool isPossible(Creature c)
    {
        for (int i = 0; i < c.neighborLands.Length; i++)
        {
            if (c.neighborLands[i].creatureIsOn() && c.neighborLands[i].creatureOn.species.Equals(victimSpecies))
            {
                return true;
            }
        }
        return false;
    }
    

    public override void perform(Creature creature, Ecosystem eco)
    {
        // pick a random victim from the applicable neighbors
        List<Creature> potentialVictims = new List<Creature>();
        for (int i = 0; i < creature.neighborLands.Length; i++)
        {
            if (creature.neighborLands[i].creatureIsOn() && creature.neighborLands[i].creatureOn.species.Equals(victimSpecies))
            {
                potentialVictims.Add(creature.neighborLands[i].creatureOn);
            }
        }
        System.Random rand = new System.Random();
        int index = rand.Next(potentialVictims.Count);
        Creature victim = potentialVictims[index];

        try
        {
            int attackAbility = creature.abilities[victim.species + "Attack"].level;
            int defenseAbility = victim.abilities[creature.species + "Defense"].level;
            float fractionRes = baseResourceFractionTaken * (attackAbility + 1) / (defenseAbility + 1);

            float backfireProbability = (float)Math.Pow(2.0, defenseAbility) / 10;
            if(backfireProbability > 1)
            {
                backfireProbability = 1;
            }
            float randomNum = (float) rand.NextDouble();
            if(randomNum <= backfireProbability)
            {
                //Debug.Log("attack backfired");
                creature.health -= backfireHealthLost;
            }
            else
            {
                // for each of victim's resources
                foreach (string res in victim.storedResources.Keys)
                {
                    // if creature can store that resource, then take the resource from the victim
                    if (creature.storedResources.ContainsKey(res))
                    {
                        float resourceTaken = victim.storedResources[res].currentLevel * fractionRes;
                        creature.storedResources[res].currentLevel += resourceTaken;
                        victim.storedResources[res].currentLevel -= resourceTaken;
                        // if surpased max level, set to max
                        if (creature.storedResources[res].currentLevel > creature.storedResources[res].maxLevel)
                        {
                            creature.storedResources[res].currentLevel = creature.storedResources[res].maxLevel;
                        }
                    }
                }

                //Debug.Log("attack succeeded");
            }

        }
        catch(KeyNotFoundException e)
        {
            Debug.LogError("Attack or defense ability not found. Make sure to use <species name>Attack or <species name>Defense when setting these abilities");
            Debug.Log("Attacking creature abilities:");
            foreach (string key in creature.abilities.Keys)
            {
                Debug.Log(key);
            }
            Debug.Log("Defending creature abilities:");
            foreach (string key in victim.abilities.Keys)
            {
                Debug.Log(key);
            }
        }

    }
}