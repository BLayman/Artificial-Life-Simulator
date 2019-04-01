// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Priority_Queue;

using UnityEngine;


/// <summary>
/// Class for storing all data about a creature/agent, including its neural networks, queued actions, stored resources, location and other parameters.
/// </summary>
public class Creature
{
    public Creature founder;
    public Land dummyLand = new Land(); // dummyLand used for edges of map
    public int index; // unique number assigned upon creation in species populator
    public System.Random rand; // used for action probabilites
    public System.Random rand2; // used for selecting actions from network
    int count;
    public Color color; // color displayed on map
    public bool senseNeighborPhenotypes = false;
    public float annealMutationFraction = 1;
    public float baseMutationDeviation = 0;

    /// <summary>
    /// Stores all networks into layers of lists of Networks. 10 Maximum
    /// </summary>
    public List<Dictionary<string, Network>> networks = new List<Dictionary<string, Network>>();
    public string species = "default";
    public List<List<Land>> map; // x coord by y coord
    public int[] position = new int[2]; // x y
    /// <summary>
    /// Neighbors are up, down, left, and right. Index 0 for land creature is on.
    /// </summary>
    public Land[] neighborLands = new Land[5];
    /// <summary>
    /// Actions to be taken by creature
    /// </summary>
    public SimplePriorityQueue<Action> actionQueue = new SimplePriorityQueue<Action>();
    /// <summary>
    /// Time remaining in turn: limits number of actions that can be taken in one turn.
    /// </summary>
    public float remainingTurnTime;

    public float mutationStandardDeviation;

    public float fullTurnTime;
    /// <summary>
    /// The number of layers of Networks
    /// </summary>
    public int numLayersOfNets;
    /// <summary>
    /// stores an array of booleans for each neighbor for communication. e.g. 8 neighbors by 3 bools each.
    /// </summary>
    public List<CommSignal> outputCommSignals = new List<CommSignal>();
    /// <summary>
    /// Not implememted yet. Stores the state of the networks for previous time steps. The front of the queue is the most recent network state (t-1).
    /// </summary>
    public List<List<Dictionary<string, Network>>> prevNetStates = new List<List<Dictionary<string, Network>>>();
    /// <summary>
    /// Designates which resources or species the creature has an advantage in consuming, attacking, or defending against. Certain combinations of excess resources can boost abilities.
    /// </summary>
    public Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();
    /// <summary>
    /// When health reaches zero, creature dies.
    /// </summary>
    public float health;
    public Dictionary<string, CreatureResource> storedResources = new Dictionary<string, CreatureResource>();
    /// <summary>
    /// Outward appearance of creature: for communication purposes. Typically 4 bits.
    /// </summary>
    public bool[] phenotype;
    /// <summary>
    /// A list of incoming comm signals.
    /// </summary>
    public List<CommSignal> inputCommList = new List<CommSignal>();
    /// <summary>
    /// A comm network will be created for each CommSignal in commList, and added to the first layer of networks in "networks" .
    /// </summary>
    public CommNetwork commInNetTemplate = new CommNetwork();
    /// <summary>
    /// A template for the network that generates actions towards a specific neighbor in response to comm input from that neighbor.
    /// </summary>
    public CommNetwork commOutNetTemplate = new CommNetwork();
    /// <summary>
    /// Maximum health that creature can attain.
    /// </summary>

    /// <summary>
    /// A list of neighbor phenotypes
    /// </summary>
    // public List<bool[]> inputPhenotypeList = new List<bool[]>();


    /// <summary>
    /// A network will be created for each neighbor to process its phenotype, and added to the first layer of networks in "networks" 
    public PhenotypeNetwork phenotypeNetTemplate = new PhenotypeNetwork();

    public float maxHealth;

    public int remainingAbilityPoints;
    /// <summary>
    /// List of requests for reproduction from neighbors.
    /// </summary>
    public List<ReproAction> reproductionRequests = new List<ReproAction>();
    /// <summary>
    /// Network to decide whether a creature should reproduce with a neightbor.
    /// </summary>
    public ReproNetwork reproductionDeciderNetwork = new ReproNetwork();
    /// <summary>
    /// Dictionary of potential actions that the creature can take if assigned to an output node.
    /// </summary>
    public Dictionary<string, Action> actionPool = new Dictionary<string, Action>();

