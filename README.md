# Artificial Life Simulator

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

The above description is an oversimplification of the process, but should get the general idea across. One of the goals of this program was to
make every aspect of the process, including neural network architecture, easily customizable through function calls, and eventually a graphical
user interface. Please see the EcoManager class as an example of how parameters can be set.

Over time, this system is capable of evolving more adaptive neural networks. A process similar to natural selection is at work, where the
agents that survive to produce the most offspring will pass on the weights of their neural networks to a larger portion of the next generation.
Currently, asexual reproduction with mutation is the only form of reproduction, but there are plans to implement sexual reproduction with
crossover. 

License information for this software can be found in the LICENSE.txt file.

If you have any questions or comments feel free to contact me at brettlayman7@gmail.com. 

Thank you for your interest in Artificial Life simulator! Have fun experimenting with artificial life!
