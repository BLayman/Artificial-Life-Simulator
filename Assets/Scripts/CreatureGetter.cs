using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        return creature.mutationStandardDeviation.ToString();
    }

}