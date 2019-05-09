using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EcoDemo3 : DemoInterface
{
    /// <summary>
    /// Stores state of ecosystem.
    /// </summary>
    Ecosystem ecosystem;
    EcosystemEditor ecoCreator;
    bool called = false;

    public void makeEco()
    {

        createEcosystem(100);

        // add cow species
        addPlant("grass", ColorChoice.green, 1f, false, .9f, .01f);

        // populate with low standard deviation from founder creature
        populateSpecies("grass", 2f, 1500, 4000);

        // add cow species
        addHerbivore("cow", ColorChoice.blue, 2f, true, .9f, .01f, "grass");

        // populate with low standard deviation from founder creature
        populateSpecies("cow", 2f, 100, 700);

        addCarnivore("wolf", ColorChoice.red, 2f, true, .9f, .01f, "cow");

        // populate with low standard deviation from founder creature
        populateSpecies("wolf", 2f, 100, 700);


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
        EcoCreationHelper.setEcoParams(ecoCreator, 10, 32, 50, true, false);

        // create grass
        EcoCreationHelper.addResource(ecoCreator, "energy", 200, 250, 1, .5f, 8f);
        ecoCreator.saveResource();


        EcoCreationHelper.addResource(ecoCreator, "vitamin", 1f, 20, 1, .1f, 1f);
        ecoCreator.saveResource();

        ecoCreator.saveResourceOptions(); // adds all resources to ecosystem resources


        // generate map
        ecoCreator.createMap();
        // TODO: account for asymetric maps
        ecoCreator.mapEditor.generateMap(mapWidth, mapWidth);
        // islands: 300, .8, 50, 30
        // barriers: 300, .8, 100, 30 (creature pop 2000 for barely survive)
        //ecoCreator.mapEditor.addClusteredResource("grass", 1f, 100, 30);
        ecoCreator.mapEditor.addLERPXResource("energy", 1f);
        ecoCreator.mapEditor.addUniformResource("vitamin", .5f);
        ecoCreator.saveEditedMap(); // saves to tentative map
        ecoCreator.saveMap(); // saves to ecosystem map

    }




    public void addPlant(string name, ColorChoice color, float mutationDeviation, bool useHiddenNodes, float mutationDeviationFraction,
        float lowestMutationDeviation)
    {
        // when user clicks to start species creation process:
        CreatureEditor cc = ecoCreator.addCreature();

        EcoCreationHelper.setCreatureStats(cc, name, 1, 100, 1000, 700, 3, 10, mutationDeviation, color, false,
                        mutationDeviationFraction, lowestMutationDeviation, MutationDeviationCoefficientType.exponentialDecay);

        // add resource for the creature to store
        ResourceEditor resourceCreator = cc.addResource();

        List<string> ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);
        EcoCreationHelper.addCreatureResource(resourceCreator, "energy", 1000, 800, 1, 900, 10, 100, 1);
        cc.saveResource();


        resourceCreator = cc.addResource();
        ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);
        EcoCreationHelper.addCreatureResource(resourceCreator, "vitamin", 100, 20, 0, 90, 0, 20, 0);
        cc.saveResource();


        // for future reference
        List<string> creatureResources = new List<string>(cc.creature.storedResources.Keys);


        /* MUST GENERATE ACTIONS AND ADD THEM TO CREATURE'S ACTION POOL BEFORE CREATING OUTPUT NODES FOR THOSE ACTIONS */

        // add default abilities for consuming resources
        cc.addDefaultResourceAbilities();

        List<string> predatorList = new List<string>() { "cow" };
        cc.addDefenseAbilities(predatorList);

        cc.saveAbilities();


        // create action for consuming primary resource
        ActionEditor ae = cc.addAction();
        ae.setCreator(ActionCreatorType.consumeCreator);
        ConsumeFromLandEditor cle = (ConsumeFromLandEditor)ae.getActionCreator();
        // define resource costs
        Dictionary<string, float> resourceCosts = new Dictionary<string, float>(); // no resource cost
        // set parameters
        EcoCreationHelper.setBasicActionParams(cle, "photosynth", 1, 10, resourceCosts);
        EcoCreationHelper.setConsumeParams(cle, 0, "energy");
        cc.saveAction();


        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.consumeCreator);
        cle = (ConsumeFromLandEditor)ae.getActionCreator();
        // define resource costs
        resourceCosts = new Dictionary<string, float>()
        {
            {"energy", 1},
        };
        // set parameters
        EcoCreationHelper.setBasicActionParams(cle, "eatVitamin", 1, 10, resourceCosts);
        EcoCreationHelper.setConsumeParams(cle, 0, "vitamin");
        cc.saveAction();


        // create action for reproduction
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.reproduceCreator);
        ReproActionEditor rae = (ReproActionEditor)ae.getActionCreator();
        // high resource costs for reproduction
        resourceCosts = new Dictionary<string, float>()
        {
            {"energy", 30},
            {"vitamin", 10}
        };
        EcoCreationHelper.setBasicActionParams(rae, "reproduce", 1, 10, resourceCosts);
        // no special params to set for reproduction yet
        cc.saveAction();


        // user opens networks creator for that creature


        // user adds a network
        /*
        NetworkEditor netCreator = cc.addNetwork(NetworkType.regular);
        List<string> resourcesToSense = creatureResources; // sense resources creature can store
        List<string> outputActions = new List<string>()
        {
            "reproduce",
            "photosynth",
            "eatVitamin",
        };

        EcoCreationHelper.makeSensoryInputNetwork(netCreator, 0, "SensoryNet", resourcesToSense, outputActions, 0, 0,
                                ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);
        

        // user clicks save on network creator
        cc.saveNetwork();
        */

        // sense internal levels of resources
        NetworkEditor InternalNetCreator = cc.addNetwork(NetworkType.regular);
        // sense all creature resources again, this time internally
        List<string> resourcesToSense = creatureResources;
        // use all output actions again
        List<string> outputActions = new List<string>()
        {
            "reproduce",
            "photosynth",
            "eatVitamin",
        };

        EcoCreationHelper.makeInternalInputNetwork(InternalNetCreator, 0, "internalNet", resourcesToSense, outputActions, 0, 0,
                                ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);

        // user clicks save on network creator
        cc.saveNetwork();




        Dictionary<string, string> actionNameByNetName = new Dictionary<string, string>()
        {

            {"outNetPhoto", "photosynth" },
            {"outNetRepro", "reproduce"},
            {"outNetEatVit", "eatVitamin" }

        };

        EcoCreationHelper.createOutputNetworks(cc, 1, actionNameByNetName, 0, 0, ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);


        // adds creature to list of founders
        ecoCreator.addToFounders();
        // saves founders to ecosystem species list
        ecoCreator.saveFoundersToSpecies();
    }



    /*       **************************************************************************************************************************       */



    public void addHerbivore(string name, ColorChoice color, float mutationDeviation, bool useHiddenNodes, float mutationDeviationFraction,
        float lowestMutationDeviation, string prey)
    {
        // when user clicks to start species creation process:
        CreatureEditor cc = ecoCreator.addCreature();

        EcoCreationHelper.setCreatureStats(cc, name, 2, 100, 1000, 700, 3, 10, mutationDeviation, color, true,
                        mutationDeviationFraction, lowestMutationDeviation, MutationDeviationCoefficientType.exponentialDecay);


        // add resource for the creature to store
        ResourceEditor resourceCreator = cc.addResource();

        List<string> ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);
        EcoCreationHelper.addCreatureResource(resourceCreator, "energy", 1000, 800, 1, 900, 5, 200, 1f);
        cc.saveResource();

        /*
        resourceCreator = cc.addResource();
        ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);
        EcoCreationHelper.addCreatureResource(resourceCreator, "vitamin", 100, 10, 0, 90, 0, 20, 0);
        cc.saveResource();
        */

        // for future reference
        List<string> creatureResources = new List<string>(cc.creature.storedResources.Keys);


        // TODO create default actions for creature action pool, and example user made action 
        // (should use add an action creator to creature creator)
        cc.generateMovementActions("energy", .5f);

        /* MUST GENERATE ACTIONS AND ADD THEM TO CREATURE'S ACTION POOL BEFORE CREATING OUTPUT NODES FOR THOSE ACTIONS */


        // add default abilities for consuming resources
        cc.addDefaultResourceAbilities();
        // if predator

        List<string> preyList = new List<string>() { "grass" };
        cc.addAttackAbilities(preyList);

        List<string> predatorList = new List<string>() { "wolf" };
        cc.addDefenseAbilities(predatorList);

        cc.saveAbilities();


        // create action for consuming primary resource


        /*
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.consumeCreator);
        cle = (ConsumeFromLandEditor)ae.getActionCreator();
        // define resource costs
        resourceCosts = new Dictionary<string, float>()
        {
            {"energy", 1},
        };
        // set parameters
        EcoCreationHelper.setBasicActionParams(cle, "eatVitamin", 1, 10, resourceCosts);
        EcoCreationHelper.setConsumeParams(cle, 0, "vitamin");
        cc.saveAction();
        */

        //createAttackAction

        ActionEditor ae = cc.addAction();
        ae.setCreator(ActionCreatorType.attackEditor);
        AttackEditor attackEdit = (AttackEditor)ae.getActionCreator();
        Dictionary<string, float> resourceCosts = new Dictionary<string, float> { { "energy", .2f } };
        EcoCreationHelper.setBasicActionParams(attackEdit, "eatGrass", 1, 10, resourceCosts);
        EcoCreationHelper.setAttackActionParams(attackEdit, "grass", 0, .7f);
        cc.saveAction();



        // create action for reproduction
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.reproduceCreator);
        ReproActionEditor rae = (ReproActionEditor)ae.getActionCreator();
        // high resource costs for reproduction
        resourceCosts = new Dictionary<string, float>()
        {
            {"energy", 200},
            //{"vitamin", 10}
        };
        EcoCreationHelper.setBasicActionParams(rae, "reproduce", 1, 10, resourceCosts);
        // no special params to set for reproduction yet
        cc.saveAction();


        // sense internal levels of resources
        NetworkEditor InternalNetCreator = cc.addNetwork(NetworkType.regular);
        // sense all creature resources again, this time internally
        List<string> resourcesToSense = creatureResources;
        // use all output actions again
        List<string> outputActions = new List<string>()
        {
            "reproduce",
            "eatGrass",
            //"eatVitamin",
            "moveUp",
            "moveDown",
            "moveLeft",
            "moveRight",

        };

        EcoCreationHelper.makeInternalInputNetwork(InternalNetCreator, 0, "internalNet", resourcesToSense, outputActions, 0, 0,
                                ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);

        // user clicks save on network creator
        cc.saveNetwork();



        PhenotypeNetworkEditor phenoNetCreator = (PhenotypeNetworkEditor)cc.addNetwork(NetworkType.phenotype);

        List<string> phenoOutputActions = new List<string>()
        {
            "reproduce",
            "eatGrass",
            //"eatVitamin",
            "moveUp",
            "moveDown",
            "moveLeft",
            "moveRight",

        };

        EcoCreationHelper.createPhenotypeNet(phenoNetCreator, 0, "phenotypeNet", 0, 0, phenoOutputActions,
                           ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);

        // Note: don't call saveNetwork(), call savePhenotypeNetwork()
        cc.savePhenotypeNetwork();




        Dictionary<string, string> actionNameByNetName = new Dictionary<string, string>()
        {
            {"outNetUp", "moveUp" },
            {"outNetDown", "moveDown" },
            {"outNetLeft", "moveLeft" },
            {"outNetRight", "moveRight" },
            {"outNetRepro", "reproduce"},
            //{"outNetEatVit", "eatVitamin" },
            { "outNetEatGrass", "eatGrass" }


        };


        EcoCreationHelper.createOutputNetworks(cc, 1, actionNameByNetName, 0, 0, ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);


        // adds creature to list of founders
        ecoCreator.addToFounders();
        // saves founders to ecosystem species list
        ecoCreator.saveFoundersToSpecies();
    }



    /*       **************************************************************************************************************************       */




    public void addCarnivore(string name, ColorChoice color, float mutationDeviation, bool useHiddenNodes, float mutationDeviationFraction,
        float lowestMutationDeviation, string prey)
    {
        // when user clicks to start species creation process:
        CreatureEditor cc = ecoCreator.addCreature();

        EcoCreationHelper.setCreatureStats(cc, name, 3, 100, 1000, 700, 3, 10, mutationDeviation, color, true,
                        mutationDeviationFraction, lowestMutationDeviation, MutationDeviationCoefficientType.exponentialDecay);


        // add resource for the creature to store
        ResourceEditor resourceCreator = cc.addResource();

        List<string> ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);
        EcoCreationHelper.addCreatureResource(resourceCreator, "energy", 1000, 800, 1, 900, 5, 200, 1);
        cc.saveResource();

        /*
        resourceCreator = cc.addResource();
        ecosystemResources = new List<string>(ecosystem.resourceOptions.Keys);
        EcoCreationHelper.addCreatureResource(resourceCreator, "vitamin", 100, 10, 0, 90, 0, 20, 0);
        cc.saveResource();
        */

        // for future reference
        List<string> creatureResources = new List<string>(cc.creature.storedResources.Keys);


        // TODO create default actions for creature action pool, and example user made action 
        // (should use add an action creator to creature creator)
        cc.generateMovementActions("energy", .5f);

        /* MUST GENERATE ACTIONS AND ADD THEM TO CREATURE'S ACTION POOL BEFORE CREATING OUTPUT NODES FOR THOSE ACTIONS */


        // add default abilities for consuming resources
        cc.addDefaultResourceAbilities();
        // if predator

        List<string> preyList = new List<string>() { "cow" };
        cc.addAttackAbilities(preyList);
        cc.saveAbilities();


        // create action for consuming primary resource


        /*
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.consumeCreator);
        cle = (ConsumeFromLandEditor)ae.getActionCreator();
        // define resource costs
        resourceCosts = new Dictionary<string, float>()
        {
            {"energy", 1},
        };
        // set parameters
        EcoCreationHelper.setBasicActionParams(cle, "eatVitamin", 1, 10, resourceCosts);
        EcoCreationHelper.setConsumeParams(cle, 0, "vitamin");
        cc.saveAction();
        */

        //createAttackAction

        ActionEditor ae = cc.addAction();
        ae.setCreator(ActionCreatorType.attackEditor);
        AttackEditor attackEdit = (AttackEditor)ae.getActionCreator();
        Dictionary<string, float> resourceCosts = new Dictionary<string, float> { { "energy", .2f } };
        EcoCreationHelper.setBasicActionParams(attackEdit, "eatCow", 1, 10, resourceCosts);
        EcoCreationHelper.setAttackActionParams(attackEdit, "cow", 1, .9f);
        cc.saveAction();



        // create action for reproduction
        ae = cc.addAction();
        ae.setCreator(ActionCreatorType.reproduceCreator);
        ReproActionEditor rae = (ReproActionEditor)ae.getActionCreator();
        // high resource costs for reproduction
        resourceCosts = new Dictionary<string, float>()
        {
            {"energy", 200},
            //{"vitamin", 10}
        };
        EcoCreationHelper.setBasicActionParams(rae, "reproduce", 1, 10, resourceCosts);
        // no special params to set for reproduction yet
        cc.saveAction();


        // sense internal levels of resources
        NetworkEditor InternalNetCreator = cc.addNetwork(NetworkType.regular);
        // sense all creature resources again, this time internally
        List<string> resourcesToSense = creatureResources;
        // use all output actions again
        List<string> outputActions = new List<string>()
        {
            "reproduce",
            "eatCow",
            //"eatVitamin",
            "moveUp",
            "moveDown",
            "moveLeft",
            "moveRight",

        };

        EcoCreationHelper.makeInternalInputNetwork(InternalNetCreator, 0, "internalNet", resourcesToSense, outputActions, 0, 0,
                                ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);

        // user clicks save on network creator
        cc.saveNetwork();



        PhenotypeNetworkEditor phenoNetCreator = (PhenotypeNetworkEditor)cc.addNetwork(NetworkType.phenotype);

        List<string> phenoOutputActions = new List<string>()
        {
            "reproduce",
            "eatCow",
            //"eatVitamin",
            "moveUp",
            "moveDown",
            "moveLeft",
            "moveRight",

        };

        EcoCreationHelper.createPhenotypeNet(phenoNetCreator, 0, "phenotypeNet", 0, 0, phenoOutputActions,
                           ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);

        // Note: don't call saveNetwork(), call savePhenotypeNetwork()
        cc.savePhenotypeNetwork();




        Dictionary<string, string> actionNameByNetName = new Dictionary<string, string>()
        {
            {"outNetUp", "moveUp" },
            {"outNetDown", "moveDown" },
            {"outNetLeft", "moveLeft" },
            {"outNetRight", "moveRight" },
            {"outNetRepro", "reproduce"},
            //{"outNetEatVit", "eatVitamin" },
            { "outNetEatCow", "eatCow" }


        };


        EcoCreationHelper.createOutputNetworks(cc, 1, actionNameByNetName, 0, 0, ActivationBehaviorTypes.LogisticAB, ActivationBehaviorTypes.LogisticAB);


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