using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



class EcoCreationHelper
{

    /// <summary>
    /// Create a node to sense the level of a particular resource from a particular neighboring square.
    /// </summary>
    /// <param name="netCreator">Network wrapper.</param>
    /// <param name="landIndex">Index of neighboring land square to sense: 0: center, 1: up, 2: down, 3: left, 4: right.</param>
    /// <param name="sensedResource">Resource to be sensed.</param>
    public static void makeSensoryInputNode(NetworkEditor netCreator, int landIndex, string sensedResource)
    {
        NodeEditor nodeCreator = netCreator.addNode(0);
        // user sets node type to sensory input node
        nodeCreator.setCreator(NodeCreatorType.siNodeCreator);

        // the sensory node editor gets it's sensory input node creator from nodeCreator
        SensoryInputNodeEditor sinc2 = (SensoryInputNodeEditor)nodeCreator.getNodeCreator();
        // the sinc is used to set properties on the sensory input node
        sinc2.setLandIndex(landIndex);
        sinc2.setSensedResource(sensedResource);

        // user clicks save on node editor
        netCreator.saveNode();
    }

    /// <summary>
    /// Create a node that senses the level of a resource stored in the Creature.
    /// </summary>
    /// <param name="netCreator">Network wrapper.</param>
    /// <param name="sensedResource">Resource to be sensed.</param>
    public static void makeInternalResourceInputNode(NetworkEditor netCreator, string sensedResource)
    {
        NodeEditor nodeCreator = netCreator.addNode(0); // add to first layer
        // user sets node type to sensory input node
        nodeCreator.setCreator(NodeCreatorType.internalResNodeEditor);
        InternalResInputNodeEditor irnc = (InternalResInputNodeEditor)nodeCreator.getNodeCreator();
        irnc.setSensedResource(sensedResource);
        // user clicks save on node editor
        netCreator.saveNode();
    }


    /// <summary>
    /// Create a node that connects the networks to actions. These nodes belong in the last layer of the last networks.
    /// </summary>
    /// <param name="netCreator">Network wrapper.</param>
    /// <param name="activationType">Activation type: should have an output in the range: [0,1] to be associated with a probability.</param>
    /// <param name="action">Action that the node is associated with.</param>
    /// <param name="layer">Layer in which to place node (final layer).</param>
    public static void makeOutputNode(NetworkEditor netCreator, ActivationBehaviorTypes activationType, string action, int layer)
    {
        // user adds node to second layer
        NodeEditor nodeCreator = netCreator.addNode(layer);
        nodeCreator.setCreator(NodeCreatorType.outputNodeCreator);
        OutputNodeEditor onc = (OutputNodeEditor)nodeCreator.getNodeCreator();
        if (!netCreator.parentCreatureCreator.creature.actionPool.ContainsKey(action))
        {
            Debug.LogError("invalid action key for output node");
        }
        else
        {
            Action a = netCreator.parentCreatureCreator.creature.actionPool[action];
            onc.setAction(a);
            onc.setActivationFunction(activationType);
            netCreator.saveNode();
        }

        // user clicks save on network creator
    }

    /// <summary>
    /// Set parameters for attack action
    /// </summary>
    /// <param name="attackEdit">Attack action wrapper.</param>
    /// <param name="target">Species to be attacked.</param>
    /// <param name="backfireHealthLost">The amount of health lost if the attack backfires.</param>
    /// <param name="resourceFractTaken">The fraction of the resources stolen from the target creature by the attack.</param>
    /// 
    public static void setAttackActionParams(AttackEditor attackEdit, string target, float backfireHealthLost, float resourceFractTaken)
    {
        attackEdit.setBackfireHealthLost(backfireHealthLost);
        attackEdit.setResourceFracTaken(resourceFractTaken);
        attackEdit.setVictimSpecies(target);
    }

