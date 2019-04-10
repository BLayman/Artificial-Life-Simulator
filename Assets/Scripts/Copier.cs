// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Priority_Queue;



public class Copier
{

    public static System.Random rand = new System.Random();
    public static System.Random seedGen = new System.Random();
    public static int seed = 0;
    public static int creatureNum = 0;

    private static System.Object randLock = new System.Object();
    private static System.Object seedGenLock = new System.Object();

    public static Ecosystem getEcosystemCopy(Ecosystem eco)
    {
        Ecosystem copy = eco.shallowCopy();
        // copy map, but removing creatures
        copy.map = new List<List<Land>>();
        for (int i = 0; i < eco.map.Count; i++)
        {
            copy.map.Add(new List<Land>());
            for (int j = 0; j < eco.map[i].Count; j++)
            {
                // copy each land
                Land landCopy = GetLandCopy(eco.map[i][j]);
                copy.map[i].Add(landCopy);
            }
        }
        
        // copy populations and add creatures back to map
        copy.populations = new Dictionary<string, Population>();
        foreach (string popName in eco.populations.Keys)
        {

            // get shallow copy of each populaiton
            copy.populations[popName] = eco.populations[popName].shallowCopy();

            // copy weights by creature
            copy.populations[popName].weightsByCreature = new List<List<float>>();
            for (int i = 0; i < eco.populations[popName].weightsByCreature.Count; i++)
            {
                copy.populations[popName].weightsByCreature.Add(new List<float>());
                for (int j = 0; j < eco.populations[popName].weightsByCreature[i].Count; j++)
                {
                    copy.populations[popName].weightsByCreature[i].Add(eco.populations[popName].weightsByCreature[i][j]);
                }
            }

            // copy averages
            copy.populations[popName].weightAverages = new List<float>();
            for (int i = 0; i < eco.populations[popName].weightAverages.Count; i++)
            {
                copy.populations[popName].weightAverages.Add(eco.populations[popName].weightAverages[i]);
            }

            // copy averages
            copy.populations[popName].weightSDs = new List<float>();
            for (int i = 0; i < eco.populations[popName].weightSDs.Count; i++)
            {
                copy.populations[popName].weightSDs.Add(eco.populations[popName].weightSDs[i]);
            }

            // get copy of founder
            copy.populations[popName].founder = getCreatureCopy(eco.populations[popName].founder);

            // copy each creature over
            copy.populations[popName].creatures = new List<Creature>();
            foreach (Creature creat in eco.populations[popName].creatures)
            {
                //Debug.Log("orig count: " + creat.actionQueue.Count);
                Creature creatCopy = getCreatureCopy(creat);
                // set map on creatCopy to new map
                creatCopy.map = copy.map;

                creatCopy.parentPopulation = copy.populations[popName];
                // place creature on new map
                copy.map[creatCopy.position[0]][creatCopy.position[1]].creatureOn = creatCopy;
                // set new neighborlands list to reference map lands
                creatCopy.updateNeighbors();
                //Debug.Log("copy count: " + creatCopy.actionQueue.Count);
                copy.populations[popName].creatures.Add(creatCopy);
            }
            // offspring should have been saved to creatures by the end of the turn anyway
            copy.populations[popName].offspring = new List<Creature>();
        }

        // copy species dictionary
        copy.species = new Dictionary<string, Creature>();
        foreach (string speciesName in eco.species.Keys)
        {
            copy.species[speciesName] = getCreatureCopy(eco.species[speciesName]);
            copy.species[speciesName].map = copy.map; // not sure if this line is necessary (sets map for founder)
        }

        // copy resource options
        copy.resourceOptions = new Dictionary<string, ResourceStore>();
        foreach (string resName in eco.resourceOptions.Keys)
        {
            copy.resourceOptions[resName] = eco.resourceOptions[resName].shallowCopy();
        }

        return copy;
    }

    public static float normRand(float sd)
    {
        double u1;
        double u2;
        lock (randLock)
        {
            u1 = 1.0 - rand.NextDouble();
            u2 = 1.0 - rand.NextDouble();
        }
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        //Debug.Log("generated normal rv: " + randStdNormal);
        return (float) (sd * randStdNormal);

    }

