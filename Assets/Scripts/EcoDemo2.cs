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
            createEcosystem(300);
            // add cat species
            addSpecies("Creature1", ColorChoice.blue, 1f, "A", "C", "D", .95f, .01f, false, 1);
            // populate with low standard deviation from founder creature
            populateSpecies("Creature1", 1f, 300, 500);

            addSpecies("Creature2", ColorChoice.green, 1f, "B", "D", "C", .95f, .01f, false, 2);
            // populate with low standard deviation from founder creature
            populateSpecies("Creature2", 1f, 300, 500);
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
    public void createEcosystem(int mapWidth)
    {
        ecosystem = new Ecosystem();

        ecoCreator = new EcosystemEditor(ecosystem);
        // set basic ecosystem parameters
        EcoCreationHelper.setEcoParams(ecoCreator, 10, 4, 50, true, false);

        // create resources A, B, and C
        EcoCreationHelper.addResource(ecoCreator, "A", 100, 150, 10, .4f, 2f);
        ecoCreator.saveResource();

        EcoCreationHelper.addResource(ecoCreator, "B", 100, 150, 10, .4f, 2f);
        ecoCreator.saveResource();


        EcoCreationHelper.addResource(ecoCreator, "C", 100, 150, 10, .2f, 0); // not renewed
        ecoCreator.saveResource();

        EcoCreationHelper.addResource(ecoCreator, "D", 100, 150, 10, .2f, 0); // not renewed
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
    public void addSpecies(string name, ColorChoice color, float mutationDeviation, string primaryConsume,
                                string dependentOn, string produces, float mutationDeviationFraction, float lowestMutationDeviation,
                                bool nonLinearPhenotypeNet, int phenotype)
    {
        // when user clicks to start species creation process:
        CreatureEditor cc = ecoCreator.addCreature();

        EcoCreationHelper.setCreatureStats(cc, name, phenotype, 10, 1000, 700, 3, 10, mutationDeviation, color, true,
                        mutationDeviationFraction, lowestMutationDeviation, MutationDeviationCoefficientType.exponentialDecay);
        // user edits:
        

        List<string> ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);

        //Debug.Log("resource added to creature: " + ecosystemResources[0]);


        // add creature resource store for primary resource that creature needs
        ResourceEditor resourceCreator = cc.addResource();
        EcoCreationHelper.addCreatureResource(resourceCreator, primaryConsume, 100, 90, 1, 90, 5, 20, 1);
        cc.saveResource();

        // add creature resource store for resouce creature produces
        // Note: Creature 1 doesn't need this resource to survive (no health gain or drain)
        resourceCreator = cc.addResource();
        EcoCreationHelper.addCreatureResource(resourceCreator, produces, 100, 90, 0, 90, 0, 20, 1);
        cc.saveResource();


        // add creature resource store for resouce creature is dependent on
        resourceCreator = cc.addResource();
        // high starting level, so that population doesn't die out immediately
        EcoCreationHelper.addCreatureResource(resourceCreator, dependentOn, 200, 190, 1, 180, 5, 10, 1);
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
        EcoCreationHelper.setBasicActionParams(cle,  "eat" + primaryConsume, 1, 10, resourceCosts);
        EcoCreationHelper.setConsumeParams(cle, 0, primaryConsume);
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
        EcoCreationHelper.setBasicActionParams(cle, "eat" + dependentOn, 1, 10, resourceCosts);
        EcoCreationHelper.setConsumeParams(cle, 0, dependentOn);
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
        EcoCreationHelper.setBasicActionParams(rae, "reproduce", 1, 10, resourceCosts);
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
        EcoCreationHelper.setBasicActionParams(convEdit, "convert" + primaryConsume + "To" + produces, 1, 10, resourceCosts);

        Dictionary<string, float> startResources = new Dictionary<string, float>()
        {
            {primaryConsume, 1f}
        };
        Dictionary<string, float> endResources = new Dictionary<string, float>()
        {
            {produces, 3f}
        };

        EcoCreationHelper.setConvertActionParams(convEdit, 5, startResources, endResources);
        cc.saveAction();


        // action for depositing B
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.depositEditor);
        DepositEditor depEdit = (DepositEditor)ae.getActionCreator();
        // no resource costs for depositing
        EcoCreationHelper.setBasicActionParams(depEdit, "deposit" + produces, 1, 10, null);
        EcoCreationHelper.setDepositActionParams(depEdit, 0, produces, 10);

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
            EcoCreationHelper.createPhenotypeNet(phenoNetCreator, 0, "phenotypeNet", 4, 2, phenoOutputActions,
                           ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);
        }
        else
        {
            EcoCreationHelper.createPhenotypeNet(phenoNetCreator, 0, "phenotypeNet", 0, 0, phenoOutputActions,
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

        EcoCreationHelper.makeSensoryInputNetwork(netCreator, 0, "externalNet", resourcesToSense, outputActions, 1, 6,
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

        EcoCreationHelper.makeInternalInputNetwork(InternalNetCreator, 0, "internalNet", resourcesToSense, outputActions, 1, 6,
                                ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);

        // user clicks save on network creator
        cc.saveNetwork();


        Dictionary<string, string> actionNameByNetName = new Dictionary<string, string>()
        {
            {"outNetUp", "moveUp" },
            {"outNetDown", "moveDown" },
            {"outNetLeft", "moveLeft" },
            {"outNetRight", "moveRight" },
            {"outNetEat" + primaryConsume , "eat" + primaryConsume },
            {"outNetEat" + dependentOn, "eat" + dependentOn},
            {"outNetRepro", "reproduce"},
            {"outNetDeposit" + produces, "deposit" + produces },
            {"outNetConvert",  "convert" + primaryConsume + "To" + produces}

        };

        EcoCreationHelper.createOutputNetworks(cc, 1, actionNameByNetName, 0, 0, ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);
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
    public void populateSpecies(string name, float populationDeviation, int popSize, int maxPopSize)
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





}