    /// <summary>
    /// Make a first layer node that gets its value from a node in the last layer of another network.
    /// </summary>
    /// <param name="netCreator">Network wrapper.</param>
    /// <param name="layer">Layer in which node is to be added.</param>
    /// <param name="linkedNetName">Name of the linked network.</param>
    /// <param name="linkedNetIndex">Layer of linked network in layers of networks.</param>
    /// <param name="linkedNodeIndex">The index of the linked node in that final layer of the linked network.</param>
    public static void makeInnerInputNode(NetworkEditor netCreator, int layer, string linkedNetName, int linkedNetIndex, int linkedNodeIndex)
    {
        // user adds nodes to input layer (0)
        NodeEditor nodeCreator = netCreator.addNode(layer);
        // user adds inner input node
        nodeCreator.setCreator(NodeCreatorType.innerInputNodeCreator);
        InnerInputNodeEditor iinc = (InnerInputNodeEditor)nodeCreator.getNodeCreator();
        // the inner input node gets its value from net1's output node at index 0
        iinc.setLinkedNode(linkedNetName, linkedNodeIndex, linkedNetIndex);
        // user clicks save on node editor
        netCreator.saveNode();
    }

    /// <summary>
    /// Add a hidden node to a particular network.
    /// </summary>
    /// <param name="netCreator">Wrapper for Network.</param>
    /// <param name="activationType">Activation type for hidden node.</param>
    /// <param name="layer">Layer in which hidden node is to be added.</param>
    public static void makeHiddenNode(NetworkEditor netCreator, ActivationBehaviorTypes activationType, int layer)
    {
        // user adds node to second layer
        NodeEditor nodeCreator = netCreator.addNode(layer);
        nodeCreator.setCreator(NodeCreatorType.hiddenNode);
        HiddenNodeEditor hne = (HiddenNodeEditor)nodeCreator.getNodeCreator();
        hne.setActivationFunction(activationType);
        netCreator.saveNode();
        // user clicks save on network creator
    }


    /// <summary>
    /// Add resource to be stored on Land spaces.
    /// </summary>
    /// <param name="ecoEditor">Wrapper for ecosystem.</param>
    /// <param name="name">Name of resource. Use this same name when creating creature resources.</param>
    /// <param name="initialAmt">Initial amount stored on a Land. Note: this parameter may be ignored by MapEditor when distributing the resource.</param>
    /// <param name="maxAmt">Max amount stored on a Land.</param>
    /// <param name="consumedPerTime">Amount of the resource consumed per unit of time.</param>
    /// <param name="proportionExtract">Proportion of the amount consumed that is actually extracted. This proportion is modified by the creature's ability to extract this resource.</param>
    /// <param name="renewAmt">The amount of the resource renewed every renew interval (see setEcoParams).</param>
    public static void addResource(EcosystemEditor ecoEditor, string name, float initialAmt, float maxAmt,
                                    float consumedPerTime, float proportionExtract, float renewAmt)
    {
        LandResourceEditor lre = ecoEditor.addResource(name);
        lre.setAmountOfResource(initialAmt);
        lre.setMaxAmt(maxAmt);
        lre.setAmtConsumedPerTime(consumedPerTime);
        lre.setProportionExtracted(proportionExtract); // higher proportion extracted for primary resources
        lre.setRenewalAmt(renewAmt);
    }

    /// <summary>
    /// Set basic parameters for entire ecosystem.
    /// </summary>
    /// <param name="ecoEditor">Ecosystem wrapper.</param>
    /// <param name="abilityPtsPerCreat">The maximum number of ability points that each creature is alloted (gives creatures the capacity to perform certain actions better).</param>
    /// <param name="commBits">The number of communication bits per creature (not currently in use).</param>
    /// <param name="renewInterval">The number of steps between each renewal of all resources on all Lands.</param>
    /// <param name="uniformRenewal">Renew resources across entire map.</param>
    /// <param name="clusterRenewal">Only renew resources in the clusters in which they were created (if using clusters to distribute).</param>
    public static void setEcoParams(EcosystemEditor ecoEditor, int abilityPtsPerCreat, int commBits,
                                        int renewInterval, bool uniformRenewal, bool clusterRenewal)
    {
        ecoEditor.setAbilityPointsPerCreature(10);
        ecoEditor.setCommBits(4);
        ecoEditor.setDistinctPhenotypeNum(4);
        ecoEditor.setRenewInterval(50);
        ecoEditor.setUniformRenewal(uniformRenewal);
        ecoEditor.setClusterRenewal(clusterRenewal);
    }