    // copy a land but remove the creature from the land
    public static Land GetLandCopy(Land land)
    {
        Land landCopy = land.shallowCopy();
        landCopy.creatureOn = null; // remove creature (will be replaced)
        landCopy.propertyDict = new Dictionary<string, ResourceStore>();
        // copy ResourceStores into new propertyDict by key
        foreach (string resName in land.propertyDict.Keys)
        {
            landCopy.propertyDict[resName] = land.propertyDict[resName].shallowCopy();
        }
        return landCopy;
    }


    // get child of creature (don't maintain all of state)
    public static Creature getCreatureChild(Creature c)
    {
        Creature creatureCopy = c.getShallowCopy();
        int actualSeed;
        creatureCopy.childIndex = 0;
        int timeInMillis = System.DateTime.Now.Millisecond;
        lock (seedGenLock)
        {
            // reset seed if too large
            if (seed == Int32.MaxValue - 1000)
            {
                seed = 0;
            }
            // increment seed
            seed++;
            // generate random seed
            actualSeed = seedGen.Next(seed);
        }
        // Debug.Log("new rand gen seed: " + actualSeed);
        // use combination of random seed and time in milliseconds to create random number generator for new creature
        creatureCopy.rand = new System.Random(actualSeed + timeInMillis);
        creatureCopy.rand2 = new System.Random(actualSeed + timeInMillis + 999);

        // copy dummy land
        creatureCopy.dummyLand = new Land();
        creatureCopy.dummyLand.isDummy = true;

        // copy creature networks
        List<Dictionary<string, Network>> networks = new List<Dictionary<string, Network>>();
        creatureCopy.networks = networks;
        List<Dictionary<string, Network>> origNetworks = c.networks;
        copyCreatureNetworks(origNetworks, networks, creatureCopy);

        //Debug.Log("before child copy: " + c.phenotypeNetTemplate.net[0].Count);
        creatureCopy.phenotypeNetTemplate = (PhenotypeNetwork) copyNetwork(c.phenotypeNetTemplate, creatureCopy);
        creatureCopy.phenotypeNetTemplate.parentCreature = creatureCopy;
        //Debug.Log("after child copy: " + creatureCopy.phenotypeNetTemplate.net[0].Count);

        // position will be set by reproduction action
        creatureCopy.position = new int[2];

        creatureCopy.neighborLands = new Land[5]; // assigne when creature is placed on the map

        // don't copy parent's actions
        creatureCopy.actionQueue = new SimplePriorityQueue<Action>();

        // don't copy output comm signals to child
        creatureCopy.outputCommSignals = new List<CommSignal>();

        // don't copy prevNetStates to child
        creatureCopy.prevNetStates = new List<List<Dictionary<string, Network>>>();

        // copy abilities
        creatureCopy.abilities = new Dictionary<string, Ability>();
        foreach (string ability in c.abilities.Keys)
        {
            Ability oldAbility = c.abilities[ability];
            Ability newAbility = getNewAbility(oldAbility);
            creatureCopy.abilities[ability] = newAbility;
        }

        // copy stored resources: child will have similar resource levels to parent
        creatureCopy.storedResources = new Dictionary<string, CreatureResource>();
        foreach (string resKey in c.storedResources.Keys)
        {
            creatureCopy.storedResources[resKey] = c.storedResources[resKey].getShallowCopy();
        }

        // copy phenotype
        creatureCopy.phenotype = new bool[c.phenotype.Length];
        Array.Copy(c.phenotype, creatureCopy.phenotype, c.phenotype.Length);

        // don't copy inputCommList
        creatureCopy.inputCommList = new List<CommSignal>();

        // copy commInNetTemplate
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
        // copy commOutNetTemplate
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

        // don't copy reproductionRequests to child
        creatureCopy.reproductionRequests = new List<ReproAction>();

        // TODO: copy reproduction decider network


        // copy action pool
        creatureCopy.actionPool = new Dictionary<string, Action>();
        foreach (string key in c.actionPool.Keys)
        {
            creatureCopy.actionPool[key] = getNewAction(c.actionPool[key]);
        }

        return creatureCopy;
    }