    public int actionClearInterval = 5;

    public int actionClearCount = 0;

    public int actionClearSize = 10;

    public Creature(int maxAbilityPoints)
    {
        remainingAbilityPoints = maxAbilityPoints;

        dummyLand.isDummy = true;
        rand = new System.Random();
        rand2 = new System.Random();

        count = 0;
        // Debug.Log("*********************        creature constructor called             ********************");

    }

    // Not called during MemberwiseClone?
    // TEST ONLY CONSTRUCTOR?
    public Creature()
    {
        // TODO: this is only test case, delete
        remainingAbilityPoints = 10;
        dummyLand.isDummy = true;

        rand = new System.Random();
        rand2 = new System.Random();

    }


    /// <summary>
    /// Starts creatures turn
    /// </summary>
    public void startTurn(Ecosystem eco)
    {
        // reset turn time
        remainingTurnTime = fullTurnTime;
        // run inputs through neural networks
        updateNets(); 
        // add actions to action queue
        addActionsToQueue();
        // perform actions
        performActions(eco);
        // update health based on resource levels
        resourceHealthUpdate();

    }

    // add actions from last layer of neural network to the queue
    public void addActionsToQueue()
    {
        int lastNetLayer = networks.Count - 1;

        //Debug.Log("");
        // for each network in last layer of networks
        foreach (Network network in networks[lastNetLayer].Values)
        {
            int finalLayer = network.net.Count - 1;
            // for each node in final layer of network
            List<Action> tempList = new List<Action>();

            foreach (OutputNode node in network.net[finalLayer])
            {
                // user random number generator to decide action based on probability
                count++;
                // if random number generator generates 1 billion numbers, reset it
                // TODO: make work indefinitely
                if(count > 1000000000)
                {
                    //Debug.Log("creating new random number generator");
                    // will also need to reset seed generator
                    rand = new System.Random();
                    count = 0;
                }

                double uniform = rand.NextDouble();

                //Debug.Log("Action: " + node.action.name + ", probability: " + node.value + " uniform: " + uniform);
                //Debug.Log("random number " + uniform);
                // treat node value as probability. If random number (0,1] is less than that probability, then enqueue action.
                if (uniform < node.value)
                {
                    tempList.Add(node.action);
                }
            }

            int nodesLeft = tempList.Count;
            while(nodesLeft > 0)
            {
                int randIndex = rand2.Next(nodesLeft);
                actionQueue.Enqueue(tempList[randIndex], tempList[randIndex].priority);
                nodesLeft--;
            }
        }
    }

    // TODO : also print hidden layers
    public void printNetworks()
    {
        // print creature's networks:
        Debug.Log("**************************   creature: " + index + "  *********************");
        foreach (Network network in networks[0].Values)
        {
            Debug.Log("net name: " + network.name);
            Debug.Log("layer 1:");
            foreach (Node node in network.net[0])
            {
                Debug.Log("input value: " + node.value);
            }
            Debug.Log("layer 2:");

            foreach (NonInputNode node in network.net[1])
            {
                node.printInputsAndWeights();
                //node.setActivBehavior(new LogisticActivBehavior());
                //node.updateValue();
                Debug.Log("this node value: " + node.value);
            }
        }
        foreach (Network network in networks[1].Values)
        {
            Debug.Log("net name: " + network.name);
            Debug.Log("layer 1:");
            foreach (Node node in network.net[0])
            {
                Debug.Log("input value: " + node.value);
            }
            Debug.Log("layer 2:");

            foreach (NonInputNode node in network.net[1])
            {
                node.printInputsAndWeights();
                Debug.Log("*************   this final output node value: " + node.value);
            }
        }

        Debug.Log("********     phenotype net template      ***********");
        Network net = phenotypeNetTemplate;
        Debug.Log("net name: " + net.name);
        Debug.Log("layer 1:");
        foreach (Node node in net.net[0])
        {
            Debug.Log("input value: " + node.value);
        }
        Debug.Log("layer 2:");

        foreach (NonInputNode node in net.net[1])
        {
            node.printInputsAndWeights();
            Debug.Log("*************   this final output node value: " + node.value);
        }
    }