    /// <summary>
    /// Set basic parameters for a creature type.
    /// </summary>
    /// <param name="ce">Creature wrapper.</param>
    /// <param name="name">Name of creature.</param>
    /// <param name="phenotype">Phenotype of creature (potentially sensed by other creatures).</param>
    /// <param name="turnTime">The amount of time given for a creature's turn. It's recommended to use the same turn time for all creatures.</param>
    /// <param name="maxHealth">The maximum possible health a creature can achieve.</param>
    /// <param name="initialHealth">The initial health of the creature when a population is created. Note that a child inherits the health of it's parent, and doesn't use this value.</param>
    /// <param name="actionClearInterval">The action queue is cleared at intervals of this number of steps.</param>
    /// <param name="actionClearSize">The size that the action queue must reach before it is cleared.</param>
    /// <param name="mutationDeviation">The initial amount of mutation that occurs with reproduction.</param>
    /// <param name="color">The color of the creature.</param>
    /// <param name="usePhenoNet">Chose wether creature should sense the phenotypes of it's neighbors. If so, you must create a phenotype network template.</param>
    /// <param name="mutationDeviationFraction">The number that the mutation deviation is multiplied by each time a creature reproduces. This new deviation is passed on to the child. This can be used to slowy reduce the amount of variability in the population.</param>
    /// <param name="lowestMutationDeviation">The lowest value that the mutation deviation can reach, even with the mutationDeviationFraction.</param>
    /// <param name="mutationType">The type of mutation being used.</param>
    public static void setCreatureStats(CreatureEditor ce, string name, int phenotype, float turnTime, float maxHealth, float initialHealth,
                                     int actionClearInterval, int actionClearSize, float mutationDeviation, ColorChoice color, bool usePhenoNet,
                                     float mutationDeviationFraction, float lowestMutationDeviation, MutationDeviationCoefficientType mutationType)
    {
        ce.setSpecies(name);
        ce.setPhenotype(phenotype);
        ce.setTurnTime(turnTime);
        ce.setMaxHealth(maxHealth);
        ce.setInitialHealth(initialHealth);
        ce.setActionClearInterval(actionClearInterval);
        ce.setActionClearSize(actionClearSize);
        ce.setMutationStandardDeviation(mutationDeviation);
        ce.setColor(color);
        ce.setUsePhenotypeNet(usePhenoNet);
        ce.setAnnealMutationFraction(mutationDeviationFraction);
        ce.setBaseMutationDeviation(lowestMutationDeviation);
        ce.setMutationCoeffType(mutationType);
    }

    /// <summary>
    /// Give creature the capacity to store a particular resource.
    /// </summary>
    /// <param name="resourceEditor">Wrapper for helping to create a resource.</param>
    /// <param name="name">Name of resource. Must match name passed into EcoCreator.addResource().</param>
    /// <param name="maxLevel">Maximum amount that can be stored.</param>
    /// <param name="initialLevel">Initial amount stored.</param>
    /// <param name="healthGain">The amount of health gained when the resource amount surpases gainThreshold.</param>
    /// <param name="gainThreshold">The threshold above which healthGain is added to the creature's health each turn.</param>
    /// <param name="healthDrain">The amount of health that is lost each turn when the resource amount fall below drainThreshold.</param>
    /// <param name="drainThreshold">The threshold below which healthDrain is subtracted from the creature's health each turn.</param>
    /// <param name="baseUsage">The baseline amount of the resource that is used every turn.</param>
    public static void addCreatureResource(ResourceEditor resourceEditor, string name, float maxLevel, float initialLevel, float healthGain,
                                        float gainThreshold, float healthDrain, float drainThreshold, float baseUsage)
    {
        resourceEditor.setName(name);
        resourceEditor.setMaxLevel(maxLevel);
        resourceEditor.setLevel(initialLevel);
        resourceEditor.setHealthGain(healthGain);
        resourceEditor.setHealthGainThreshold(gainThreshold);
        resourceEditor.setDeficiencyHealthDrain(healthDrain);
        resourceEditor.setDeficiencyThreshold(drainThreshold);
        resourceEditor.setBaseUsage(baseUsage);
    }

