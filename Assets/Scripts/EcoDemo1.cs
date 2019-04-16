// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class EcoDemo1 : DemoInterface
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
            userAddsSpecies("cow", ColorChoice.blue, 1f, true, .95f, .01f);
            // populate with low standard deviation from founder creature
            userPopulatesSpecies("cow", 1f, 300, 500);
            // add dog species
            //userAddsSpecies("dog", ColorChoice.green, .01f);
            //populate dog with high amount of variation in weights
            //userPopulatesSpecies("dog", 2f, 100, 300);

            //userAddsSpecies("cow", ColorChoice.red, .01f);
            //userPopulatesSpecies("cow", 2f, 100, 300);
        }
        else
        {
            // for debugging
            // Debug.Log(" Make eco called twice! ");
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
        EcoCreationHelper.setEcoParams(ecoCreator, 10, 32, 50);

        // create hay and grass
        EcoCreationHelper.addResource(ecoCreator, "grass", 100, 150, 5, .2f, .5f);
        ecoCreator.saveResource();


        ecoCreator.saveResourceOptions(); // adds all resources to ecosystem resources

        // generate map
        ecoCreator.createMap();
        // TODO: account for asymetric maps
        ecoCreator.mapEditor.generateMap(mapWidth, mapWidth);
        ecoCreator.mapEditor.addLERPXResource("grass", 1f);
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
    public void userAddsSpecies(string name, ColorChoice color, float mutationDeviation, bool useHiddenNodes, float mutationDeviationFraction, float lowestMutationDeviation)
    {
        // when user clicks to start species creation process:
        CreatureEditor cc = ecoCreator.addCreature();

        EcoCreationHelper.setCreatureStats(cc, name, 3, 10, 1000, 700, 3, 10, mutationDeviation, color, false,
                        mutationDeviationFraction, lowestMutationDeviation, MutationDeviationCoefficientType.exponentialDecay);


        // add resource for the creature to store
        ResourceEditor resourceCreator = cc.addResource();

        List<string> ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);
        EcoCreationHelper.addCreatureResource(resourceCreator, "grass", 100, 50, 1, 90, 10, 20, 1);
        cc.saveResource();

        // for future reference
        List<string> creatureResources = new List<string>(cc.creature.storedResources.Keys);


        // TODO create default actions for creature action pool, and example user made action 
        // (should use add an action creator to creature creator)
        cc.generateDefaultActionPool("grass", 5);

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
            {"grass", 1},
        };
        // set parameters
        EcoCreationHelper.setBasicActionParams(cle, "eatGrass", 1, 10, resourceCosts);
        EcoCreationHelper.setConsumeParams(cle, 0, "grass");
        cc.saveAction();

        // create action for reproduction
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.reproduceCreator);
        ReproActionEditor rae = (ReproActionEditor)ae.getActionCreator();
        // high resource costs for reproduction
        resourceCosts = new Dictionary<string, float>()
        {
            {"grass", 20}
        };
        EcoCreationHelper.setBasicActionParams(rae, "reproduce", 1, 10, resourceCosts);
        // no special params to set for reproduction yet
        cc.saveAction();


        // user opens networks creator for that creature


        // user adds a network
        NetworkEditor netCreator = cc.addNetwork(NetworkType.regular);
        List<string> resourcesToSense = creatureResources; // sense resources creature can store
        List<string> outputActions = new List<string>()
        {
            "reproduce",
            "eatGrass",
            "moveUp",
            "moveDown",
            "moveLeft",
            "moveRight"

        };

        EcoCreationHelper.makeSensoryInputNetwork(netCreator, 0, "SensoryNet", resourcesToSense, outputActions, 1, 6,
                                ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);

        // user clicks save on network creator
        cc.saveNetwork();


        /**** outNetUp ****/

        OutputNetworkEditor outNetCreator = (OutputNetworkEditor)cc.addNetwork(NetworkType.output);
        EcoCreationHelper.createOutputNetwork(outNetCreator, "moveUp", 1, "outNetUp", cc.creature.actionPool["moveUp"], 0, 0,
                            ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB, cc.creature.networks);
        // user clicks save on creature creator
        cc.saveNetwork();


        /**** outNetDown ****/

        outNetCreator = (OutputNetworkEditor)cc.addNetwork(NetworkType.output);
        EcoCreationHelper.createOutputNetwork(outNetCreator, "moveDown", 1, "outNetDown", cc.creature.actionPool["moveDown"], 0, 0,
                            ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB, cc.creature.networks);
        // user clicks save on creature creator
        cc.saveNetwork();


        /**** outNetLeft ****/

        outNetCreator = (OutputNetworkEditor)cc.addNetwork(NetworkType.output);
        EcoCreationHelper.createOutputNetwork(outNetCreator, "moveLeft", 1, "outNetLeft", cc.creature.actionPool["moveLeft"], 0, 0,
                            ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB, cc.creature.networks);
        // user clicks save on creature creator
        cc.saveNetwork();



        /**** outNetRight ****/

        outNetCreator = (OutputNetworkEditor)cc.addNetwork(NetworkType.output);
        EcoCreationHelper.createOutputNetwork(outNetCreator, "moveRight", 1, "outNetRight", cc.creature.actionPool["moveRight"], 0, 0,
                            ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB, cc.creature.networks);
        // user clicks save on creature creator
        cc.saveNetwork();


        /**** outNetConsume grass ****/

        outNetCreator = (OutputNetworkEditor)cc.addNetwork(NetworkType.output);
        EcoCreationHelper.createOutputNetwork(outNetCreator, "eatGrass", 1, "outNetEat", cc.creature.actionPool["eatGrass"], 0, 0,
                            ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB, cc.creature.networks);
        // user clicks save on creature creator
        cc.saveNetwork();


        /**** outNetReproduce ****/

        outNetCreator = (OutputNetworkEditor)cc.addNetwork(NetworkType.output);
        EcoCreationHelper.createOutputNetwork(outNetCreator, "reproduce", 1, "outNetRepro", cc.creature.actionPool["reproduce"], 0, 0,
                            ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB, cc.creature.networks);
        // user clicks save on creature creator
        cc.saveNetwork();

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

    
}