    // get exact clone of creature (maintains state)
    public static Creature getCreatureCopy(Creature c)
    {
        Creature creatureCopy = c.getShallowCopy();
        int actualSeed;
        int timeInMillis = System.DateTime.Now.Millisecond;
        lock (seedGenLock)
        {
            // reset seed if too large
            if (seed == Int32.MaxValue - 1000)
            {
                seed = 0;
            }
            // increment seed
            seed++;
            // generate random seed
            actualSeed = seedGen.Next(seed);
        }
        // Debug.Log("new rand gen seed: " + actualSeed);
        // use combination of random seed and time in milliseconds to create random number generator for new creature
        creatureCopy.rand = new System.Random(actualSeed + timeInMillis);
        creatureCopy.rand2 = new System.Random(actualSeed + timeInMillis + 999);

        // copy dummy land
        creatureCopy.dummyLand = new Land();
        creatureCopy.dummyLand.isDummy = true;

        // copy creature networks
        List<Dictionary<string, Network>> networks = new List<Dictionary<string, Network>>();
        creatureCopy.networks = networks;
        List<Dictionary<string, Network>> origNetworks = c.networks;
        copyCreatureNetworks(origNetworks, networks, creatureCopy);

        // copy phenotype network template
        //Debug.Log("before copy: " + c.phenotypeNetTemplate.net[0].Count);
        creatureCopy.phenotypeNetTemplate = (PhenotypeNetwork)copyNetwork(c.phenotypeNetTemplate, creatureCopy);
        creatureCopy.phenotypeNetTemplate.parentCreature = creatureCopy;
        //Debug.Log("after copy: " + creatureCopy.phenotypeNetTemplate.net[0].Count);

        // copy position (overridden when populating)
        creatureCopy.position = new int[2];
        creatureCopy.position[0] = c.position[0];
        creatureCopy.position[1] = c.position[1];

        creatureCopy.neighborLands = new Land[5]; // assigne when creature is placed on the map

        // copy actions
        // TODO: test to make sure that order is maintained
        creatureCopy.actionQueue = new SimplePriorityQueue<Action>();
        while (c.actionQueue.Count > 0)
        {
            Action a = c.actionQueue.Dequeue();
            creatureCopy.actionQueue.Enqueue(getNewAction(a), a.priority);
        }

        // copy output comm signals
        creatureCopy.outputCommSignals = new List<CommSignal>();
        copyCommList(c.outputCommSignals, creatureCopy.outputCommSignals);

        // copy prevNetStates
        creatureCopy.prevNetStates = new List<List<Dictionary<string, Network>>>();
        foreach (List <Dictionary<string, Network>> origState in c.prevNetStates)
        {
            List<Dictionary<string, Network>> stateCopy = new List<Dictionary<string, Network>>();
            copyCreatureNetworks(origState, stateCopy, creatureCopy);
            creatureCopy.prevNetStates.Add(stateCopy);
        }

        // copy abilities
        creatureCopy.abilities = new Dictionary<string, Ability>();
        foreach (string ability in c.abilities.Keys)
        {
            Ability oldAbility = c.abilities[ability];
            Ability newAbility = getNewAbility(oldAbility);
            creatureCopy.abilities[ability] = newAbility;
        }

        // copy stored resources
        creatureCopy.storedResources = new Dictionary<string, CreatureResource>();
        foreach (string resKey in c.storedResources.Keys)
        {
            creatureCopy.storedResources[resKey] = c.storedResources[resKey].getShallowCopy();
        }

        // copy phenotype
        creatureCopy.phenotype = new bool[c.phenotype.Length];
        Array.Copy(c.phenotype, creatureCopy.phenotype, c.phenotype.Length);

        // copy inputCommList
        creatureCopy.inputCommList = new List<CommSignal>();
        copyCommList(c.inputCommList, creatureCopy.inputCommList);

        // copy commInNetTemplate
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
        // copy commOutNetTemplate
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

        // copy reproductionRequests
        creatureCopy.reproductionRequests = new List<ReproAction>();
        foreach (ReproAction reproAction in c.reproductionRequests)
        {
            ReproAction reproActionCopy = reproAction.shallowCopy();
            creatureCopy.reproductionRequests.Add(reproActionCopy);

        }

        // TODO: copy reproduction decider network


        // copy action pool
        creatureCopy.actionPool = new Dictionary<string, Action>();
        foreach (string key in c.actionPool.Keys)
        {
            creatureCopy.actionPool[key] = getNewAction(c.actionPool[key]);
        }

        return creatureCopy;
    }