    /// <summary>
    /// Set the parameters that all actions have in common. There may be an additional function for setting parameters specific to a particular action.
    /// </summary>
    /// <param name="aea">Action wrapper.</param>
    /// <param name="name">Name of action.</param>
    /// <param name="priority">Priority of action when placed in queue (lower number is higher priority).</param>
    /// <param name="timeCost">The amount of time required to carry out the action. If there is not enough time, the action will be placed back on the queue (which could be periodically cleared).</param>
    /// <param name="resourceCosts">The resource costs for performing the action. These costs don't take effect unless the creature has enough resources.</param>
    public static void setBasicActionParams(ActionEditorAbstract aea, string name, int priority,
                                    int timeCost, Dictionary<string, float> resourceCosts)
    {
        aea.setName(name);
        aea.setPriority(priority);
        aea.setTimeCost(timeCost);
        // add resource costs
        if (resourceCosts != null)
        {
            foreach (string key in resourceCosts.Keys)
            {
                aea.addResourceCost(key, resourceCosts[key]);

            }
        }

    }

    /// <summary>
    /// Set parameters for a resource consumption action.
    /// </summary>
    /// <param name="cle">Action wrapper.</param>
    /// <param name="neighborIndex">Neighbor index from which resource is consumed (typically 0 for center spot).</param>
    /// <param name="toConsume">Resource to be consumed.</param>
    public static void setConsumeParams(ConsumeFromLandEditor cle, int neighborIndex, string toConsume)
    {
        cle.setNeighborIndex(neighborIndex);
        cle.setResourceToConsume(toConsume);
    }

    /// <summary>
    /// Set parameters for the action to convert one set of resources into another.
    /// </summary>
    /// <param name="convEdit">Action wrapper.</param>
    /// <param name="amtToProd">The amount to convert (which gets multiplied by the conversion weights).</param>
    /// <param name="startResources">The reactant resources and their correspding weights.</param>
    /// <param name="endResources">The product resources and their corresponding weights.</param>
    public static void setConvertActionParams(ConvertEditor convEdit, float amtToProd, Dictionary<string, float> startResources,
                                           Dictionary<string, float> endResources)
    {
        convEdit.setAmtToProduce(amtToProd);
        foreach (string key in startResources.Keys)
        {
            convEdit.addStartResource(key, startResources[key]);
        }
        foreach (string key in endResources.Keys)
        {
            convEdit.addEndResource(key, endResources[key]);
        }
    }


    /// <summary>
    /// Set parameters for an action to deposit a resource.
    /// </summary>
    /// <param name="depEdit">Action wrapper.</param>
    /// <param name="neighborIndex">Index on which to deposit resource (typically 0).</param>
    /// <param name="produces">The name of the resource to deposit.</param>
    /// <param name="depositAmt">The amount to deposit.</param>
    public static void setDepositActionParams(DepositEditor depEdit, int neighborIndex, string produces, float depositAmt)
    {
        depEdit.setNeighborIndex(neighborIndex);
        depEdit.setDepositResource(produces);
        depEdit.setAmtToDeposit(depositAmt);
    }

    /// <summary>
    /// Create a template of a PhenotypeNetwork, to use on sensing adjacent creatures.
    /// </summary>
    /// <param name="phenoNetEditor">Wrapper for PhenotypeNetwork.</param>
    /// <param name="layer">Layer in which network exists (probably 0).</param>
    /// <param name="name">Name of network.</param>
    /// <param name="hiddenNodesPerLayer">Nodes per hidden layer.</param>
    /// <param name="layersHiddenNodes">Number of hidden layers in the network.</param>
    /// <param name="outputActionNames">Names of output actions, whose output networks will be connected to this network.</param>
    /// <param name="hiddenNodeActiv">The type of activation function used by the hidden nodes.</param>
    /// <param name="outputNodeActiv">The type of activation function used by the output nodes. Note: the output of this function should be in the range [0,1].</param>
    public static void createPhenotypeNet(PhenotypeNetworkEditor phenoNetEditor, int layer, string name, int hiddenNodesPerLayer, int layersHiddenNodes,
                                          List<string> outputActionNames, ActivationBehaviorTypes hiddenNodeActiv, ActivationBehaviorTypes outputNodeActiv)
    {
        phenoNetEditor.setInLayer(layer); // called by default with index of layer user clicked
        phenoNetEditor.setName(name);
        phenoNetEditor.createInputNodes();
        // set index of output layer based on hidden nodes
        int outputLayer = layersHiddenNodes + 1;
        // for every hidden layer
        for (int i = 0; i < layersHiddenNodes; i++)
        {
            phenoNetEditor.insertNewLayer(i + 1);
            // add every node in that layer
            for (int j = 0; j < hiddenNodesPerLayer; j++)
            {
                makeHiddenNode(phenoNetEditor, hiddenNodeActiv, i + 1);
            }
        }

        for (int i = 0; i < outputActionNames.Count; i++)
        {
            makeOutputNode(phenoNetEditor, outputNodeActiv, outputActionNames[i], outputLayer);
        }

    }



