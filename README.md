# Eco-Simulator

## Overview

For a video overview and demo check out: https://www.youtube.com/watch?v=tDqDRze9cmk

For a poster overview or in depth report please see: https://github.com/BLayman/Artificial-Life-Simulator/tree/master/Additional_Resources

Eco-Simulator is a program for running a multi-agent system that is analogous to an ecosystem. The system is designed to be
open-ended, with various parameters that the user can set. The system plays out on a 2D grid of land spaces and is centered around two main
features: neural networks, and resources.

Each agent has a collection of neural networks for determining that agent's behavior. An agent's neural network maps input information
such as: perception of the environment, sensing internal state, and communication with other agents to output actions such as: consumption
of a resource, movement, reproduction, and interaction with other agents.

Each agent also stores a number of resources that can be acquired from land spaces. These resources may be essential for the agent's
survival, and certain resources may be necessary for performing certain actions.

During one step of the simulation, each agent performs a turn. A turn involves running inputs through the agent's neural networks,
adding the resulting actions to a queue, and then performing as many actions as they can in the time allotted for their turn.

The above description is an oversimplification of the process, but should get the general idea across.

## Installation

To run the program and one of the three demos I've created: simply download/clone the repo and then extract the zip file titled, "EcoSimulatorBuild". Within the extracted folder there is an executable titled, "EcoNet". Run this to start the program. Press the ESC key to exit the program

To edit the program, you'll need the Unity game engine, which is free. After installing the engine, you can open this project and edit the code with MonoDevelop or VisualStudio. In the future I would like to create a standalone application, since the simulation code is easily decoupled, but for now it is being run with Unity.

## Basic Use and Customization

There are currently three demos of the program as configured by the EcoDemo1, EcoDemo2, and EcoDemo3 classes. You can choose a demo from the main menu when you start the program. It may take some time to load. These files use a number of function calls to set the parameters for the simulation, so it's relatively easy to modify the simulation by changing the parameters.

You can run an experiment by setting the "SimulationTestRunner" to InActive (with the check box) in the Unity Editor Hierarchy and setting its sibling: "ExperimentRunnerObj" to active. Set parameters for the experiment in the "ExperimentRunner" class.

## Description of Demos

### Demo 1

This is a relatively simple example to demonstrate how a population can adapt to an ecosystem through evolution by natural selection. K-Means clustering is used to show which creatures are genetically similar by assigning them the same color (1 color per cluster). The population starts with a certain amount of genetic variation, and mutation can add further variation to the population (although the mutation rate decreases over time to keep variation from getting out of control). As you can see, many of the clusters of creatures die out, while others are better adapted and survive. Occasionally a cluster will switch colors if it evolves enough before the next clustering step (which occurs every 20 steps). The number of creatures rarely reaches the population cap of 2000, because the "vitamin" resource is required for reproduction, and it has a slow regeneration rate. "grass" can effect cow health when it gets low, but "vitamin" does not.

### Demo 2

This demo was created to show the intricacies of resource flow and to explore cooperation between creatures. It doesn't use clustering, but instead has two species with different neural networks and actions. Each species is dependent on the other to produce a resource that they need. Creatures are able to sense the phenotype and position of their neighbors, which can then guide their behavior. This system uses artificial population caps.

### Demo 3

This demo uses a less abstract example of a system with plants, herbivores, and predators. The plants are green, herbivores blue, and predators red. Herbivores can steal resources from plants (potentially causing them to die) and predators can steal resources from herbivores. Creatures are able to sense the phenotype and position of their neighbors, which can then guide their behavior. This seems to cause herbivores to move less when they are next to plants (they can consume neighboring plants). This system uses artificial population caps.

## Goals

One of the goals of this program was to make every aspect of the process, including neural network architecture, easily customizable through function calls, and eventually a graphical user interface. Another goal is to study various phenomena related to evolution and ecology such as: competition, cooperation, biodiversity, and stability.

Over time, this system is capable of evolving more adaptive neural networks. A process similar to natural selection is at work, where the
agents that survive to produce the most offspring will pass on the weights of their neural networks to a larger portion of the next generation.
Currently, asexual reproduction with mutation is the only form of reproduction, but there are plans to implement sexual reproduction with
crossover.

## Additional Information

The Additional_Resources folder contains a comprehensive poster, and an in depth project report. The Documentation folder contains various forms of documentation for the code. Keep in mind that the documentation isn't complete, but may have useful information for developers. If you are simply planning to create your own system, I would recommend using the documentation in the EcoCreationHelper Documentation folder. As you can see in the EcoDemo classes, much of the ecosystem creation is done through the EcoCreationHelper class. To view the documentation, first download the folder, and then open the index.html file inside the folder with your web browser.

## Future Directions

As you may notice, the simulation can run fairly slowly with a large number of creatures. This is because each creature's neural network is being run in sequence rather than in parallel. Running these neural nets in parallel on the graphics card would dramatically improve performance.

It would also be useful to create a system for saving and loading populations of creatures, so that they can be reused after evolving, and be placed into new systems.

## License
License information for this software can be found in LICENSE.txt.

## Contact
If you have any questions or comments feel free to contact me at brettlayman7@gmail.com.

### Thank you for your interest in Eco-Simulator! Have fun experimenting with artificial life!
