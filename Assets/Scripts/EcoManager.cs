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
    EcosystemEditor ecoCreator;

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

        ecoCreator = new EcosystemEditor(ecosystem);
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
        ecoCreator.mapEditor.generateMap(100, 100);
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
        CreatureEditor cc = ecoCreator.addCreature();

        // user edits:
        cc.setSpecies("Cat");
        cc.setPhenotype(3);
        cc.setTurnTime(10);
        cc.setMaxHealth(100);
        cc.setInitialHealth(50);



        // add resource for the creature to store
        ResourceEditor resourceCreator = cc.addResource();

        List<string> ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);

        //Debug.Log("resource added to creature: " + ecosystemResources[0]);

        resourceCreator.setName(ecosystemResources[0]);
        resourceCreator.setMaxLevel(100);
        resourceCreator.setLevel(90);
        resourceCreator.setHealthGain(1);
        resourceCreator.setHealthGainThreshold(90);
        resourceCreator.setDeficiencyHealthDrain(5);
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


        // user opens networks creator for that creature


        /**** net1 ****/

        // user adds a network
        NetworkEditor netCreator = cc.addNetwork();
        netCreator.setInLayer(0); // called by default with index of layer user clicked
        netCreator.setName("net1");

        /* Node net1 0,0 */
        makeSensoryInputNode(netCreator, 1, creatureResources[0]);

        /* Node net1 1,0 */
        makeOutputNode(netCreator, ActivationBehaviorTypes.LogisticAB, "moveUp", 1);
        /* Node net1 1,1 */
        makeOutputNode(netCreator, ActivationBehaviorTypes.LogisticAB, "moveDown", 1);

        // user clicks save on network creator
        cc.saveNetwork();



        /**** net2 ****/

        // user adds a network
        NetworkEditor netCreator3 = cc.addNetwork();
        netCreator3.setInLayer(0); // called by default with index of layer user clicked
        netCreator3.setName("net2");

        /* Node net2 0,0 */
        makeSensoryInputNode(netCreator3, 1, creatureResources[1]);

        /* Node net2 1,0 */
        makeOutputNode(netCreator3, ActivationBehaviorTypes.LogisticAB, "moveUp", 1);
        /* Node net2 1,1 */
        makeOutputNode(netCreator3, ActivationBehaviorTypes.LogisticAB, "moveDown", 1);

        // user clicks save on network creator
        cc.saveNetwork();



        /**** outNetUp ****/

        // user adds a second network
        NetworkEditor netCreator2 = cc.addNetwork();
        // network added to second layer of networks
        netCreator2.setInLayer(1); // called by default with index of layer user clicked
        netCreator2.setName("outNetUp");

        /* Node outNet 0,0 */
        makeInnerInputNode(netCreator2, 0, "net1", 0, 0);

        /* Node outNet 0,1 */
        makeInnerInputNode(netCreator2, 0, "net2", 0, 0);

        /* Node outNet 1,0 */
        makeOutputNode(netCreator2, ActivationBehaviorTypes.LogisticAB, "moveUp", 1);
        // user clicks save on creature creator
        cc.saveNetwork();


        /**** outNetDown ****/

        // user adds a second network
        NetworkEditor netCreator4 = cc.addNetwork();
        // network added to second layer of networks
        netCreator4.setInLayer(1); // called by default with index of layer user clicked
        netCreator4.setName("outNetDown");

        /* Node outNet 0,0 */
        makeInnerInputNode(netCreator4, 0, "net1", 0, 1);

        /* Node outNet 0,1 */
        makeInnerInputNode(netCreator4, 0, "net2", 0, 1);

        /* Node outNet 1,0 */
        makeOutputNode(netCreator4, ActivationBehaviorTypes.LogisticAB, "moveDown", 1);
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
    public void userPopulatesSpecies()
    {
        SpeciesPopulator populator = ecoCreator.populateSpecies("Cat");
        populator.SetAbilityStandardDeviation(1);
        populator.setNetworkWeightStandardDeviation(.1f);
        populator.populateRandom(20);
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


    public void makeOutputNode(NetworkEditor netCreator, ActivationBehaviorTypes activationType, string action, int layer)
    {
        // user adds node to second layer
        NodeEditor nodeCreator = netCreator.addNode(layer);
        nodeCreator.setCreator(NodeCreatorType.outputNodeCreator);
        OutputNodeEditor onc = (OutputNodeEditor)nodeCreator.getNodeCreator();
        onc.setAction(action);
        onc.setActivationFunction(activationType);
        netCreator.saveNode();
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