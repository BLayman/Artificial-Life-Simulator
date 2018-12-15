using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EcoManager
{
    /// <summary>
    /// Stores state of ecosystem.
    /// </summary>
    Ecosystem ecosystem;
    EcosystemCreator ecoCreator;

    public void makeEco()
    {
        userCreatesEcosystem();
        userAddsSpecies();
        userPopulatesSpecies();
    }


    /*
     * set ecosystem parameters,
     * create resources,
     * create map,
     * add resource to map
     * */
    public void userCreatesEcosystem()
    {
        ecosystem = new Ecosystem();

        ecoCreator = new EcosystemCreator(ecosystem);
        // set basic ecosystem parameters
        ecoCreator.setAbilityPointsPerCreature(10);
        ecoCreator.setCommBits(4);
        ecoCreator.setDistinctPhenotypeNum(32);
        ecoCreator.setTimeUnitsPerTurn(10);

        // create and save resources
        ecoCreator.addResource("grass");
        ecoCreator.lrc.setAmountOfResource(1500);
        ecoCreator.lrc.setMaxAmt(1500);
        ecoCreator.lrc.setAmtConsumedPerTime(100);
        ecoCreator.lrc.setProportionExtracted(.05f);
        ecoCreator.lrc.setRenewalAmt(10);
        ecoCreator.saveResource();

        ecoCreator.addResource("flowers");
        ecoCreator.lrc.setAmountOfResource(500);
        ecoCreator.lrc.setMaxAmt(500);
        ecoCreator.lrc.setAmtConsumedPerTime(100);
        ecoCreator.lrc.setProportionExtracted(.1f);
        ecoCreator.lrc.setRenewalAmt(1);
        ecoCreator.saveResource();// saves to tentative resources

        ecoCreator.saveResourceOptions(); // adds all resources to ecosystem resources

        // generate map
        ecoCreator.mapEditor = new MapEditor(ecoCreator.tentativeMap, ecoCreator.tentativeResourceOptions);
        ecoCreator.mapEditor.generateMap(50, 50);
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
    public void userAddsSpecies()
    {
        // when user clicks to start species creation process:
        CreatureCreator cc = ecoCreator.addCreature();

        // user edits:
        cc.setSpecies("Cat");
        cc.setPhenotype(3);

        // add resource for the creature to store
        ResourceCreator resourceCreator = cc.addResource();

        List<string> ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);

        //Debug.Log("resource added to creature: " + ecosystemResources[0]);

        resourceCreator.setName(ecosystemResources[0]);
        resourceCreator.setMaxLevel(100);
        resourceCreator.setLevel(90);
        resourceCreator.setHealthGain(1);
        resourceCreator.setHealthGainThreshold(50);
        resourceCreator.setDeficiencyHealthDrain(1);
        resourceCreator.setDeficiencyThreshold(10);
        resourceCreator.setBaseUsage(1);

        cc.saveResource();

        //Debug.Log("resource added to creature: " + ecosystemResources[1]);

        resourceCreator = cc.addResource();

        resourceCreator.setName(ecosystemResources[1]);
        resourceCreator.setMaxLevel(100);
        resourceCreator.setLevel(90);
        resourceCreator.setHealthGain(1);
        resourceCreator.setHealthGainThreshold(50);
        resourceCreator.setDeficiencyHealthDrain(1);
        resourceCreator.setDeficiencyThreshold(10);
        resourceCreator.setBaseUsage(1);

        cc.saveResource();


        List<string> creatureResources = new List<string>(cc.creature.storedResources.Keys);

        cc.generateCreatureActionPool();

        // user opens networks creator for that creature

        /* First Network */

        // user adds a network
        NetworkCreator netCreator = cc.addNetwork();
        netCreator.setInLayer(0); // called by default with index of layer user clicked
        netCreator.setName("net1");

        // user adds nodes to input layer (0)
        NodeCreator nodeCreator = netCreator.addNode(0);

        // user sets node type to sensory input node
        nodeCreator.setCreator(NodeCreatorType.siNodeCreator);

        // the sensory node editor gets it's sensory input node creator from nodeCreator
        SensoryInputNodeCreator sinc = (SensoryInputNodeCreator) nodeCreator.getNodeCreator();
        // the sinc is used to set properties on the sensory input node
        sinc.setLandIndex(4);
        sinc.setSensedResource(creatureResources[0]); // senses "grass"

        // user clicks save on node editor
        netCreator.saveNode();

        // user adds node to second layer
        NodeCreator nodeCreator2 = netCreator.addNode(1);

        // user sets node type to output node
        nodeCreator2.setCreator(NodeCreatorType.outputNodeCreator);

        // the output node editor gets it's output node creator from nodeCreator
        OutputNodeCreator onc = (OutputNodeCreator)nodeCreator2.getNodeCreator();
        // the onc is used to set properties on the output node
        onc.setAction("Move left");
        onc.setActivationFunction(ActivationBehaviorTypes.LogisticAB);

        // user clicks save on node editor
        netCreator.saveNode();

        // user clicks save on network creator
        cc.saveNetwork();

        /* Second Network */

        // user adds a network
        NetworkCreator netCreator3 = cc.addNetwork();
        netCreator3.setInLayer(0); // called by default with index of layer user clicked
        netCreator3.setName("net2");

        // user adds nodes to input layer (0)
        NodeCreator nodeCreator5 = netCreator3.addNode(0);

        // user sets node type to sensory input node
        nodeCreator5.setCreator(NodeCreatorType.siNodeCreator);

        // the sensory node editor gets it's sensory input node creator from nodeCreator
        SensoryInputNodeCreator sinc2 = (SensoryInputNodeCreator)nodeCreator5.getNodeCreator();
        // the sinc is used to set properties on the sensory input node
        sinc2.setLandIndex(4);
        sinc2.setSensedResource(creatureResources[1]); // senses "flowers"

        // user clicks save on node editor
        netCreator3.saveNode();

        // user adds node to second layer
        NodeCreator nodeCreator6 = netCreator3.addNode(1);

        // user sets node type to output node
        nodeCreator6.setCreator(NodeCreatorType.outputNodeCreator);

        // the output node editor gets it's output node creator from nodeCreator
        OutputNodeCreator onc3 = (OutputNodeCreator)nodeCreator6.getNodeCreator();
        // the onc is used to set properties on the output node
        onc3.setAction("Move left");
        onc3.setActivationFunction(ActivationBehaviorTypes.LogisticAB);

        // user clicks save on node editor
        netCreator3.saveNode();

        // user clicks save on network creator
        cc.saveNetwork();

        /* Third Network */

        // user adds a second network
        NetworkCreator netCreator2 = cc.addNetwork();
        // network added to second layer of networks
        netCreator2.setInLayer(1); // called by default with index of layer user clicked
        netCreator2.setName("outNet");

        // user adds nodes to input layer (0)
        NodeCreator nodeCreator3 = netCreator2.addNode(0);
        // user adds inner input node
        nodeCreator3.setCreator(NodeCreatorType.innerInputNodeCreator);
        InnerInputNodeCreator iinc = (InnerInputNodeCreator)nodeCreator3.getNodeCreator();
        // the inner input node gets its value from net1's output node at index 0
        iinc.setLinkedNode("net1", 0, 0);
        // user clicks save on node editor
        netCreator2.saveNode();


        // user adds nodes to input layer (0)
        NodeCreator nodeCreator7 = netCreator2.addNode(0);
        // user adds inner input node
        nodeCreator7.setCreator(NodeCreatorType.innerInputNodeCreator);
        InnerInputNodeCreator iinc2 = (InnerInputNodeCreator)nodeCreator7.getNodeCreator();
        // the inner input node gets its value from net1's output node at index 0
        iinc2.setLinkedNode("net2", 0, 0);
        // user clicks save on node editor
        netCreator2.saveNode();



        // user adds node to second layer
        NodeCreator nodeCreator4 = netCreator2.addNode(1);
        nodeCreator4.setCreator(NodeCreatorType.outputNodeCreator);
        OutputNodeCreator onc2 = (OutputNodeCreator)nodeCreator4.getNodeCreator();
        onc2.setAction("Move left");
        onc2.setActivationFunction(ActivationBehaviorTypes.LogisticAB);
        netCreator2.saveNode();

        // user clicks save on network creator
        cc.saveNetwork();

        // user clicks save on creature creator

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
    public void userPopulatesSpecies()
    {
        SpeciesPopulator populator = ecoCreator.populateSpecies("Cat");
        populator.SetAbilityStandardDeviation(1);
        populator.setNetworkWeightStandardDeviation(.1f);
        populator.populateRandom(10);
        ecoCreator.saveCurrentPopulation();
        ecoCreator.addCurrentPopulationToEcosystem();
        ecoCreator.addCurrentPopulationToMap();
        ecoCreator.saveMap(); // need to save updated tentative map

        // TODO: Include system for saving a temporary map to the actual map.
        // this applies to both the MapEditor, and SpeciesPopulator.
    }

    public void runSystem(int steps)
    {
        ecosystem.runSystem(steps);
    }

    public Ecosystem getEcosystem()
    {
        return ecosystem;
    }

}