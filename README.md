# Artificial Life Simulator

## Overview

Artificial Life Simulator is a program for running a multi-agent system that is analogous to an ecosystem. The system is designed to be
open-ended, with various parameters that the user can set. The system plays out on a 2D grid of land spaces and is centered around two main
features: neural networks, and resources.

Each agent has a collection of neural networks for determining that agent's behavior. An agent's neural network maps input information
such as: perception of the environment, sensing internal state, and communication with other agents; to output actions such as: consumption 
of a resource, movement, reproduction, and interaction with other agents.

Each agent also stores a number of resources that can be aquired from land spaces. These resources may be essential for the agents's
survival, and certain resources may be necessary for performing certain actions.

During one step of the simulation, each agent performs a turn. A turn involves running inputs through the agent's neural networks,
adding the resulting actions to a queue, and then performing as many actions as they can in the time alotted for their turn.

The above description is an oversimplification of the process, but should get the general idea across.

## Installation

This project is being developed in the Unity game engine. You will need to clone the repository, and then open the project in Unity to edit it. To run a simple demo executable, Unzip the the "ExampleBuilds" folder, or build your own executable in Unity. When running the demo, press ESC to exit the program.

## Current Settings and Basic Use

The program is currently set to create two species: one colored blue and the other colored green.The weights of each specie's neural
networks is randomly generated, so you will see different behavior each time you run the simulation. One key difference between the two
species is the amount of random variation added to the initial weights when creating a population. The blue population is created with a
small amount of variability in each individual's weights, whereas green is created with a large amount of variability between individuals.
The result is a number of sub-populations of green with distict behaviors (could be considered different species even). I'd suggest playing
around with these parameters for population variation in MakeEco function of EcoManager. I'd also recommend adjusting the mutationDeviation
of the species to see what happens when large amounts of variation are added to the weights upon reproduction. The overall result seems
to be that more variation leads to increased chances of survival. 

The populations are currently capped at 500 to keep the program running relatively quickly. Feel free to increase this cap or the map size. I have tested up to a population size of 10,000 creatures total on a 1000 X 1000 map (1 million land squares). Note that it can take some time just to render a large map. 

You will notice a line moving across the map. This is resources being renewed, not a bug. Feel free to change the number of steps processed between each rendering of hte system by entering a number in the upper left and pressing the "Apply" button. The slider changes the rendering speed on smaller maps where the simulation can run faster. 

## Goals

One of the goals of this program was to make every aspect of the process, including neural network architecture, easily customizable through function calls, and eventually a graphical user interface. Please see the EcoManager class as an example of how parameters can be set.

Over time, this system is capable of evolving more adaptive neural networks. A process similar to natural selection is at work, where the
agents that survive to produce the most offspring will pass on the weights of their neural networks to a larger portion of the next generation.
Currently, asexual reproduction with mutation is the only form of reproduction, but there are plans to implement sexual reproduction with
crossover. 

## Additional Information

The Additional_Resources folder contains a powerpoint describing Artificial Life Simulator, and an in depth project proposal. Note that the project was originally called "Artificial Life Agents". The Documentation folder contains various forms of documentaion for the code. Keep in mind that the documentation isn't complete, but may have useful information for developers.

## License
License information for this software can be found in LICENSE.txt.

## Contact
If you have any questions or comments feel free to contact me at brettlayman7@gmail.com. 

### Thank you for your interest in Artificial Life simulator! Have fun experimenting with artificial life!
