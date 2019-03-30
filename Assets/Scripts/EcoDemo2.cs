// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EcoDemo2
{
    /// <summary>
    /// Stores state of ecosystem.
    /// </summary>
    Ecosystem ecosystem;
    EcosystemEditor ecoCreator;
    bool called = false;

    public void makeEco()
    {
        if (!called)
        {
            // Create a 300 X 300 map
            userCreatesEcosystem(200);
            // add cat species
            userAddsSpecies("cat", ColorChoice.blue, .01f);
            // populate with low standard deviation from founder creature
            userPopulatesSpecies("cat", .1f, 100, 300);
            // add dog species
            userAddsSpecies("dog", ColorChoice.green, .01f);
            //populate dog with high amount of variation in weights
            userPopulatesSpecies("dog", 2f, 100, 300);

            //userAddsSpecies("cow", ColorChoice.red, .01f);
            //userPopulatesSpecies("cow", 2f, 100, 300);
        }
        else
        {
            // for debugging
            Debug.Log(" Make eco called twice! ");
        }
    }



    /*
     * set ecosystem parameters,
     * create resources,
     * create map,
     * add resource to map
     * */
    public void userCreatesEcosystem(int mapWidth)
    {
        ecosystem = new Ecosystem();

        ecoCreator = new EcosystemEditor(ecosystem);
        // set basic ecosystem parameters
        ecoCreator.setAbilityPointsPerCreature(10);
        ecoCreator.setCommBits(4);
        ecoCreator.setDistinctPhenotypeNum(3);
        ecoCreator.setTimeUnitsPerTurn(10);
        ecoCreator.setRenewInterval(50);

        // create and save resources
        LandResourceEditor lre = ecoCreator.addResource("grass");
        lre.setAmountOfResource(100);
        lre.setMaxAmt(150);
        lre.setAmtConsumedPerTime(10);
        lre.setProportionExtracted(.2f);
        lre.setRenewalAmt(2f);

        ecoCreator.saveResource();
        
        ecoCreator.addResource("flowers");
        ecoCreator.lre.setAmountOfResource(500);
        ecoCreator.lre.setMaxAmt(500);
        ecoCreator.lre.setAmtConsumedPerTime(10);
        ecoCreator.lre.setProportionExtracted(.2f);
        ecoCreator.lre.setRenewalAmt(1);
        ecoCreator.saveResource();// saves to tentative resources
        

        ecoCreator.saveResourceOptions(); // adds all resources to ecosystem resources

        // generate map
        ecoCreator.createMap();
        // max size ~ 320 X 320 (100,000 cells)
        // TODO: account for asymetric maps
        ecoCreator.mapEditor.generateMap(mapWidth, mapWidth);
        ecoCreator.mapEditor.addLERPXResource("grass", 1f);
        ecoCreator.mapEditor.addLERPXResource("flowers", 1f);
        ecoCreator.saveEditedMap(); // saves to tentative map
        ecoCreator.saveMap(); // saves to ecosystem map
        
    }

    /*
     * create creature,
     * create and save creature resource,
     * create creature network,
     * create network node,
     * add resource to node, 
     * save creature to founder creatures dict and species dict
     */
    public void userAddsSpecies(string name, ColorChoice color, float mutationDeviation)
    {
        // when user clicks to start species creation process:
        CreatureEditor cc = ecoCreator.addCreature();

        // user edits:
        cc.setSpecies(name);
        cc.setPhenotype(1);
        cc.setTurnTime(10);
        cc.setMaxHealth(1000);
        cc.setInitialHealth(700);
        cc.setActionClearInterval(3);
        cc.setActionClearSize(10);
        cc.setMutationStandardDeviation(mutationDeviation);
        cc.setColor(color);
        cc.setUsePhenotypeNet(true);

        // add resource for the creature to store
        ResourceEditor resourceCreator = cc.addResource();

        List<string> ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);

        //Debug.Log("resource added to creature: " + ecosystemResources[0]);

        resourceCreator.setName(ecosystemResources[0]);
        resourceCreator.setMaxLevel(100);
        resourceCreator.setLevel(50);
        resourceCreator.setHealthGain(1);
        resourceCreator.setHealthGainThreshold(90);
        resourceCreator.setDeficiencyHealthDrain(10);
        resourceCreator.setDeficiencyThreshold(20);
        resourceCreator.setBaseUsage(1);

        cc.saveResource();

        //Debug.Log("resource added to creature: " + ecosystemResources[1]);

        resourceCreator = cc.addResource();

        
        resourceCreator.setName(ecosystemResources[1]);
        resourceCreator.setMaxLevel(100);
        resourceCreator.setLevel(90);
        resourceCreator.setHealthGain(1);
        resourceCreator.setHealthGainThreshold(90);
        resourceCreator.setDeficiencyHealthDrain(5);
        resourceCreator.setDeficiencyThreshold(20);
        resourceCreator.setBaseUsage(1);

        cc.saveResource();
        

        List<string> creatureResources = new List<string>(cc.creature.storedResources.Keys);


        // TODO create default actions for creature action pool, and example user made action 
        // (should use add an action creator to creature creator)
        cc.generateDefaultActionPool();

        /* MUST GENERATE ACTIONS AND ADD THEM TO CREATURE'S ACTION POOL BEFORE CREATING OUTPUT NODES FOR THOSE ACTIONS */


        // add default abilities for consuming resources
        cc.addDefaultResourceAbilities();
        cc.saveAbilities();

        // create action for consuming grass
        ActionEditor ae = cc.addAction();
        ae.setCreator(ActionCreatorType.consumeCreator);
        ConsumeFromLandEditor cle = (ConsumeFromLandEditor)ae.getActionCreator();
        cle.setName("eatGrass");
        cle.setNeighborIndex(0);
        cle.setResourceToConsume("grass");
        cle.setPriority(1);
        cle.setTimeCost(10);
        cle.addResourceCost("grass", 1);
        cc.saveAction();

        // create action for reproduction
        ActionEditor ae2 = cc.addAction();
        ae2.setCreator(ActionCreatorType.reproduceCreator);
        ReproActionEditor rae = (ReproActionEditor)ae2.getActionCreator();
        rae.setName("reproduce");
        rae.setPriority(1);
        rae.setTimeCost(10);
        rae.addResourceCost("grass", 20);
        cc.saveAction();

        // action for converting grass resource into flowers 
        ActionEditor ae3 = cc.addAction();
        ae3.setCreator(ActionCreatorType.convertEditor);
        ConvertEditor convEdit = (ConvertEditor)ae3.getActionCreator();
        convEdit.setName("convertGrassToFlowers");
        convEdit.setPriority(1);
        convEdit.setTimeCost(10);
        convEdit.setAmtToProduce(5);
        convEdit.addStartResource("grass", 1);
        convEdit.addEndResource("flowers", 2);
        cc.saveAction();
        // TODO: add to neural network

        // action for 
        ActionEditor ae4 = cc.addAction();
        ae4.setCreator(ActionCreatorType.depositEditor);
        DepositEditor depEdit = (DepositEditor)ae4.getActionCreator();
        depEdit.setName("depositFlowers");
        depEdit.setNeighborIndex(0);
        depEdit.setDepositResource("flowers");
        depEdit.setAmtToDeposit(10);
        depEdit.setPriority(1);
        depEdit.setTimeCost(10);
        cc.saveAction();
        // TODO: add to neural network


        /*
        ActionEditor ae2 = cc.addAction();
        ae2.setCreator(ActionCreatorType.consumeCreator);
        ConsumeFromLandEditor cle2 = (ConsumeFromLandEditor)ae2.getActionCreator();
        cle2.setName("eat");
        cle2.setNeighborIndex(3);
        cle2.setResourceToConsume("flowers");
        cle2.setPriority(1);
        cle2.setTimeCost(5);
        cle2.addResourceCost("flowers", 1);
        cc.saveAction();
        */

        // user opens networks creator for that creature


        /**** phenotype network template ****/

        
        // user adds a network
        PhenotypeNetworkEditor phenoNetCreator = (PhenotypeNetworkEditor) cc.addNetwork(NetworkType.phenotype);
        phenoNetCreator.setInLayer(0); // called by default with index of layer user clicked
        phenoNetCreator.setName("phenotypeNet");
        phenoNetCreator.createInputNodes();
   
        /* Node phenotypeNet 1,0 */
        makeOutputNode(phenoNetCreator, ActivationBehaviorTypes.LogisticAB, "reproduce", 1);

        // Note: don't call saveNetwork, call savePhenotypeNetwork
        cc.savePhenotypeNetwork();

        Debug.Log("finished creating phenotype net");

        //cc.creature.printNetworks();
        
        
        /**** net1 ****/
        
        // user adds a network
        NetworkEditor netCreator = cc.addNetwork(NetworkType.regular);
        netCreator.setInLayer(0); // called by default with index of layer user clicked
        netCreator.setName("net1");

        // Node net1 0,0 
        // sense resource 0 up
        makeSensoryInputNode(netCreator, 1, creatureResources[0]);

        // Node net1 0,1 
        // sense resource 0 down
        makeSensoryInputNode(netCreator, 2, creatureResources[0]);

        // Node net1 0,2 
        // sense resource 0 left
        makeSensoryInputNode(netCreator, 3, creatureResources[0]);

        // Node net1 0,3 
        // sense resource 0 right
        makeSensoryInputNode(netCreator, 4, creatureResources[0]);

        // Node net1 0,4 
        // sense resource 0 at current location
        makeSensoryInputNode(netCreator, 0, creatureResources[0]);

        // Node net1 0,4 
        // sense internal level of resource 0
        makeInternalResourceInputNode(netCreator, creatureResources[0]);
        

        // Node net1 1,0 
        makeOutputNode(netCreator, ActivationBehaviorTypes.LogisticAB, "moveUp", 1);
        // Node net1 1,1 
        makeOutputNode(netCreator, ActivationBehaviorTypes.LogisticAB, "moveDown", 1);
        // Node net1 1,2 
        makeOutputNode(netCreator, ActivationBehaviorTypes.LogisticAB, "moveLeft", 1);
        // Node net1 1,3 
        makeOutputNode(netCreator, ActivationBehaviorTypes.LogisticAB, "moveRight", 1);
        // Node net1 1,4 
        makeOutputNode(netCreator, ActivationBehaviorTypes.LogisticAB, "eatGrass", 1);
        // Node net1 1,5 
        //makeOutputNode(netCreator, ActivationBehaviorTypes.LogisticAB, "reproduce", 1);

        // user clicks save on network creator
        cc.saveNetwork();

        
        
        
        /**** net2 ****/
        /*
        // user adds a network
        NetworkEditor netCreator3 = cc.addNetwork();
        netCreator3.setInLayer(0); // called by default with index of layer user clicked
        netCreator3.setName("net2");

        // Node net2 0,0 
        // sense resource up
        makeSensoryInputNode(netCreator3, 1, creatureResources[1]);

        // Node net2 0,1 
        // sense resource down
        makeSensoryInputNode(netCreator3, 2, creatureResources[1]);

        // Node net2 1,0 
        makeOutputNode(netCreator3, ActivationBehaviorTypes.LogisticAB, "moveUp", 1);
        // Node net2 1,1 
        makeOutputNode(netCreator3, ActivationBehaviorTypes.LogisticAB, "moveDown", 1);

        // user clicks save on network creator
        cc.saveNetwork();
        */
        

        /**** outNetUp ****/

        // user adds a second network
        OutputNetworkEditor netCreator2 = (OutputNetworkEditor) cc.addNetwork(NetworkType.output);
        string outputAction = "moveUp";
        // network added to second layer of networks
        netCreator2.setInLayer(1); // called by default with index of layer user clicked
        netCreator2.setName("outNetUp");
        netCreator2.setOutputAction(cc.creature.actionPool[outputAction]);
        /* Node outNet 0,0 */
        // insert a node into 0th layer new network. Connect it to the 0th node in the last layer of net1 (net1 is in layer 0)
        makeInnerInputNode(netCreator2, 0, "net1", 0, 0);

        /* Node outNet 0,1 */
        //makeInnerInputNode(netCreator2, 0, "net2", 0, 0);

        /* Node outNet 1,0 */
        makeOutputNode(netCreator2, ActivationBehaviorTypes.LogisticAB, outputAction, 1);
        // user clicks save on creature creator
        cc.saveNetwork();


        /**** outNetDown ****/

        // user adds a second network
        OutputNetworkEditor netCreator4 = (OutputNetworkEditor) cc.addNetwork(NetworkType.output);
        string outputAction2 = "moveDown";
        // network added to second layer of networks
        netCreator4.setInLayer(1); // called by default with index of layer user clicked
        netCreator4.setName("outNetDown");
        netCreator4.setOutputAction(cc.creature.actionPool[outputAction2]);
        /* Node outNet 0,0 */
        // insert a node into 0th layer new network. Connect it to the index 1 node in the last layer of net1 (net1 is in layer 0)
        makeInnerInputNode(netCreator4, 0, "net1", 0, 1);

        /* Node outNet 0,1 */
        //makeInnerInputNode(netCreator4, 0, "net2", 0, 1);

        /* Node outNet 1,0 */
        makeOutputNode(netCreator4, ActivationBehaviorTypes.LogisticAB, outputAction2, 1);
        // user clicks save on creature creator
        cc.saveNetwork();


        /**** outNetLeft ****/

        // user adds a second network
        OutputNetworkEditor netCreator6 = (OutputNetworkEditor) cc.addNetwork(NetworkType.output);
        string outputAction3 = "moveLeft";
        // network added to second layer of networks
        netCreator6.setInLayer(1); // called by default with index of layer user clicked
        netCreator6.setName("outNetLeft");
        netCreator6.setOutputAction(cc.creature.actionPool[outputAction3]);

        /* Node outNet 0,0 */
        // insert a node into 0th layer new network. Connect it to the index 1 node in the last layer of net1 (net1 is in layer 0)
        makeInnerInputNode(netCreator6, 0, "net1", 0, 2);

        /* Node outNet 1,0 */
        makeOutputNode(netCreator6, ActivationBehaviorTypes.LogisticAB, outputAction3, 1);
        // user clicks save on creature creator
        cc.saveNetwork();


        /**** outNetRight ****/

        // user adds a second network
        OutputNetworkEditor netCreator7 = (OutputNetworkEditor) cc.addNetwork(NetworkType.output);
        string outputAction4 = "moveRight";

        // network added to second layer of networks
        netCreator7.setInLayer(1); // called by default with index of layer user clicked
        netCreator7.setName("outNetRight");
        netCreator7.setOutputAction(cc.creature.actionPool[outputAction4]);

        /* Node outNet 0,0 */
        // insert a node into 0th layer new network. Connect it to the index 1 node in the last layer of net1 (net1 is in layer 0)
        makeInnerInputNode(netCreator7, 0, "net1", 0, 3);


        /* Node outNet 1,0 */
        makeOutputNode(netCreator7, ActivationBehaviorTypes.LogisticAB, outputAction4, 1);
        // user clicks save on creature creator
        cc.saveNetwork();

        /**** outNetConsume ****/

        // user adds a second network
        OutputNetworkEditor netCreator5 = (OutputNetworkEditor) cc.addNetwork(NetworkType.output);
        string outputAction5 = "eatGrass";

        // network added to second layer of networks
        netCreator5.setInLayer(1); // called by default with index of layer user clicked
        netCreator5.setName("outNetEat");
        netCreator5.setOutputAction(cc.creature.actionPool[outputAction5]);

        /* Node outNet 0,0 */
        makeInnerInputNode(netCreator5, 0, "net1", 0, 4);

        /* Node outNet 0,1 */
        //makeInnerInputNode(netCreator5, 0, "net2", 0, 2);

        /* Node outNet 1,0 */
        makeOutputNode(netCreator5, ActivationBehaviorTypes.LogisticAB, outputAction5, 1);
        // user clicks save on creature creator
        cc.saveNetwork();


        /**** outNetReproduce ****/

        // user adds a second network
        OutputNetworkEditor netCreatorOutRepro = (OutputNetworkEditor) cc.addNetwork(NetworkType.output);
        string outputAction6 = "reproduce";

        // network added to second layer of networks
        netCreatorOutRepro.setInLayer(1); // called by default with index of layer user clicked
        netCreatorOutRepro.setName("outNetRepro");
        netCreatorOutRepro.setOutputAction(cc.creature.actionPool[outputAction6]);

        /* Node outNet 0,0 */
        //makeInnerInputNode(netCreatorOutRepro, 0, "net1", 0, 5);

        /* Node outNet 0,1 */
        //makeInnerInputNode(netCreator5, 0, "net2", 0, 2);

        /* Node outNet 1,0 */
        makeOutputNode(netCreatorOutRepro, ActivationBehaviorTypes.LogisticAB, outputAction6, 1);
        // user clicks save on creature creator
        cc.saveNetwork();


        //cc.creature.printNetworks();

        // adds creature to list of founders
        ecoCreator.addToFounders();
        // saves founders to ecosystem species list
        ecoCreator.saveFoundersToSpecies();


    }

    /*
    public void userAddsSpecies2(string name, ColorChoice color, float mutationDeviation)
    {
       
    }
    */



    /*
     * Create populator (with population),
     * set population parameters
     * generate population
     * saves population and adds it to list of populations
     * adds population to map, and saves map
     * */
    public void userPopulatesSpecies(string name, float populationDeviation, int popSize, int maxPopSize)
    {
        SpeciesPopulator populator = ecoCreator.populateSpecies(name);
        populator.SetAbilityStandardDeviation(1);
        populator.setNetworkWeightStandardDeviation(populationDeviation);
        populator.setMaxPopSize(maxPopSize);
        populator.populateRandom(popSize);
        ecoCreator.saveCurrentPopulation();
        ecoCreator.addCurrentPopulationToEcosystem();
        ecoCreator.addCurrentPopulationToMap();
        ecoCreator.saveMap(); // need to save updated tentative map

        // TODO: Include system for saving a temporary map to the actual map.
        // this applies to both the MapEditor, and SpeciesPopulator.
    }

    // this will be the multi-threaded part
    public void runSystem(int steps)
    {
        ecosystem.runSystem(steps);
    }

    public Ecosystem getEcosystem()
    {
        return ecosystem;
    }


    public void makeSensoryInputNode(NetworkEditor netCreator, int landIndex, string sensedResource)
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

    public void makeInternalResourceInputNode(NetworkEditor netCreator, string sensedResource)
    {
        NodeEditor nodeCreator = netCreator.addNode(0); // add to first layer
        // user sets node type to sensory input node
        nodeCreator.setCreator(NodeCreatorType.internalResNodeEditor);
        InternalResInputNodeEditor irnc = (InternalResInputNodeEditor)nodeCreator.getNodeCreator();
        irnc.setSensedResource(sensedResource);
        // user clicks save on node editor
        netCreator.saveNode();
    }


    public void makeOutputNode(NetworkEditor netCreator, ActivationBehaviorTypes activationType, string action, int layer)
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




    public void makeInnerInputNode(NetworkEditor netCreator, int layer, string linkedNetName, int linkedNetIndex, int linkedNodeIndex)
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

}