using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Priority_Queue;

public class Utility
{



    public static System.Random rand = new System.Random();
    /*
    public static object getDeepCopy(object toCopy)
    {
        Type sourceType = toCopy.GetType();
        object instance = Activator.CreateInstance(sourceType);
        PropertyInfo[] properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            if (property.CanWrite)
            {
                if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                {
                    property.SetValue(instance, property.GetValue(toCopy, null), null);
                }
                else
                {
                    object propertyVal = property.GetValue(toCopy, null);
                    if (propertyVal == null)
                    {
                        property.SetValue(instance, null, null);
                    }
                    else
                    {
                        property.SetValue(instance, getDeepCopy(propertyVal), null);
                    }
                }
            }
        }
        return instance;
    }*/

    public static float normRand(float sd)
    {
        double u1 = 1.0 - rand.NextDouble();
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        //Debug.Log("generated normal rv: " + randStdNormal);
        return (float) (sd * randStdNormal);

    }

    public Creature getCreatureCopy(Creature c)
    {
        Creature creatureCopy = c.getShallowCopy();

        // copy creature networks
        List<Dictionary<string, Network>> networks = new List<Dictionary<string, Network>>();
        creatureCopy.networks = networks;
        List<Dictionary<string, Network>> origNetworks = c.networks;

        for (int i = 0; i < origNetworks.Count; i++)
        {
            Dictionary<string, Network> dict = new Dictionary<string, Network>();
            networks.Add(dict);
            foreach (string key in origNetworks[i].Keys)
            {
                dict[key] = origNetworks[i][key].getShallowCopy();
                List<List<Node>> origNet = origNetworks[i][key].net;
                List<List<Node>> newNet = new List<List<Node>>();
                dict[key].net = newNet;
                for (int j = 0; j < origNet.Count; j++)
                {
                    newNet.Add(new List<Node>());
                    for (int k = 0; k < origNet[i].Count; k++)
                    {
                        newNet[j].Add(getNewNode(origNet[i][j], creatureCopy));
                    }
                }
            }
        }

        creatureCopy.position = new int[2];
        creatureCopy.neighborLands = new Land[5];
        creatureCopy.actionQueue = new SimplePriorityQueue<Action>();
        creatureCopy.outputCommSignals = new List<CommSignal>();
        creatureCopy.prevNetStates = new List<List<Network>>();
        creatureCopy.abilities = new Dictionary<string, Ability>();

        // copy abilities
        foreach (string ability in c.abilities.Keys)
        {
            Ability oldAbility = c.abilities[ability];
            Ability newAbility = getNewAbility(oldAbility);
            creatureCopy.abilities[ability] = newAbility;
        }

        // Next: storedResources


        // TODO: create instances of reference variables
        return creatureCopy;
    }

    public static Ability getNewAbility(Ability oldAbility)
    {
        Ability newAbility = oldAbility.getShallowCopy();
        newAbility.boostOptions = new List<BoostRequirement>();
        for (int i = 0; i < oldAbility.boostOptions.Count; i++)
        {
            BoostRequirement newBoostReq = oldAbility.boostOptions[i].getShallowCopy();
            newAbility.boostOptions.Add(newBoostReq);
            newBoostReq.requiredResources = new Dictionary<string, int>();
            foreach (string key in oldAbility.boostOptions[i].requiredResources.Keys)
            {
                newAbility.boostOptions[i].requiredResources[key] = oldAbility.boostOptions[i].requiredResources[key];
            }
        }
        return newAbility;
    }


    public static Action getNewAction(Action oldAction)
    {
        Action newAction = oldAction.getShallowCopy();
        Dictionary<string, float> dict = new Dictionary<string, float>();
        newAction.resourceCosts = dict;
        foreach (string key in oldAction.resourceCosts.Keys)
        {
            dict[key] = oldAction.resourceCosts[key];
        }
        return newAction;
    }

    public static Node getNewNode(Node oldNode, Creature creatureCopy)
    {
        if (oldNode.GetType().Name == "SensoryInputNode")
        {
            SensoryInputNode oldNode2 = (SensoryInputNode)oldNode;
            SensoryInputNode newNode = oldNode2.clone();
            newNode.creature = creatureCopy;
            return newNode;
            
        }
        else if (oldNode.GetType().Name == "OutputNode")
        {
            OutputNode oldNode2 = (OutputNode)oldNode;
            OutputNode newNode = oldNode2.clone();
            newNode.parentCreature = creatureCopy;
            newNode.action = getNewAction(oldNode2.action);
            return newNode;
        }
        else if (oldNode.GetType().Name == "BiasNode")
        {
            BiasNode oldNode2 = (BiasNode)oldNode;
            BiasNode newNode = oldNode2.clone();
            return newNode;
        }

        else if (oldNode.GetType().Name == "InnerInputNode")
        {
            InnerInputNode oldNode2 = (InnerInputNode)oldNode;
            InnerInputNode newNode = oldNode2.clone();
            newNode.parentCreature = creatureCopy;
            Network network = creatureCopy.networks[newNode.linkedNodeNetworkLayer][newNode.netName];
            newNode.linkedNode = network.net[network.net.Count][newNode.linkedNodeIndex];
            return newNode;
        }
        else if (oldNode.GetType().Name == "CommInputNode")
        {
            //TODO 
            return null;
        }
        else if (oldNode.GetType().Name == "MemoryInputNode")
        {
            //TODO
            return null;
        }
        else
        {
            Debug.LogError("Didn't find correct type of node to add");
            return null;
        }

    }

}
