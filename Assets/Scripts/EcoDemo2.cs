// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EcoDemo2 : DemoInterface
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
            userCreatesEcosystem(300);
            // add cat species
            userAddsSpecies("Creature1", ColorChoice.blue, 1f, "A", "C", "D", .95f, .01f, false, 1);
            // populate with low standard deviation from founder creature
            userPopulatesSpecies("Creature1", 1f, 300, 500);

            userAddsSpecies("Creature2", ColorChoice.green, 1f, "B", "D", "C", .95f, .01f, false, 2);
            // populate with low standard deviation from founder creature
            userPopulatesSpecies("Creature2", 1f, 300, 500);
        }
        else
        {
            // for debugging
            //Debug.Log(" Make eco called twice! ");
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
        setEcoParams(ecoCreator, 10, 4, 50);

        // create resources A, B, and C
        addResource(ecoCreator, "A", 100, 150, 10, .4f, 2f);
        ecoCreator.saveResource();

        addResource(ecoCreator, "B", 100, 150, 10, .4f, 2f);
        ecoCreator.saveResource();


        addResource(ecoCreator, "C", 100, 150, 10, .2f, 0); // not renewed
        ecoCreator.saveResource();

        addResource(ecoCreator, "D", 100, 150, 10, .2f, 0); // not renewed
        ecoCreator.saveResource(); 

        ecoCreator.saveResourceOptions(); // adds all resources to ecosystem resources

        // generate map
        ecoCreator.createMap();
        // max size ~ 320 X 320 (100,000 cells)
        // TODO: account for asymetric maps
        ecoCreator.mapEditor.generateMap(mapWidth, mapWidth);
        ecoCreator.mapEditor.addLERPXResource("A", 1f);
        ecoCreator.mapEditor.addLERPXResource("B", 1f);
        // small starting amount of B and C
        ecoCreator.mapEditor.addUniformResource("C", .2f); 
        ecoCreator.mapEditor.addUniformResource("D", .2f);
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
    public void userAddsSpecies(string name, ColorChoice color, float mutationDeviation, string primaryConsume,
                                string dependentOn, string produces, float mutationDeviationFraction, float lowestMutationDeviation,
                                bool nonLinearPhenotypeNet, int phenotype)
    {
        // when user clicks to start species creation process:
        CreatureEditor cc = ecoCreator.addCreature();

        setCreatureStats(cc, name, phenotype, 10, 1000, 700, 3, 10, mutationDeviation, color, true,
                        mutationDeviationFraction, lowestMutationDeviation, MutationDeviationCoefficientType.exponentialDecay);
        // user edits:
        

        List<string> ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);

        //Debug.Log("resource added to creature: " + ecosystemResources[0]);


        // add creature resource store for primary resource that creature needs
        ResourceEditor resourceCreator = cc.addResource();
        addCreatureResource(resourceCreator, primaryConsume, 100, 90, 1, 90, 5, 20, 1);
        cc.saveResource();

        // add creature resource store for resouce creature produces
        // Note: Creature 1 doesn't need this resource to survive (no health gain or drain)
        resourceCreator = cc.addResource();
        addCreatureResource(resourceCreator, produces, 100, 90, 0, 90, 0, 20, 1);
        cc.saveResource();


        // add creature resource store for resouce creature is dependent on
        resourceCreator = cc.addResource();
        // high starting level, so that population doesn't die out immediately
        addCreatureResource(resourceCreator, dependentOn, 200, 190, 1, 180, 5, 10, 1);
        cc.saveResource();

        // for reference later
        List<string> creatureResources = new List<string>(cc.creature.storedResources.Keys);

        // generates movement actions with a resource cost
        cc.generateDefaultActionPool(primaryConsume, 5);

        /* MUST GENERATE ACTIONS AND ADD THEM TO CREATURE'S ACTION POOL BEFORE CREATING OUTPUT NODES FOR THOSE ACTIONS */

        // add default abilities for consuming resources
        cc.addDefaultResourceAbilities();
        cc.saveAbilities();

        // create action for consuming primary resource
        ActionEditor ae = cc.addAction();
        ae.setCreator(ActionCreatorType.consumeCreator);
        ConsumeFromLandEditor cle = (ConsumeFromLandEditor)ae.getActionCreator();
        // define resource costs
        Dictionary<string, float> resourceCosts = new Dictionary<string, float>()
        {
            {primaryConsume, 1},
            {dependentOn, 1}
        };
        // set parameters
        setBasicActionParams(cle,  "eat" + primaryConsume, 1, 10, resourceCosts);
        setConsumeParams(cle, 0, primaryConsume);
        cc.saveAction();


        // create action for consuming Resource that creature is dependent on
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.consumeCreator);
        cle = (ConsumeFromLandEditor)ae.getActionCreator();
        // define resource costs
        resourceCosts = new Dictionary<string, float>()
        {
            {primaryConsume, 1},
            {dependentOn, 1}
        };
        // set parameters
        setBasicActionParams(cle, "eat" + dependentOn, 1, 10, resourceCosts);
        setConsumeParams(cle, 0, dependentOn);
        cc.saveAction();


        // create action for reproduction
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.reproduceCreator);
        ReproActionEditor rae = (ReproActionEditor)ae.getActionCreator();
        // high resource costs for reproduction
        resourceCosts = new Dictionary<string, float>()
        {
            {primaryConsume, 20},
            {dependentOn, 50}
        };
        setBasicActionParams(rae, "reproduce", 1, 10, resourceCosts);
        // no special params to set for reproduction yet
        cc.saveAction();


        // action for converting with a 1 to 2 ratio
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.convertEditor);
        ConvertEditor convEdit = (ConvertEditor)ae.getActionCreator();
        resourceCosts = new Dictionary<string, float>()
        {
            {primaryConsume, 1},
            {dependentOn, 1}
        };
        setBasicActionParams(convEdit, "convert" + primaryConsume + "To" + produces, 1, 10, resourceCosts);

        Dictionary<string, float> startResources = new Dictionary<string, float>()
        {
            {primaryConsume, 1f}
        };
        Dictionary<string, float> endResources = new Dictionary<string, float>()
        {
            {produces, 3f}
        };

        setConvertActionParams(convEdit, 5, startResources, endResources);
        cc.saveAction();


        // action for depositing B
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.depositEditor);
        DepositEditor depEdit = (DepositEditor)ae.getActionCreator();
        // no resource costs for depositing
        setBasicActionParams(depEdit, "deposit" + produces, 1, 10, null);
        setDepositActionParams(depEdit, 0, produces, 10);

        cc.saveAction();


        // user opens networks creator for that creature


        /**** phenotype network template ****/

        PhenotypeNetworkEditor phenoNetCreator = (PhenotypeNetworkEditor) cc.addNetwork(NetworkType.phenotype);
        
        List<string> phenoOutputActions = new List<string>()
        {
            "deposit" + produces,
            "convert" + primaryConsume + "To" + produces,
            "reproduce",
            "eat" + dependentOn,
            "eat" + primaryConsume,
            "moveUp",
            "moveDown",
            "moveLeft",
            "moveRight"

        };
        if (nonLinearPhenotypeNet)
        {
            createPhenotypeNet(phenoNetCreator, 0, "phenotypeNet", 4, 2, phenoOutputActions,
                           ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);
        }
        else
        {
            createPhenotypeNet(phenoNetCreator, 0, "phenotypeNet", 0, 0, phenoOutputActions,
                           ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);
        }

        // Note: don't call saveNetwork(), call savePhenotypeNetwork()
        cc.savePhenotypeNetwork();

        //Debug.Log("finished creating phenotype net");


        // create network to sense external resource levels

        /**** net1 ****/
        
        // user adds a network
        NetworkEditor netCreator = cc.addNetwork(NetworkType.regular);
        List<string> resourcesToSense = creatureResources; // sense resources creature can store
        List<string> outputActions = new List<string>()
        {
            "deposit" + produces,
            "convert" + primaryConsume + "To" + produces,
            "reproduce",
            "eat" + dependentOn,
            "eat" + primaryConsume,
            "moveUp",
            "moveDown",
            "moveLeft",
            "moveRight"

        };

        makeSensoryInputNetwork(netCreator, 0, "externalNet", resourcesToSense, outputActions, 1, 6,
                                ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);

        // user clicks save on network creator
        cc.saveNetwork();




        // sense internal levels of resources
        NetworkEditor InternalNetCreator = cc.addNetwork(NetworkType.regular);
        // sense all creature resources again, this time internally
        resourcesToSense = creatureResources;
        // use all output actions again
        outputActions = new List<string>()
        {
            "deposit" + produces,
            "convert" + primaryConsume + "To" + produces,
            "reproduce",
            "eat" + dependentOn,
            "eat" + primaryConsume,
            "moveUp",
            "moveDown",
            "moveLeft",
            "moveRight"

        };

        makeInternalInputNetwork(InternalNetCreator, 0, "internalNet", resourcesToSense, outputActions, 1, 6,
                                ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);

        // user clicks save on network creator
        cc.saveNetwork();




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
        makeInnerInputNode(netCreator2, 0, "externalNet", 0, 0); // TODO: automate this linking processes
        makeInnerInputNode(netCreator2, 0, "internalNet", 0, 0); // TODO: automate this linking processes

        netCreator2.insertNewLayer(1);

        makeHiddenNode(netCreator2, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator2, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator2, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator2, ActivationBehaviorTypes.LogisticAB, 1);

        /* Node outNet 1,0 */
        makeOutputNode(netCreator2, ActivationBehaviorTypes.LogisticAB, outputAction, 2);
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
        makeInnerInputNode(netCreator4, 0, "externalNet", 0, 1);
        makeInnerInputNode(netCreator4, 0, "internalNet", 0, 1);

        netCreator4.insertNewLayer(1);

        makeHiddenNode(netCreator4, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator4, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator4, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator4, ActivationBehaviorTypes.LogisticAB, 1);
        /* Node outNet 0,1 */
        //makeInnerInputNode(netCreator4, 0, "net2", 0, 1);

        /* Node outNet 1,0 */
        makeOutputNode(netCreator4, ActivationBehaviorTypes.LogisticAB, outputAction2, 2);
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
        makeInnerInputNode(netCreator6, 0, "externalNet", 0, 2);
        makeInnerInputNode(netCreator6, 0, "internalNet", 0, 2);

        netCreator6.insertNewLayer(1);

        makeHiddenNode(netCreator6, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator6, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator6, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator6, ActivationBehaviorTypes.LogisticAB, 1);

        /* Node outNet 1,0 */
        makeOutputNode(netCreator6, ActivationBehaviorTypes.LogisticAB, outputAction3, 2);
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
        makeInnerInputNode(netCreator7, 0, "externalNet", 0, 3);
        makeInnerInputNode(netCreator7, 0, "internalNet", 0, 3);

        netCreator7.insertNewLayer(1);

        makeHiddenNode(netCreator7, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator7, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator7, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreator7, ActivationBehaviorTypes.LogisticAB, 1);

        /* Node outNet 1,0 */
        makeOutputNode(netCreator7, ActivationBehaviorTypes.LogisticAB, outputAction4, 2);
        // user clicks save on creature creator
        cc.saveNetwork();


        /**** outNetConsumeA ****/

        // user adds a second network
        OutputNetworkEditor eatAOutNet = (OutputNetworkEditor) cc.addNetwork(NetworkType.output);
        string eatAOutAction = "eat" + primaryConsume;

        // network added to second layer of networks
        eatAOutNet.setInLayer(1); // called by default with index of layer user clicked
        eatAOutNet.setName("outNetEat" + primaryConsume);
        eatAOutNet.setOutputAction(cc.creature.actionPool[eatAOutAction]);

        /* Node outNet 0,0 */
        makeInnerInputNode(eatAOutNet, 0, "externalNet", 0, 4);
        makeInnerInputNode(eatAOutNet, 0, "internalNet", 0, 4);


        eatAOutNet.insertNewLayer(1);

        makeHiddenNode(eatAOutNet, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(eatAOutNet, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(eatAOutNet, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(eatAOutNet, ActivationBehaviorTypes.LogisticAB, 1);

        makeOutputNode(eatAOutNet, ActivationBehaviorTypes.LogisticAB, eatAOutAction, 2);
        // user clicks save on creature creator
        cc.saveNetwork();



        // user adds a second network
        OutputNetworkEditor eatCOutNet = (OutputNetworkEditor)cc.addNetwork(NetworkType.output);
        string eatCOutAction = "eat" + dependentOn;

        // network added to second layer of networks
        eatCOutNet.setInLayer(1); // called by default with index of layer user clicked
        eatCOutNet.setName("outNetEat" + dependentOn);
        eatCOutNet.setOutputAction(cc.creature.actionPool[eatCOutAction]);

        /* Node outNet 0,0 */
        makeInnerInputNode(eatCOutNet, 0, "externalNet", 0, 5);
        makeInnerInputNode(eatCOutNet, 0, "internalNet", 0, 5);


        eatCOutNet.insertNewLayer(1);

        makeHiddenNode(eatCOutNet, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(eatCOutNet, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(eatCOutNet, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(eatCOutNet, ActivationBehaviorTypes.LogisticAB, 1);

        /* Node outNet 1,0 */
        makeOutputNode(eatCOutNet, ActivationBehaviorTypes.LogisticAB, eatCOutAction, 2);
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
        makeInnerInputNode(netCreatorOutRepro, 0, "externalNet", 0, 6);
        makeInnerInputNode(netCreatorOutRepro, 0, "internalNet", 0, 6);
        
        netCreatorOutRepro.insertNewLayer(1);

        makeHiddenNode(netCreatorOutRepro, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreatorOutRepro, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreatorOutRepro, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreatorOutRepro, ActivationBehaviorTypes.LogisticAB, 1);

        /* Node outNet 1,0 */
        makeOutputNode(netCreatorOutRepro, ActivationBehaviorTypes.LogisticAB, outputAction6, 2);
        // user clicks save on creature creator
        cc.saveNetwork();



        /**** outNetDeposit ****/

        // user adds a second network
        OutputNetworkEditor netCreatorOutDeposit = (OutputNetworkEditor)cc.addNetwork(NetworkType.output);
        string depositAction = "deposit" + produces;

        // network added to second layer of networks
        netCreatorOutDeposit.setInLayer(1); // called by default with index of layer user clicked
        netCreatorOutDeposit.setName("outNetDeposit" + produces);
        netCreatorOutDeposit.setOutputAction(cc.creature.actionPool[depositAction]);

        /* Node outNet 0,0 */
        makeInnerInputNode(netCreatorOutDeposit, 0, "externalNet", 0, 7);
        makeInnerInputNode(netCreatorOutDeposit, 0, "internalNet", 0, 7);

        netCreatorOutDeposit.insertNewLayer(1);

        makeHiddenNode(netCreatorOutDeposit, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreatorOutDeposit, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreatorOutDeposit, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreatorOutDeposit, ActivationBehaviorTypes.LogisticAB, 1);

        /* Node outNet 1,0 */
        makeOutputNode(netCreatorOutDeposit, ActivationBehaviorTypes.LogisticAB, depositAction, 2);
        // user clicks save on creature creator
        cc.saveNetwork();



        /**** OutNetConvert ****/

        // user adds a second network
        OutputNetworkEditor netCreatorOutConvert = (OutputNetworkEditor)cc.addNetwork(NetworkType.output);
        string convertAction = "convert" + primaryConsume + "To" + produces;

        // network added to second layer of networks
        netCreatorOutConvert.setInLayer(1); // called by default with index of layer user clicked
        netCreatorOutConvert.setName("outNetConvert");
        netCreatorOutConvert.setOutputAction(cc.creature.actionPool[convertAction]);

        /* Node outNet 0,0 */
        makeInnerInputNode(netCreatorOutConvert, 0, "externalNet", 0, 8);
        makeInnerInputNode(netCreatorOutConvert, 0, "internalNet", 0, 8);
        // Note: linked nodes for phenotype nets are set on the fly in Creature

        netCreatorOutConvert.insertNewLayer(1);

        makeHiddenNode(netCreatorOutConvert, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreatorOutConvert, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreatorOutConvert, ActivationBehaviorTypes.LogisticAB, 1);
        makeHiddenNode(netCreatorOutConvert, ActivationBehaviorTypes.LogisticAB, 1);

        /* Node outNet 1,0 */
        makeOutputNode(netCreatorOutConvert, ActivationBehaviorTypes.LogisticAB, convertAction, 2);
        // user clicks save on creature creator
        cc.saveNetwork();



        //cc.creature.printNetworks();

        // adds creature to list of founders
        ecoCreator.addToFounders();
        // saves founders to ecosystem species list
        ecoCreator.saveFoundersToSpecies();


    }


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

    public void makeHiddenNode(NetworkEditor netCreator, ActivationBehaviorTypes activationType, int layer)
    {
        // user adds node to second layer
        NodeEditor nodeCreator = netCreator.addNode(layer);
        nodeCreator.setCreator(NodeCreatorType.hiddenNode);
        HiddenNodeEditor hne = (HiddenNodeEditor)nodeCreator.getNodeCreator();
        hne.setActivationFunction(activationType);
        netCreator.saveNode();
        // user clicks save on network creator
    }


    public void addResource(EcosystemEditor ecoEditor, string name, float initialAmt, float maxAmt,
                                float consumedPerTime, float proportionExtract, float renewAmt)
    {
        LandResourceEditor lre = ecoEditor.addResource(name);
        lre.setAmountOfResource(initialAmt);
        lre.setMaxAmt(maxAmt);
        lre.setAmtConsumedPerTime(consumedPerTime);
        lre.setProportionExtracted(proportionExtract); // higher proportion extracted for primary resources
        lre.setRenewalAmt(renewAmt);
    }

    public void setEcoParams(EcosystemEditor ecoEditor, int abilityPtsPerCreat, int commBits, int renewInterval)
    {
        ecoEditor.setAbilityPointsPerCreature(10);
        ecoEditor.setCommBits(4);
        ecoEditor.setDistinctPhenotypeNum(4);
        ecoEditor.setRenewInterval(50);
    }


    public void setCreatureStats(CreatureEditor ce, string name, int phenotype, float turnTime, float maxHealth, float initialHealth,
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

    public void addCreatureResource(ResourceEditor resourceEditor, string name, float maxLevel, float initialLevel, float healthGain,
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

    public void setBasicActionParams(ActionEditorAbstract aea, string name, int priority,
                                int timeCost, Dictionary<string,float> resourceCosts)
    {
        aea.setName(name);
        aea.setPriority(priority);
        aea.setTimeCost(timeCost);
        // add resource costs
        if(resourceCosts != null)
        {
            foreach (string key in resourceCosts.Keys)
            {
                aea.addResourceCost(key, resourceCosts[key]);

            }
        }
        
    }

    public void setConsumeParams(ConsumeFromLandEditor cle, int neighborIndex, string toConsume)
    {
        cle.setNeighborIndex(neighborIndex);
        cle.setResourceToConsume(toConsume);
    }

    public void setConvertActionParams(ConvertEditor convEdit, float amtToProd, Dictionary<string, float> startResources,
                                       Dictionary<string, float> endResources)
    {
        convEdit.setAmtToProduce(amtToProd);
        foreach(string key in startResources.Keys)
        {
            convEdit.addStartResource(key, startResources[key]);
        }
        foreach (string key in endResources.Keys)
        {
            convEdit.addEndResource(key, endResources[key]);
        }
    }


    public void setDepositActionParams(DepositEditor depEdit, int neighborIndex, string produces, float depositAmt)
    {
        depEdit.setNeighborIndex(neighborIndex);
        depEdit.setDepositResource(produces);
        depEdit.setAmtToDeposit(depositAmt);
    }

    public void createPhenotypeNet(PhenotypeNetworkEditor phenoNetEditor, int layer, string name, int hiddenNodesPerLayer, int layersHiddenNodes,
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



    public void makeSensoryInputNetwork(NetworkEditor netCreator, int layer, string name, List<string> resourcesToSense, List<string> outputActions,
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


    public void makeInternalInputNetwork(NetworkEditor netCreator, int layer, string name, List<string> resourcesToSense, List<string> outputActions,
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





}