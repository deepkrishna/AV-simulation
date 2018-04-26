# AV-simulation

Case study of the lane change car manvoure. 

This case study is done in Unity Game Engine. 


 In Unity, designed and implemented a lane change manoeuvre. 
    1. Set up the environment (create a lane/plane)
    2. Import the car package from the Unity (disable all the scripts attached to it) 
    3. Create wheel colliders using Unity packages .
    4. Define a path (trajectory with nodes, add it to the script and add it as a transform element).
    5. Create a script, add your transformation function in the start and in the fixed update (step function in Unity).   
    6. Call the functions of sensors, controllers and actuation in Unity.
    7. Go to the inspector panel and set  the wheel colliders for the script and also the path.
    8. Parameters/scenarios/values can be changed either from the panel or from the script.
    9. If you want to create obstacles, call nav mesh obstacles to define it as an obstacle (rigid body with mass).
    10. Run the experiment by attaching the path_AV and the jupiter script to every car in the lane. 
    11. Set the center of mass for all the cars. 