    public void updateNets()
    {
        if (senseNeighborPhenotypes)
        {
            addPhenotypeNetworks();
        }

        // for every layer of networks
        for (int i = 0; i < networks.Count; i++)
        {
            // for every network in that layer
            foreach (Network net in networks[i].Values)
            {
                // pass inputs through neural network
                net.feedForward();
            }
        }

        if (senseNeighborPhenotypes)
        {
            removePhenotypeNetworks();
        }
    }

    // TODO : test
    public void addPhenotypeNetworks()
    {
        // add network for every neighbor with a creature
        for (int i = 0; i < neighborLands.Length; i++)
        {
            if (neighborLands[i].creatureIsOn())
            {
                // get a copy of the template
                // Debug.Log("template first layer length: " + phenotypeNetTemplate.net[0].Count);
                PhenotypeNetwork phenotypeNet = (PhenotypeNetwork) Copier.copyNetwork(phenotypeNetTemplate, this);
               
                // set the phenotype used in the template
                phenotypeNet.setInputNodes(neighborLands[i].creatureOn.phenotype);
                // add the network to the creatures networks
                string phenoNetName = "phenotypeNet" + i;
                networks[0].Add(phenoNetName, phenotypeNet);

                // for every output node in the phenotype network
                int length = phenotypeNet.net.Count;
                for (int j = 0; j < phenotypeNet.net[length - 1].Count; j++)
                {
                    OutputNode outNode = (OutputNode) phenotypeNet.net[length - 1][j];

                    // for every output network, add an inner-input node for the phenotype node, if applicable
                    foreach (OutputNetwork net in networks[networks.Count - 1].Values)
                    {
                        // output network must match action of phenotype output node
                        if (net.outputAction.name.Equals(outNode.action.name))
                        {
                            // create a new inner-input node
                            InnerInputNode node = new InnerInputNode();

                            // set linked node
                            node.parentCreature = this;
                            node.setLinkedNode(this, 0 , phenoNetName, phenotypeNet.net.Count - 1, j);
                            node.temp = true;

                            // add inner-input node to first layer of output network
                            net.net[0].Add(node);
                        }
                        
                    }
                }

                
            }
        }
    }



    // reset phenotype networks after each turn
    public void removePhenotypeNetworks()
    {
        List<string> toRemove = new List<string>();

        foreach (string netKey in networks[0].Keys)
        {
            if (netKey.StartsWith("phenotypeNet"))
            {
                toRemove.Add(netKey);
            }
        }
        for (int i = 0; i < toRemove.Count; i++)
        {
            networks[0].Remove(toRemove[i]);
        }

        // remove extra nodes as well

        // for every output network
        foreach (OutputNetwork net in networks[networks.Count - 1].Values)
        {
            List<Node> removeNodes = new List<Node>();
            // for every inner input node in first layer, delete if temp
            foreach (Node node in net.net[0])
            {
                if(node.GetType().Name == "InnerInputNode")
                {
                    InnerInputNode iiNode = (InnerInputNode) node;
                    if (iiNode.temp)
                    {
                        removeNodes.Add(node);
                    }
                }
                
            }
            foreach (Node node in removeNodes)
            {
                net.net[0].Remove(node);
            }
        }
        
    }



    /// <summary>
    /// Called at beginning of each turn.
    /// </summary>
    public void updateNeighbors()
    {

        //Debug.Log("in update neighbors: " + map.Count);
        neighborLands[0] = map[position[0]][position[1]];

        if (position[1] + 1 >= map[position[0]].Count)
        {
            neighborLands[1] = dummyLand;
        }
        else
        {
            neighborLands[1] = map[position[0]][position[1] + 1];
        }

        if (position[1] - 1 < 0)
        {
            neighborLands[2] = dummyLand;
        }
        else
        {
            neighborLands[2] = map[position[0]][position[1] - 1];
        }

        if (position[0] - 1 < 0)
        {
            neighborLands[3] = dummyLand;
        }
        else
        {
            neighborLands[3] = map[position[0] - 1][position[1]];
        }

        if (position[0] + 1 >= map.Count)
        {
            neighborLands[4] = dummyLand;
            //Debug.Log("set neighbor on right to dummy land " + dummyLand.isDummy);

        }
        else
        {
            neighborLands[4] = map[position[0] + 1][position[1]];
        }

        
    }

