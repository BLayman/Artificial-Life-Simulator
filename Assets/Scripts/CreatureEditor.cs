using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <remarks>API for creature class. Stored by EcosystemCreator.</remarks>
public class CreatureEditor
{
    public Creature creature;
    public NetworkEditor netCreator;
    public ResourceEditor resourceCreator;
    public AbilitiesEditor abilitiesCreator;
    public ActionEditor actionCreator;
    public int distinctPhenotypes;
    EcosystemEditor ecoCreator;

    Dictionary<string, Ability> tentativeAbilities;

    /// <summary>
    /// Constructor
    /// </summary>
    public CreatureEditor(Creature creature, EcosystemEditor ecoCreator)
    {
        this.creature = creature;
        initializeNetworkStructure();
        this.distinctPhenotypes = ecoCreator.ecosystem.distictPhenotypes;
        this.ecoCreator = ecoCreator;
        tentativeAbilities = new Dictionary<string, Ability>();

        // copy abilities in from creature
        foreach (string ability in creature.abilities.Keys)
        {
            tentativeAbilities[ability] = creature.abilities[ability];
        }

    }



    /// <summary>
    /// Initializes networks of creature to have two rows of networks.
    /// </summary>
    private void initializeNetworkStructure()
    {
        // currently hard coded to support 2 layers of networks (input processing and action recommending)
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
        creature.position[0] = xCoor;
        creature.position[1] = yCoor;
    }

    public void setMaxHealth(float maxHealth)
    {
        creature.maxHealth = maxHealth;
    }

    public void setInitialHealth(float initialHealth)
    {
        creature.health = initialHealth;
    }

    /// <summary>
    /// Sets phenotype of creature. Takes in int: [1,x], where there are x distinct phenotypes allowed in the ecosystem,
    /// and assigns corrresponding bool array to creatures phenotype. The bool array contains all falses, except for a true at the x-1 index.
    /// </summary>
    /// <param name="phenotype">Must be 1 - x: Where there are x distinct phenotypes allowed in the ecosystem (set in Ecosystem class).</param>
    public void setPhenotype(int phenotype)
    {
        if (phenotype > distinctPhenotypes)
        {
            Debug.LogError("Phenotype entered excedes max range of possible phenotypes");
        }
        else
        {
            creature.phenotype = new bool[distinctPhenotypes];
            creature.phenotype[phenotype - 1] = true;
        }

    }

    /// <summary>
    /// Sets the amount of time alloted per turn.
    /// </summary>
    public void setTurnTime(float time)
    {
        creature.fullTurnTime = time;
        creature.remainingTurnTime = time;
    }

    /// <summary>
    /// Resets creatures resource creator, allowing the user to create a resource for the creature to use for survival or attribute benefits.
    /// </summary>
    public ResourceEditor addResource()
    {
        resourceCreator = new ResourceEditor(new CreatureResource());
        return resourceCreator;
    }

    /// <summary>
    /// Saves resource created by resource creator.
    /// </summary>
    public void saveResource()
    {
        creature.storedResources.Add(resourceCreator.resource.name, resourceCreator.resource);
    }

    /// <summary>
    /// edit tentativeAbilities, returns AbilitesEditor
    /// </summary>
    public AbilitiesEditor editAbilities()
    {
        abilitiesCreator = new AbilitiesEditor(tentativeAbilities, creature.remainingAbilityPoints);
        return abilitiesCreator;
    }

    /// <summary>
    /// reset tentativeAbilities if user cancels editing them
    /// </summary>
    public void cancelEditingAbilities()
    {
        tentativeAbilities = new Dictionary<string, Ability>();

        // copy abilities in from creature
        foreach (string ability in creature.abilities.Keys)
        {
            tentativeAbilities[ability] = creature.abilities[ability];
        }
    }

    public void addDefaultResourceAbilities()
    {
        abilitiesCreator = new AbilitiesEditor(tentativeAbilities, creature.remainingAbilityPoints);
        foreach (string resource in creature.storedResources.Keys)
        {
            abilitiesCreator.addAbility(resource, abilityType.comsumption);
        }
    }

    /// <summary>
    /// Resets netCreator, allowing for the creation of a new network.
    /// </summary>
    public NetworkEditor addNetwork()
    {
        netCreator = new NetworkEditor(new Network(), this);
        return netCreator;
    }

    public ActionEditor addAction()
    {
        actionCreator = new ActionEditor(this);
        return actionCreator;
    }

    public void saveAction()
    {
        creature.actionPool.Add(actionCreator.getCreatedAction().name, actionCreator.getCreatedAction());
    }

    /// <summary>
    /// Saves abilities set by abilities creator.
    /// </summary>
    public void saveAbilities()
    {
        creature.abilities = tentativeAbilities;
    }



    public void generateDefaultActionPool()
    {
        // create move up action
        ActionEditor ac = addAction();
        ac.setCreator(ActionCreatorType.moveActionCreator);
        MoveActionEditor mac = (MoveActionEditor)ac.getActionCreator();
        mac.setName("moveUp");
        mac.setDirection(moveDir.up);
        mac.setPriority(1);
        mac.setTimeCost(10);
        mac.addResourceCost("grass", 1);
        saveAction();

        // create move up action
        ActionEditor acd = addAction();
        acd.setCreator(ActionCreatorType.moveActionCreator);
        MoveActionEditor macd = (MoveActionEditor)acd.getActionCreator();
        macd.setName("moveDown");
        macd.setDirection(moveDir.down);
        macd.setPriority(1);
        macd.setTimeCost(10);
        macd.addResourceCost("grass", 1);
        saveAction();

        // create move up action
        ActionEditor acl = addAction();
        acl.setCreator(ActionCreatorType.moveActionCreator);
        MoveActionEditor macl = (MoveActionEditor)acl.getActionCreator();
        macl.setName("moveLeft");
        macl.setDirection(moveDir.left);
        macl.setPriority(1);
        macl.setTimeCost(10);
        macl.addResourceCost("grass", 1);
        saveAction();

        // create move up action
        ActionEditor acr = addAction();
        acr.setCreator(ActionCreatorType.moveActionCreator);
        MoveActionEditor macr = (MoveActionEditor)acr.getActionCreator();
        macr.setName("moveRight");
        macr.setDirection(moveDir.right);
        macr.setPriority(1);
        macr.setTimeCost(10);
        macr.addResourceCost("grass", 1);
        saveAction();

    }
}