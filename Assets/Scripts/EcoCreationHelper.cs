using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



class EcoCreationHelper
{

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

    public static void setEcoParams(EcosystemEditor ecoEditor, int abilityPtsPerCreat, int commBits, int renewInterval)
    {
        ecoEditor.setAbilityPointsPerCreature(10);
        ecoEditor.setCommBits(4);
        ecoEditor.setDistinctPhenotypeNum(4);
        ecoEditor.setRenewInterval(50);
    }


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

    public static void setConsumeParams(ConsumeFromLandEditor cle, int neighborIndex, string toConsume)
    {
        cle.setNeighborIndex(neighborIndex);
        cle.setResourceToConsume(toConsume);
    }

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


    public static void setDepositActionParams(DepositEditor depEdit, int neighborIndex, string produces, float depositAmt)
    {
        depEdit.setNeighborIndex(neighborIndex);
        depEdit.setDepositResource(produces);
        depEdit.setAmtToDeposit(depositAmt);
    }

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




}
