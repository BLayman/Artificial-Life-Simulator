using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Priority_Queue;


[Serializable]
public class Creature
{

    public Creature founder;

    /// <summary>
    /// Stores all networks into layers of lists of Networks. 10 Maximum
    /// </summary>
    public List<Dictionary<string, Network>> networks = new List<Dictionary<string, Network>>();
    public string species = "default";
    private List<List<Land>> map = new List<List<Land>>();
    public int[] position = new int[2];
    /// <summary>
    /// Neighbors are up, down, left, and right. Index 0 for land creature is on.
    /// </summary>
    private Land[] neighborLands = new Land[5];
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

    public Creature(int maxAbilityPoints)
    {
        remainingAbilityPoints = maxAbilityPoints;
    }


    /// <summary>
    /// Starts creatures turn
    /// </summary>
    public void startTurn()
    {
        Console.WriteLine("starting turn");
        throw new System.NotImplementedException();
    }

    public Creature getCopy()
    {
        using (System.IO.MemoryStream stream = new MemoryStream())
        {
            if (this.GetType().IsSerializable)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Creature) formatter.Deserialize(stream);
            }
            return null;
        }
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
        throw new System.NotImplementedException();
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


}