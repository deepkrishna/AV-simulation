# AV-simulation

Purpose
The ideal real world testing have short coming of producing sufficient quality, representation, 
diverse and well- labelled data to properly train the AI components of self- driving vehicles.
It is impossible to predict the driving experience with unexpected behaviours like traffics delays,
weather conditions and unseen obstacles. To make the autonomous vehicle to properly navigate and react, 
there should be an alternative other than real- world testing.

Simulation Superiority : 

A game engine provides an agreeable alternative which is cheaper, 
faster and safer than conducting experiments with the real vehicles.
As also the scenarios can be repeated endless times without the  risk of uncertainty and
also mimic certain situation which can be never be tested with the real world testing as an impaired driver, 
or a sudden traffic accident.

Even if driving conditions were fairly standard, covering thousands of miles for 
testing would take huge amount of time and money.On the other hand, 
a set of high- performance computers can be used to get the job done in few hours with some 
simulating cameras and sensors. 

Using Unity Game Engine to do the simulation. 

Steps :
1. Create lanes just by using the plane object in unity 
   - set the position and transitions. 
2. Import the car package from the Unity and disable all the scrpits attached to it. 
3. Define a trajectory(path) for the cars to follow , you have to do this by the script using monobehaviour (unity editor). 
4. Now that the path is created, set the wheel colliders for the wheels of the cars. 
5. As all the required basic environment is set, Add basic functions of driving and steering to the cars by 
   creating another script (This is done in the jupiter.cs script)
6. You can now attach the scripts to the cars ,set the transition object as path and set the wheel colliders for the 
   respective cars in the inspecter panel. 

There is a video  to demonstrate the simulation. 

The  jupiter.cs script functions:



sensors:

Creation of sensors to detect the obstacles defined as colliders. Sensors are set along the transform. position as origin , 
as a straight line along z axis to project in forward direction of the car. 

Using,

    Physics.Raycast(vector3.origin, vector3 direction, out hit, float maxDistance)

. returns bool True if the ray intersects with a Collider, otherwise false.
 
It casts a ray from the origin point, in the defined position of the sensor which in our case is
along the z axis. Out hit to check if the sensors sense some obstacle so that we can know the position
and point of the contact of sensor with the obstacle. 



Controls()



All the calculations of the car is been done here. 

A trajectory is defined so that the car can be driven from initial position to the goal position. 
Calculates the current position of the car and outputs the next way-point which should be reached by the car. 

Lets consider the top view of the scenario, our car is positioned at (-5,0) which is represented by a vector, 
the next point to be reached by the car is at (5,10). So how does the car move from the origin to the next
point which is been described by the path ?

We have to calculate the way-point relative to the car, This can be done by using unity function 

     Tranform.InverseTransformPoint(Vector3);


     carTransform.InverseTransformPoint(waypoint position);


When two position vectors are given as input, it outputs a relative vector. 
If X value of the obtained relative vector is positive, the way-point is at the right direction of the car, 
if its a negative X value then it will be the left direction of the car. If it is zero then straight ahead. 
Certain angle is required for the wheels to get to the position. To do this, divide the relative the relative vector's X 
by obtained relative vector. 

     Vector3 relativeVector = transform.InverseTransformPoint(nodes[currectNode].position);

     newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;


The wheelAngle is obtained by multiplying the resultant with the maxSteerAngle.maxSteerAngle describes 
the max rotation of the wheels or how smooth the wheels can make a turn. 

Steer angle in case of obstacle. 



If the physics ray cast has returned true then avoidMultiplier  which is the new steer angle to 
avoid the obstacle is calculated. If the obstacle is towards the right sensor, then the car would turn left.
If the obstacle is towards the left sensor, then the car would turn right.The avoidmultiplier will be in negative 
if the obstacle detected in right and avoidmultiplier will be positive if the obstacle detected in left.

     wheelF L.steerAngle =maxSteerAngle * avoidmultiplier.   


If there is no obstacle found then the newSteer would be same as before. 

The wheel colliders for all the wheels of the car is set to turn the wheel in right direction 
depending on where the next way-point is so the car is already pointing to next node. 
In this step, it needs some power to through the path. So a maxMotorTorque is defined to define power to the car. 

Speed 




The distance between the nodes of the path is calculated, if the next node is close by then go towards
the next node.This is done by increment  the current node and and accelerating till we reach the last node.

Acceleration is given based on the current speed, if the current speed is less 
than the maximum speed then acceleration is increased till it reaches the maximum speed and the last node of the path. 
 The current speed is calculated by

      currentSpeed = 2∗MathF.PI∗wheelFL.radius∗wheelFL.rpm∗60/1000; 


Actuation()



All the calculations for the controls are used to apply the steer and drive to 
the car to drive the car avoiding obstacles and reaching destination. 


Some of the concepts are been inspired by CarAI series by Eyeimaginary 




