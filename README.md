# cow-sim
This repository contains the code for an agent-based model of cows grazing in a field. The goal of the ABM is predict the sustainability of cow grazing patterns across a working ranch. Three versions of the AMB are included: a basic python example, a scaled down example using the Godot gaming engine framework, and a fully-featured ABM in Godot.

This python model is a simple example of using python to create an ABM of cows grazing in a field. At each model time step, each cow searches for the tallest grass near it, moves to that patch of grass, and eats. Eaten grass also regrows at each time step. Setup is done using an input .json file. 

The intermediate Godot ABM shows how this model can be created and customized using Godot.

The advanced Godot ABM shows a full-scale simulation of cows grazing.
