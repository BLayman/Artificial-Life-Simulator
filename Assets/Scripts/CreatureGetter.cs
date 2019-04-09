using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CreatureGetter
{
    public Creature creature;

    public CreatureGetter(Creature creature)
    {
        this.creature = creature;
    }

    public string getId()
    {
        return creature.iD;
    }

    public string getSpecies()
    {
        return creature.species;
    }

    public string getHealth()
    {
        return creature.health.ToString();
    }

    public string getMaxHealth()
    {
        return creature.maxHealth.ToString();
    }

    public Dictionary<string, CreatureResource> getResources()
    {
        return creature.storedResources;
    }

    public string getMutationAmt()
    {
        return (creature.actualMutationDeviation).ToString();
    }

    public List<Dictionary<string, Network>> getNets()
    {
        return creature.networks;
    }

    public SimplePriorityQueue<Action> getActions()
    {
        Debug.Log(creature.iD);
        Debug.Log(creature.actionQueue.Count);
        return creature.actionQueue;
    }

}