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

    public static Creature getCreatureCopy(Creature c)
    {
        Creature creatureCopy = c.getShallowCopy();

        // copy creature networks
        List<Dictionary<string, Network>> networks = new List<Dictionary<string, Network>>();
        creatureCopy.networks = networks;
        List<Dictionary<string, Network>> origNetworks = c.networks;

        copyCreatureNetworks(origNetworks, networks, creatureCopy);

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

        creatureCopy.storedResources = new Dictionary<string, CreatureResource>();
        foreach (string resKey in c.storedResources.Keys)
        {
            creatureCopy.storedResources[resKey] = c.storedResources[resKey].getShallowCopy();
        }
        creatureCopy.phenotype = new bool[c.phenotype.Length];
        Array.Copy(c.phenotype, creatureCopy.phenotype, c.phenotype.Length);
        creatureCopy.inputCommList = new List<CommSignal>();

        creatureCopy.commInNetTemplate = c.commInNetTemplate.getShallowCopy();
        creatureCopy.commInNetTemplate.net = new List<List<Node>>();
        List<List<Node>> newCommNet = creatureCopy.commInNetTemplate.net;
        List<List<Node>> origCommNet = c.commInNetTemplate.net;

        for (int j = 0; j < origCommNet.Count; j++)
        {
            newCommNet.Add(new List<Node>());
            for (int k = 0; k < origCommNet[j].Count; k++)
            {
                newCommNet[j].Add(getNewNode(origCommNet[j][k], creatureCopy, creatureCopy.commInNetTemplate));
            }
        }


        creatureCopy.commOutNetTemplate = c.commOutNetTemplate.getShallowCopy();
        creatureCopy.commOutNetTemplate.net = new List<List<Node>>();
        List<List<Node>> newCommOutNet = creatureCopy.commOutNetTemplate.net;
        List<List<Node>> origCommOutNet = c.commOutNetTemplate.net;

        for (int j = 0; j < origCommOutNet.Count; j++)
        {
            newCommOutNet.Add(new List<Node>());
            for (int k = 0; k < origCommOutNet[j].Count; k++)
            {
                newCommOutNet[j].Add(getNewNode(origCommOutNet[j][k], creatureCopy, creatureCopy.commOutNetTemplate));
            }
        }

        creatureCopy.reproductionRequests = new List<ReproAction>();

        creatureCopy.actionPool = new Dictionary<string, Action>();
        foreach (string key in c.actionPool.Keys)
        {
            creatureCopy.actionPool[key] = getNewAction(c.actionPool[key]);
        }

        // TODO: create instances of reference variables
        return creatureCopy;
    }

    public static void copyCreatureNetworks(List<Dictionary<string, Network>> origNetworks, List<Dictionary<string, Network>> networks, Creature creatureCopy)
    {
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
                    for (int k = 0; k < origNet[j].Count; k++)
                    {
                        newNet[j].Add(getNewNode(origNet[j][k], creatureCopy, dict[key]));
                    }
                }
            }
        }
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

    public static Node getNewNode(Node oldNode, Creature creatureCopy, Network parentNet)
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
            newNode.parentNet = parentNet;
            newNode.parentCreature = creatureCopy;
            newNode.action = getNewAction(oldNode2.action);
            newNode.prevNodes = new List<Node>();
            newNode.assignPrevNodes();
            newNode.weights = new List<float>();
            for (int i = 0; i < oldNode2.weights.Count; i++)
            {
                newNode.weights.Add(oldNode2.weights[i]);
            }
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
            /*
            Debug.Log("linked network layer number: " + oldNode2.linkedNodeNetworkLayer);
            Debug.Log("net name: " + oldNode2.linkedNetName);
            Debug.Log("node index: " + newNode.linkedNodeIndex);
            */
            Network linkedNetwork = creatureCopy.networks[oldNode2.linkedNodeNetworkLayer][oldNode2.linkedNetName];
            newNode.linkedNode = linkedNetwork.net[linkedNetwork.net.Count - 1][newNode.linkedNodeIndex];
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
