![Image of WaterVR Logo](https://github.com/benknight135/WaterVR/blob/master/Logo.png)
**v0.0.2**

Demonstration of different water simulation techiques for use in Unity VR games and experiences.
Three scenes demonstrate three different approches to fluid simulation.

## Method 1: Animation Cycle
Each frame of the animation is exported as an 'obj' file. This is then imported into Unity and cycled using a script.
See '/Unity/Assets/Animation/AnimateObjs.cs'.

## Method 2: Collision Spheres
Spheres with collision are used to represent a fluid. Large amounts of balls are very high load so not really feasiable at scale. 

## Method 3: Animation events
Animation events triggered with interactions. There are two animations, one whilst falling and another when the liquid hits the ground. This was created using Unity Meta Balls Liquids asset. 

## Player Control
The player can swap between the scenes using the 'A' and 'B' keys. The scene can be reset using the middle click of the right joystick. 

## Examples
![Image of in game fluid](https://github.com/benknight135/WaterVR/blob/master/Sample1.PNG)
![Image of in game fluid](https://github.com/benknight135/WaterVR/blob/master/Sample.PNG)

## Credits
Fluid simulation was done using FLIP Fluids (https://github.com/rlguy/Blender-FLIP-Fluids)

Method 3 uses Unity Meta Balls Liquids asset (https://github.com/Nesh108/Unity_MetaBalls_Liquids.git)