    /// <summary>
    /// Stores the state of networks in prevNetStates
    /// </summary>
    public void storeCurrentNetState()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Performs whatever actions in the queue it can.
    /// </summary>
    public void performActions(Ecosystem eco)
    {
        /** 
         * TODO: allow for a certain number of actions to carry over,
         * otherwise, only the first action put on the queue will be run each turn,
         * which is biased: based on the order of the output nodes in the final layer
         * one solution: sort the order in which output nodes are added to queue
        **/ 

        

        // remove all actions from queue each turn
        SimplePriorityQueue<Action> nextQueue = new SimplePriorityQueue<Action>();

        while (actionQueue.Count > 0)
        {
            Action nextAction = actionQueue.Dequeue();

            // ignore actions that the creature doesn't have enough resources to perform
            if (nextAction.enoughResources(this))
            {
                // if there is time left for an action, perform it
                if (nextAction.timeCost <= remainingTurnTime)
                {
                    //Debug.Log("performing " + nextAction.name);
                    nextAction.performWrapper(this, eco);
                }
                else
                {
                    // put actions that take too long on the next turns queue
                    nextQueue.Enqueue(nextAction, nextAction.priority);
                }
            }
        }
        // actionQueue is now the queue for next turn
        actionQueue = nextQueue;
        // Debug.Log("Queue size: " + actionQueue.Count);
        // keep action queue a manageable size by clearing it every few steps, and clearing it if its size gets too big
        if (actionClearCount > actionClearInterval || actionQueue.Count >= actionClearSize)
        {
            actionClearCount = 0;
            actionQueue.Clear();
        }
        
    }

    /// <summary>
    /// Updates health based on resource levels.
    /// </summary>
    public void resourceHealthUpdate()
    {
        foreach (CreatureResource resource in storedResources.Values)
        {
            resource.healthUpdate(this);
        }
        //Debug.Log("Creature health: " + health);
    }

    /// <summary>
    /// Returns true if health is 0 or below, also deletes creature from map if dead
    /// </summary>
    public bool isDead()
    {

        if (health <= 0)
        {
            map[position[0]][position[1]].creatureOn = null;
            return true;
        }
        else
        {
            return false;
        }
        
    }

    /// <summary>
    /// Creates and adds comm networks to the first layer of "networks" using commSignals, and commNetTemplate
    /// </summary>
    public void addCommNetworks()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Adds network in given layer, and returns index of added layer.
    /// </summary>
    /// <param name="layerOfNets">0 to add to input nets, 1 to add to output nets.</param>
    public int addNetwork(int layerOfNets)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Iterates over neightbors and passes comm signals to them.
    /// </summary>
    public void sendCommOutputSignals()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// process requests for reproduction.
    /// </summary>
    public void processReproRequests()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Reproduce with neighboring creature.
    /// </summary>
    /// <param name="mate">Creature to reproduce with.</param>
    public void reproduce(Creature mate)
    {
        throw new System.NotImplementedException();
    }



    public void addVariationToWeights (float standardDev)
    {
        foreach (Dictionary<string, Network> dict in networks)
        {
            foreach (Network net in dict.Values)
            {
                foreach (List<Node> layer in net.net)
                {
                    foreach (NonInputNode node in layer.OfType<NonInputNode>())
                    {
                        for (int i = 0; i < node.weights.Count; i++)
                        {
                            //Debug.Log("old weight: " + node.weights[i]);
                            node.weights[i] += Copier.normRand(standardDev);
                            //Debug.Log("new weight: " + node.weights[i]);
                        }
                    }
                }
            }
        }
        // update weights of phenotype network
        foreach (List<Node> layer in phenotypeNetTemplate.net)
        {
            foreach (NonInputNode node in layer.OfType<NonInputNode>())
            {
                for (int i = 0; i < node.weights.Count; i++)
                {
                    //Debug.Log("old weight: " + node.weights[i]);
                    node.weights[i] += Copier.normRand(standardDev);
                    //Debug.Log("new weight: " + node.weights[i]);
                }
            }
        }

    }

    public Creature getShallowCopy()
    {
        return (Creature)this.MemberwiseClone();
    }
}