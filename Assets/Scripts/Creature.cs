using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Priority_Queue;

using UnityEngine;


/// <summary>
/// Class for storing all data about a creature/agent, including its neural networks, queued actions, stored resources, location and other parameters.
/// </summary>
public class Creature
{
    public Creature founder;
    public Land dummyLand = new Land(); // dummyLand used for edges of map
    public int index;
    public System.Random rand = new System.Random();

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
    /// stores the state of the network for the past 3 time steps. The front of the queue is the most recent network state (t-1).
    /// </summary>
    public List<List<Network>> prevNetStates = new List<List<Network>>();
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
    /// A comm network will be created for each CommSignal in commList, and added to the first layer of networks in "networks" (the input layer).
    /// </summary>
    public CommNetwork commInNetTemplate = new CommNetwork();
    /// <summary>
    /// A template for the network that generates actions towards a specific neighbor in response to comm input from that neighbor.
    /// </summary>
    public CommNetwork commOutNetTemplate = new CommNetwork();
    /// <summary>
    /// Maximum health that creature can attain.
    /// </summary>
    public float maxHealth;

    public int remainingAbilityPoints;
    /// <summary>
    /// List of requests for reproduction from neighbors.
    /// </summary>
    public List<ReproAction> reproductionRequests;
    /// <summary>
    /// Network to decide whether a creature should reproduce with a neightbor.
    /// </summary>
    public ReproNetwork reproductionDeciderNetwork;
    /// <summary>
    /// Dictionary of potential actions that the creature can take if assigned to an output node.
    /// </summary>
    public Dictionary<string, Action> actionPool = new Dictionary<string, Action>();

    public Creature(int maxAbilityPoints)
    {
        remainingAbilityPoints = maxAbilityPoints;
    }

    public Creature()
    {
        // TODO: this is only test case, delete
        remainingAbilityPoints = 10;
        dummyLand.isDummy = true;
    }


    /// <summary>
    /// Starts creatures turn
    /// </summary>
    public void startTurn()
    {
        remainingTurnTime = fullTurnTime;
        updateNets(); // only need to do first turn, and after movement
        //printNetworks();
        addActionsToQueue();
        performActions();
        resourceHealthUpdate();
        //printNetworks();
    }

    public void addActionsToQueue()
    {
        int lastNetLayer = networks.Count - 1;
        foreach (Network network in networks[lastNetLayer].Values)
        {
            int finalLayer = network.net.Count - 1;
            foreach (OutputNode node in network.net[finalLayer])
            {
                double uniform = rand.NextDouble();
                //Debug.Log("probability: " + node.value);
                if (uniform < node.value)
                {
                    actionQueue.Enqueue(node.action, node.action.priority);
                }

            }
        }
    }


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
    }

    public void updateNets()
    {
        //Debug.Log("network count: " + networks.Count);
        // for every layer of networks
        for (int i = 0; i < networks.Count; i++)
        {
            // for every network in that layer
            foreach (Network net in networks[i].Values)
            {
                net.feedForward();
            }
        }
    }

    /*
    public Creature getCopy()
    {
        return CSDeepCloneObject.DeepCloneHelper.DeepClone(this);
    }
    */


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
    public void performActions()
    {
        while (actionQueue.Count > 0)
        {
            Action nextAction = actionQueue.Dequeue();
            if (nextAction.timeCost <= remainingTurnTime)
            {
                //Debug.Log("performing action");
                nextAction.perform(this);
            }
            else
            {
                actionQueue.Enqueue(nextAction, nextAction.priority);
                break;
            }
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



    public void addVariationToWeights
        (float standardDev)
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

    }

    public Creature getShallowCopy()
    {
        return (Creature)this.MemberwiseClone();
    }
}