    public static void copyCommList(List<CommSignal> origList, List<CommSignal> copyList)
    {

        foreach (CommSignal signal in origList)
        {
            CommSignal signalCopy = new CommSignal();
            foreach (string signalName in signal.commProperties.Keys)
            {
                bool[] commBits = new bool[signal.commProperties[signalName].Length];
                for (int l = 0; l < signal.commProperties[signalName].Length; l++)
                {
                    commBits[l] = signal.commProperties[signalName][l];
                }
                signalCopy.commProperties[signalName] = commBits;
            }
            copyList.Add(signalCopy);
        }
    }

    public static void copyCreatureNetworks(List<Dictionary<string, Network>> origNetworks, List<Dictionary<string, Network>> copyNetworks, Creature creatureCopy)
    {
        for (int i = 0; i < origNetworks.Count; i++)
        {
            Dictionary<string, Network> dict = new Dictionary<string, Network>();
            copyNetworks.Add(dict);
            foreach (string key in origNetworks[i].Keys)
            {
                dict[key] = copyNetwork(origNetworks[i][key], creatureCopy);

            }
        }
    }


    public static Network copyNetwork(Network oldNetwork, Creature creatureCopy)
    {
        Network newNetwork = oldNetwork.getShallowCopy();
        List<List<Node>> origNet = oldNetwork.net;
        List<List<Node>> newNet = new List<List<Node>>();
        newNetwork.net = newNet;
        for (int j = 0; j < origNet.Count; j++)
        {
            newNet.Add(new List<Node>());
            for (int k = 0; k < origNet[j].Count; k++)
            {
                newNet[j].Add(getNewNode(origNet[j][k] , creatureCopy, newNetwork));
            }
        }
        return newNetwork;
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
        Action newAction = oldAction.getShallowCopy(); // MemberwiseClone also includes subclass fields?
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
            SensoryInputNode newNode = (SensoryInputNode)oldNode.clone();
            newNode.creature = creatureCopy;
            return newNode;
            
        }
        else if (oldNode.GetType().Name == "InternalResourceInputNode")
        {
            InternalResourceInputNode newNode = (InternalResourceInputNode) oldNode.clone();
            newNode.creature = creatureCopy;
            return newNode;

        }
        else if (oldNode.GetType().Name == "OutputNode")
        {
            OutputNode oldNode2 = (OutputNode)oldNode;
            OutputNode newNode = (OutputNode) oldNode.clone();
            newNode.resetRand();
            newNode.parentNet = parentNet;
            newNode.parentCreature = creatureCopy;
            newNode.action = getNewAction(newNode.action);
            //newNode.setActivBehavior(new LogisticActivBehavior());
            newNode.prevNodes = new List<Node>();
            newNode.assignPrevNodes();
            newNode.weights = new List<float>();
            for (int i = 0; i < oldNode2.weights.Count; i++)
            {
                newNode.weights.Add(oldNode2.weights[i]);
            }
            newNode.extraWeights = new List<float>();
            for (int i = 0; i < oldNode2.extraWeights.Count; i++)
            {
                newNode.extraWeights.Add(oldNode2.extraWeights[i]);
            }
            return newNode;
        }
        else if (oldNode.GetType().Name == "NonInputNode")
        {
            NonInputNode oldNode2 = (NonInputNode)oldNode;
            NonInputNode newNode = (NonInputNode)oldNode.clone();
            newNode.resetRand();
            newNode.parentNet = parentNet;
            newNode.parentCreature = creatureCopy;
            //newNode.setActivBehavior(new LogisticActivBehavior());
            newNode.prevNodes = new List<Node>();
            newNode.assignPrevNodes();
            newNode.weights = new List<float>();
            for (int i = 0; i < oldNode2.weights.Count; i++)
            {
                newNode.weights.Add(oldNode2.weights[i]);
            }
            newNode.extraWeights = new List<float>();
            for (int i = 0; i < oldNode2.extraWeights.Count; i++)
            {
                newNode.extraWeights.Add(oldNode2.extraWeights[i]);
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
        // called when template is being copied
        else if (oldNode.GetType().Name == "PhenotypeInputNode")
        {
            PhenotypeInputNode oldNode2 = (PhenotypeInputNode)oldNode;
            PhenotypeInputNode newNode = (PhenotypeInputNode)oldNode.clone();
            // copy phenotype
            newNode.parentCreat = creatureCopy;
            int length = oldNode2.phenotype.Length;
            newNode.phenotype = new bool[length];
            for (int i = 0; i < newNode.phenotype.Length; i++)
            {
                newNode.phenotype[i] = oldNode2.phenotype[i];
            }
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
