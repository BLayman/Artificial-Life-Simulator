using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        ecoCreator.setDistinctPhenotypeNum(16);
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
        ecoCreator.mapEditor.addLERPXResource("grass", .75f);
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

        // add resource for the creature to store
        ResourceCreator resourceCreator = cc.addResource();


        List<string> keyList = new List<string>(ecosystem.resourceOptions.Keys);
        Console.WriteLine("resource added to creature: " + keyList[0]);

        resourceCreator.setName(keyList[0]);
        resourceCreator.setMaxLevel(100);
        resourceCreator.setLevel(90);
        resourceCreator.setHealthGain(1);
        resourceCreator.setHealthGainThreshold(50);
        resourceCreator.setDeficiencyHealthDrain(1);
        resourceCreator.setDeficiencyThreshold(10);
        resourceCreator.setBaseUsage(1);

        cc.saveResource();

        // user opens networks creator for that creature

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
        sinc.setSensedResource(keyList[0]);

        // user clicks save on node editor
        netCreator.saveNode();

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
        populator.setNetworkWeightStandardDeviation(.5f);
        populator.populateRandom(100);
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