    /// <summary>
    /// Create a network to sense the levels of resources on surrounding Land spaces.
    /// </summary>
    /// <param name="netCreator">Network wrapper.</param>
    /// <param name="layer">Layer of network.</param>
    /// <param name="name">Name of network.</param>
    /// <param name="resourcesToSense">List or resources to sense from surrounding land spaces.</param>
    /// <param name="outputActions">List of output actions to associate the network with.</param>
    /// <param name="hiddenLayerNum">Number of hidden layers.</param>
    /// <param name="nodesPerLayer">Nodes per hidden layer.</param>
    /// <param name="hiddenActiv">Activation function of hidden nodes.</param>
    /// <param name="outputActiv">Activation function of output nodes.</param>
    public static void makeSensoryInputNetwork(NetworkEditor netCreator, int layer, string name, List<string> resourcesToSense, List<string> outputActions,
                                            int hiddenLayerNum, int nodesPerLayer, ActivationBehaviorTypes hiddenActiv, ActivationBehaviorTypes outputActiv)
    {
        netCreator.setInLayer(layer); // called by default with index of layer user clicked
        netCreator.setName(name);

        // for every resource
        for (int i = 0; i < resourcesToSense.Count; i++)
        {
            // for every neighbor
            for (int j = 0; j < 5; j++)
            {
                makeSensoryInputNode(netCreator, j, resourcesToSense[i]);
            }
        }

        int outputLayer = hiddenLayerNum + 1;

        // for every hidden layer
        for (int i = 0; i < hiddenLayerNum; i++)
        {
            netCreator.insertNewLayer(i + 1);
            // add every node in that layer
            for (int j = 0; j < nodesPerLayer; j++)
            {
                makeHiddenNode(netCreator, hiddenActiv, i + 1);
            }
        }

        for (int i = 0; i < outputActions.Count; i++)
        {
            makeOutputNode(netCreator, outputActiv, outputActions[i], outputLayer);
        }

    }


    /// <summary>
    /// Create a network that senses some or all of the creature's internal resource levels.
    /// </summary>
    /// <param name="netCreator">Network wrapper.</param>
    /// <param name="layer">Layer of network (probably 0).</param>
    /// <param name="name">Name of network.</param>
    /// <param name="resourcesToSense">A list of the internal resources that the network should sense.</param>
    /// <param name="outputActions">A list of the output actions that should be associated with this network (through output nodes connected to output networks).</param>
    /// <param name="hiddenLayerNum">Number of hidden layers.</param>
    /// <param name="nodesPerLayer">Number of nodes per hidden layer.</param>
    /// <param name="hiddenActiv">Activation function for hidden nodes.</param>
    /// <param name="outputActiv">Activation function for output nodes.</param>
    public static void makeInternalInputNetwork(NetworkEditor netCreator, int layer, string name, List<string> resourcesToSense, List<string> outputActions,
                                            int hiddenLayerNum, int nodesPerLayer, ActivationBehaviorTypes hiddenActiv, ActivationBehaviorTypes outputActiv)
    {
        netCreator.setInLayer(layer); // called by default with index of layer user clicked
        netCreator.setName(name);

        // for every resource
        for (int i = 0; i < resourcesToSense.Count; i++)
        {
            makeInternalResourceInputNode(netCreator, resourcesToSense[i]);
        }

        int outputLayer = hiddenLayerNum + 1;

        // for every hidden layer
        for (int i = 0; i < hiddenLayerNum; i++)
        {
            netCreator.insertNewLayer(i + 1);
            // add every node in that layer
            for (int j = 0; j < nodesPerLayer; j++)
            {
                makeHiddenNode(netCreator, hiddenActiv, i + 1);
            }
        }

        for (int i = 0; i < outputActions.Count; i++)
        {
            makeOutputNode(netCreator, outputActiv, outputActions[i], outputLayer);
        }
    }


