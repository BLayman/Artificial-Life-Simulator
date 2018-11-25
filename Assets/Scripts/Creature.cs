using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Priority_Queue;

using UnityEngine;


public class Creature
{

    public Creature founder;
    public Land dummyLand = new Land();
    public int index;

    /// <summary>
    /// Stores all networks into layers of lists of Networks. 10 Maximum
    /// </summary>
    public List<Dictionary<string, Network>> networks = new List<Dictionary<string, Network>>();
    public string species = "default";
    public List<List<Land>> map = new List<List<Land>>(); // x coord by y coord
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
    public int remainingTurnTime;

    public int fullTurnTime;
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
    public int health;
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
    public CommNetwork commInNetTemplate;
    /// <summary>
    /// A template for the network that generates actions towards a specific neighbor in response to comm input from that neighbor.
    /// </summary>
    public CommNetwork commOutNetTemplate;
    /// <summary>
    /// Maximum health that creature can attain.
    /// </summary>
    public int maxHealth;

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
        addActionsToQueue();
        updateNeighbors();
        updateNets();
        //addActionsToQueue();
    }

    public void addActionsToQueue()
    {
        // for every network in that layer
        Debug.Log("creature: " + index);
        foreach (Network network in networks[0].Values)
        {
            Debug.Log("net name: " + network.name);
            foreach(Node node in network.net[0])
            {
                Debug.Log(node.value);
            }
            Debug.Log("layer 2");

            foreach (NonInputNode node in network.net[1])
            {
                node.printInputsAndWeights();
                Debug.Log("value:" + node.value);
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


    public Creature getCopy()
    {
        return CSDeepCloneObject.DeepCloneHelper.DeepClone(this);
    }

    /// <summary>
    /// updates creatures current position based on a move action, also updates creatureOn and creatureIsOn for relevant Land objects
    /// </summary>
    private void movePos()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Called at beginning of each turn.
    /// </summary>
    public void updateNeighbors()
    {
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

        if (position[0] + 1 >= map.Count)
        {
            neighborLands[4] = dummyLand;
        }
        else
        {
            neighborLands[4] = map[position[0] + 1][position[1]];
        }

        if (position[0] - 1 < 0)
        {
            neighborLands[3] = dummyLand;
        }
        else
        {
            neighborLands[3] = map[position[0] - 1][position[1]];
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
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Updates health based on resource levels.
    /// </summary>
    public void resourceHealthUpdate()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Returns true if health is 0 or below.
    /// </summary>
    public bool checkIfDead()
    {
        throw new System.NotImplementedException();
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

    /// <summary>
    /// Adds default actions to action pool: movement, reproduction, resource consumption.
    /// </summary>
    public void generateDefaultActions()
    {
        // create movement actions
        actionPool.Add("Move up", new MoveAction(1));
        actionPool.Add("Move down", new MoveAction(2));
        actionPool.Add("Move left", new MoveAction(3));
        actionPool.Add("Move right", new MoveAction(4));

        // create consumption actions
        
        // for each resource that creature can store
        foreach (string resource in storedResources.Keys)
        {
            // for each neighbor spot reachable by creature
            for (int i = 0; i < 5; i++)
            {
                actionPool.Add(resource + "At" + i, new ConsumeFromLand(i, resource, this));
            }
        }

        // add Reproduction action?
        // actionPool.Add("reproduce", new ReproAction());
    }

    public void addVariationToWeights(float standardDev)
    {
        foreach (Dictionary<string,Network> dict in networks)
        {
            foreach (Network net in dict.Values)
            {
                foreach (List<Node> layer in net.net)
                {
                    foreach(NonInputNode node in layer.OfType<NonInputNode>())
                    {
                        for (int i = 0; i < node.weights.Count; i++)
                        {
                            node.weights[i] += Utility.normRand(standardDev);
                        }
                    }
                }
            }
        }
    }

    public Creature getShallowCopy()
    {
        return (Creature) this.MemberwiseClone();
    }
}