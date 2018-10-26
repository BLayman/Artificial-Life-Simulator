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


    public void userCreatesEcosystem()
    {
        ecosystem = new Ecosystem();

        ecoCreator = new EcosystemCreator(ecosystem);
        ecoCreator.setAbilityPointsPerCreature(10);
        ecoCreator.setCommBits(4);
        ecoCreator.setDistinctPhenotypeNum(20);
        ecoCreator.generateMap(100, 100);
        ecoCreator.setTimeUnitsPerTurn(10);

        ecoCreator.addResource("grass");
        ecoCreator.lrc.setAmountOfResource(1000);
        ecoCreator.lrc.setAmtConsumedPerTime(100);
        ecoCreator.lrc.setProportionExtracted(.05f);
        ecoCreator.lrc.setRenewalAmt(10);
        ecoCreator.saveResource();

        ecoCreator.mapEditor.addUniformResource("grass", .5f); 
    }

    
    public void userAddsSpecies()
    {
        // when user clicks to start species creation process:
        ecoCreator.creatureCreator =  new CreatureCreator(new Creature(ecosystem.abilityPointsPerCreature));
        CreatureCreator cc = ecoCreator.creatureCreator;

        // user edits:
        cc.setSpecies("Cat");

        // user opens networks creator for that creature

        // user adds a network
        cc.netCreator = new NetworkCreator(new Network());
        cc.netCreator.setInLayer(0); // called by default with index of layer user clicked
        cc.netCreator.setName("net1");

        // user adds nodes to input layer (0)
        cc.netCreator.nodeCreator = new NodeCreator(0);

        // user sets node type to sensory input node
        cc.netCreator.nodeCreator.setCreator(NodeCreatorType.siNodeCreator);

        // the sensory node editor gets it's sensory input node creator from nodeCreator
        SensoryInputNodeCreator sinc = (SensoryInputNodeCreator) cc.netCreator.nodeCreator.getNodeCreator();
        // the sinc is used to set properties on the sensory input node
        sinc.setLandIndex(4);
        sinc.setSensedResource("grass");

        // user clicks save on node editor
        cc.netCreator.saveNode();

        // user clicks save on network creator
        cc.saveNetwork();

        // user clicks save on creature creator
        ecoCreator.addToFounders(cc.creature);
    }

    public void userPopulatesSpecies()
    {
        ecoCreator.populateSpecies("Cat");
        SpeciesPopulator populator = ecoCreator.speciesPopulator;
        populator.SetAbilityStandardDeviation(1);
        populator.setNetworkWeightStandardDeviation(.5f);
        populator.populateRandom(100);

        ecoCreator.saveFounders();
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