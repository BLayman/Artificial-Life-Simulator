using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Allows creature to convert one set of resource into another.
/// </summary>
public class Convert : Action
{
    /// <summary>
    /// reactants with their coefficients
    /// </summary>
    public Dictionary<string, float> startResources = new Dictionary<string, float>();


    /// <summary>
    /// products with their coefficients
    /// </summary>
    public Dictionary<string, float> endResources = new Dictionary<string, float>();

    /// <summary>
    /// The amount of products to be made (before being multiplied by product coefficients).
    /// </summary>
    public float amtToProduce;


    /// <summary>
    /// convert a particular amount of startResources into endResources
    /// </summary>
    // TODO: write test for this
    public override void perform(Creature creature, Ecosystem eco)
    {
        // don't produce more than the creature can store
        foreach (string  key in endResources.Keys)
        {
            CreatureResource resource = creature.storedResources[key];
            float storageSpace = resource.maxLevel - resource.currentLevel;
            float toProduce = amtToProduce * endResources[key];
            if(storageSpace < toProduce)
            {
                amtToProduce = storageSpace / endResources[key];
            }
        }


        // determine how much of the products can actually be produced 
        // amount produced is the full amount intended, unless overriden below by a limiting reactant
        float actualAmtProduced = amtToProduce; 

        foreach(string resource in startResources.Keys)
        {
            float multipleNeeded = startResources[resource];
            float amtOfResRequired = amtToProduce * multipleNeeded;
            // if the creature doesn't have enough of a particular resource
            float actualStored = creature.storedResources[resource].currentLevel;
            // if there is not enough of the resource to produce the full amount
            if (actualStored < amtOfResRequired)
            {
                // calculate how much the resource can produce
                float amtProducedByRes = actualStored / multipleNeeded;
                // if that amount is less than the current amount actually produced, update the current amount
                if (amtProducedByRes < actualAmtProduced)
                {
                    actualAmtProduced = amtProducedByRes;
                }
            }
        }

        /* use the amount that can actaully be produced to update resource values */

        // update reactant levels
        foreach (string resource in startResources.Keys)
        {
            float amtUsed = startResources[resource] * actualAmtProduced;
            creature.storedResources[resource].currentLevel -= amtUsed;
        }

        // update product levels
        foreach (string resource in endResources.Keys)
        {
            float amtCreated = endResources[resource] * actualAmtProduced;
            creature.storedResources[resource].currentLevel += amtCreated;
        }

    }

}