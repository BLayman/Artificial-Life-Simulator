# Eco-Simulator

## Overview

For a video overview and demo check out: https://www.youtube.com/watch?v=JCABsCcMQ6A&feature=youtu.be

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

This project is being developed in the Unity game engine. You will need to clone the repository, and then open the project in Unity to edit it. To run a simple demo executable, Unzip the the "ExampleBuilds" folder, or build your own executable in Unity. When running the demo, press ESC to exit the program.

## Basic Use

There are currently two demos of the program as configured by the EcoDemo1 and EcoDemo2 classes. These files use a number of function calls to set the parameters for the simulation, so it's relatively easy to modify the simulation by changing the parameters. You can choose which demo to run by expanding the "Demos" game object in the Unity Hierarchy, selecting the "SimulationTestRunner" runner object, and setting the demo Index to 1 or 2 in the Unity Inspector.

You can run an experiment by setting the "SimulationTestRunner" to InActive (with the check box) and setting its sibling: "ExperimentRunnerObj" to active. Set parameters for the experiment in the "ExperimentRunner" class.

## Goals

One of the goals of this program was to make every aspect of the process, including neural network architecture, easily customizable through function calls, and eventually a graphical user interface. Another goal is to study various phenomena related to evolution and ecology such as: competition, cooperation, biodiversity, and stability.

Over time, this system is capable of evolving more adaptive neural networks. A process similar to natural selection is at work, where the
agents that survive to produce the most offspring will pass on the weights of their neural networks to a larger portion of the next generation.
Currently, asexual reproduction with mutation is the only form of reproduction, but there are plans to implement sexual reproduction with
crossover.

## Additional Information

The Additional_Resources folder contains a powerpoint describing Eco-Simulator, a comprehensive poster, and an in depth project proposal. Note that the project was originally titled "Artificial Life Agents". The Documentation folder contains various forms of documentation for the code. Keep in mind that the documentation isn't complete, but may have useful information for developers.

## License
License information for this software can be found in LICENSE.txt.

## Contact
If you have any questions or comments feel free to contact me at brettlayman7@gmail.com.

### Thank you for your interest in Eco-Simulator! Have fun experimenting with artificial life!
