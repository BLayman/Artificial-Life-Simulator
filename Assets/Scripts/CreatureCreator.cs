using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CreatureCreator
{
    public Creature creature;
    public NetworkCreator netCreator;
    public ResourceCreator resourceCreator;
    public AbilitiesCreator abilitiesCreator;

    /// <summary>
    /// Constructor
    /// </summary>
    public CreatureCreator(Creature _creature)
    {
        creature = _creature;
        initializeNetworkStructure();
    }


    /// <summary>
    /// Initializes networks of creature to have two rows of networks.
    /// </summary>
    private void initializeNetworkStructure()
    {
        creature.networks.Add(new Dictionary<string, Network>());
        creature.networks.Add(new Dictionary<string, Network>());
    }

    /// <summary>
    /// Sets name of species to which the creature belongs.
    /// </summary>
    public void setSpecies(string species)
    {
        creature.species = species;
    }

    /// <summary>
    /// Used to save network that is created by NetCreator.
    /// </summary>
    public void saveNetwork()
    {
        // TODO: account for comm network templates
        creature.networks[netCreator.network.inLayer][netCreator.network.name] = netCreator.network;
    }

    /// <summary>
    /// Sets creature's position on the map.
    /// </summary>
    public void setPosition(int xCoor, int yCoor)
    {
        throw new System.NotImplementedException();
    }

    public void setMaxHealth(int maxHealth)
    {
        throw new System.NotImplementedException();
    }

    public void setInitialHealth(int initialHealth)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Sets phenotype of creature. Takes in int: [0,x], where there are x distinct phenotypes allowed in the ecosystem,
    /// and assigns corrresponding bool array to creatures phenotype. The bool array contains all zeros, except for a 1 at the x index.
    /// </summary>
    /// <param name="phenotype">Must be 0 - x: Where there are x distinct phenotypes allowed in the ecosystem (set in Ecosystem class).</param>
    public void setPhenotype(int phenotype)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Sets the amount of time alloted per turn.
    /// </summary>
    public void setTurnTime(int time)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Resets creatures resource creator, allowing the user to create a resource for the creature to use for survival or attribute benefits.
    /// </summary>
    public void addCreatureResource(CreatureResource resource)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Saves resource created by resource creator.
    /// </summary>
    public void saveResource()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Resets and returns abilitiesCreator.
    /// </summary>
    public void addAbilities()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Resets netCreator, allowing for the creation of a new network.
    /// </summary>
    public void addNetwork()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Saves abilities set by abilities creator.
    /// </summary>
    public void saveAbilities()
    {
        throw new System.NotImplementedException();
    }
}