    /// <summary>
    /// Create an output network (a network that decides on wether or not to take an action.
    /// </summary>
    /// <param name="outNetEditor">OutputNetwork wrapper.</param>
    /// <param name="outputAction">Name of the action associated with this network.</param>
    /// <param name="layer">The layer of this network's Dictionary in the List of Dictionaries of networks.</param>
    /// <param name="name">The name of the network.</param>
    /// <param name="action">Action that is associated with this output network.</param>
    /// <param name="hiddenLayerNum">Number of hidden layers in the network.</param>
    /// <param name="nodesPerLayer">Nodes per hidden layer.</param>
    /// <param name="hiddenActiv">The type of activation function used by the hidden nodes.</param>
    /// <param name="outputActiv">The type of activation function used by the output nodes. Note: the output of this function should be in the range [0,1].</param>
    /// <param name="networks">The networks of which this network is a part.</param>
    public static void createOutputNetwork(OutputNetworkEditor outNetEditor, string outputAction, int layer, string name, Action action, int hiddenLayerNum,
            int nodesPerLayer, ActivationBehaviorTypes hiddenActiv, ActivationBehaviorTypes outputActiv, List<Dictionary<string, Network>> networks)
    {
        // network added to second layer of networks
        outNetEditor.setInLayer(layer); // called by default with index of layer user clicked
        outNetEditor.setName(name);
        outNetEditor.setOutputAction(action);

        // for every network in previous layer
        foreach (string key in networks[layer - 1].Keys)
        {
            List<List<Node>> net = networks[layer - 1][key].net;
            int lastLayer = net.Count - 1;
            // for every node in the final layer of that network
            for (int i = 0; i < net[lastLayer].Count; i++)
            {
                OutputNode node = (OutputNode)net[lastLayer][i];
                // if the node's action is the same as output action:
                if (node.action.name.Equals(outputAction))
                {
                    // create inner input node to that node
                    makeInnerInputNode(outNetEditor, 0, key, layer - 1, i); // TODO: automate this linking processes
                }

            }

        }


        int outputLayer = hiddenLayerNum + 1;

        // for every hidden layer
        for (int i = 0; i < hiddenLayerNum; i++)
        {
            outNetEditor.insertNewLayer(i + 1);
            // add every node in that layer
            for (int j = 0; j < nodesPerLayer; j++)
            {
                makeHiddenNode(outNetEditor, hiddenActiv, i + 1);
            }
        }

        /* Node outNet 1,0 */
        makeOutputNode(outNetEditor, outputActiv, outputAction, outputLayer);
    }


    /// <summary>
    /// Create a series of output network for different actions, but otherwise identical.
    /// </summary>
    /// <param name="ce">Creature wrapper object.</param>
    /// <param name="netLayer">Layer in which the output networks belong (probably last layer).</param>
    /// <param name="names">Names of output networks to be made (could be any name) and their associated actions (must use exact action names).</param>
    /// <param name="hiddenLayerNum">Number of hidden layers in the network.</param>
    /// <param name="nodesPerLayer">Nodes per hidden layer.</param>
    /// <param name="hiddenActiv">The type of activation function used by the hidden nodes.</param>
    /// <param name="outputActiv">The type of activation function used by the output nodes. Note: the output of this function should be in the range [0,1].</param>
    public static void createOutputNetworks(CreatureEditor ce, int netLayer, Dictionary<string, string> names, int hiddenLayerNum, int nodesPerLayer,
                                                ActivationBehaviorTypes hiddenActiv, ActivationBehaviorTypes outputActiv)
    {
        OutputNetworkEditor outNetCreator;
        foreach (string netName in names.Keys)
        {
            outNetCreator = (OutputNetworkEditor)ce.addNetwork(NetworkType.output);
            createOutputNetwork(outNetCreator, names[netName], netLayer, netName, ce.creature.actionPool[names[netName]], hiddenLayerNum, nodesPerLayer,
                                hiddenActiv, outputActiv, ce.creature.networks);
            // user clicks save on creature creator
            ce.saveNetwork();
        }


    }



}
