using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SensoryInputNode : Node
{
    /// <summary>
    /// specifies which neighbor Land (0 - 8) this node gets it's input from: 0 for currentPos
    /// </summary>
    public int neighborLandIndex;
    /// <summary>
    /// string designating resource to be found in neighbor dictionary
    /// </summary>
    public string sensedResource;
    /// <summary>
    /// stores a reference to the creature it belongs to (for getting neighbors)
    /// </summary>
    public Creature creature;


    public SensoryInputNode() { }

    public SensoryInputNode(Creature creature) {
        this.creature = creature;
    }

    public void setCreature(Creature parentCreature)
    {
        creature = parentCreature;
    }


    /// <summary>
    /// Uses neighborIndex and key with propertyDict to get an updated value
    /// </summary>
    public float senseValFromNeighbor()
    {
        Dictionary<string,ResourceStore> properties = creature.neighborLands[neighborLandIndex].propertyDict;
        /*
        Debug.Log(properties.Keys.Count);
        foreach (string key in properties.Keys)
        {
            Debug.Log(key);
        }
        */

        //Debug.Log("sensing: " + sensedResource);
        if (!creature.neighborLands[neighborLandIndex].isDummy)
        {
            if (properties.ContainsKey(sensedResource))
            {
                //Debug.Log("sensed: " + properties[sensedResource].amountStored);
               //Debug.Log("out of possible: " + properties[sensedResource].maxAmount);
                return (float) properties[sensedResource].amountStored / (float) properties[sensedResource].maxAmount;
            }
            else
            {
                return 0; // creature percieves wall as having no resource
            }
            
        }
        else
        {
            return 0;
        }

    }

    /// <summary>
    /// calls senseValFromNeighbor to updateValue
    /// </summary>
    public override void updateValue()
    {
        float amount = senseValFromNeighbor();
        //Debug.Log("creature " + creature.index + " sensed " + amount + " of " + sensedResource);
        value = amount;
    }

    public SensoryInputNode clone()
    {
        return (SensoryInputNode) this.MemberwiseClone();
    }
}