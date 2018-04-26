using System.Collections;
using System.Collections.Generic;
using UnityEngine;










public class jupiter : MonoBehaviour {

	//path variables
	public Transform path;
	private List<Transform> nodes;
	private int currectNode = 0;


	//wheel colliders variables 
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;

	//speed and steering variables
	public float maxMotorTorque = 80f;
	public float maxSteerAngle = 45f;
	public float currentSpeed;
	public float maxSpeed = 100f;
	public Vector3 centerOfMass;
	public float newSteer;
	public float minimum=0;
	public float power;
	//public float turnSpeed = 5f;
	//private float targetSteerAngle = 0;
	//sensor variables
	public float sensorLength = 3f;
	public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 0.5f);
	public float frontSideSensorPosition = 0.2f;
	public float frontSensorAngle = 30f;
	 public bool s1=false;
	public bool s2=false;
	public bool s3=false;
	public bool s4 =false;
	public bool s5=false; 
	public float outAngle;
	public bool avoiding;
	RaycastHit hit;
	float avoidMultiplier = 0;
	//public float movement;



	public float totalSteer;


	// Use this for initialization


	void Start () 
	{
		 GetComponent<Rigidbody>().centerOfMass = centerOfMass; // set the center of mass. 
		 Transform[] pathTransforms = path.GetComponentsInChildren<Transform>(); // path tarnsform 
		 nodes = new List<Transform>();

		   for (int i = 1; i < pathTransforms.Length; i++) 
		     {
			   if (pathTransforms[i] != path.transform)
			    {
				   nodes.Add(pathTransforms[i]);
			    }
		     }
	}




	void FixedUpdate () 
	{
		
		sensors();
		controls();
		actuation();
		//LerpToSteerAngle();
	}


	private void sensors()
	{

	
		Vector3 sensorStartPos = transform.position;    //origin position of the sensor
		sensorStartPos += transform.forward * frontSensorPosition.z; //setting the sensor to the straight line. 
		sensorStartPos += transform.up * frontSensorPosition.y; // setting it along the y axis to get some certain height. 
		s1=s2=s3=s4=s5= false;


		//front right sensor
		sensorStartPos += transform.right * frontSideSensorPosition;
		if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) 
		{

			Debug.DrawLine(sensorStartPos, hit.point); // get the hit information 
			//Debug.Log("front right sensor");
	

			s1 = true;
		}



		//front right angle sensor
		else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) 
		{
			Debug.DrawLine(sensorStartPos, hit.point);
			//Debug.Log("front right angle sensor");
		
			s2 = true;
		} 


		//front left sensor
		sensorStartPos -= transform.right * frontSideSensorPosition * 2;
		if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) 
		{
			Debug.DrawLine(sensorStartPos, hit.point);

			//Debug.Log("front left sensor");
		
			s3 =true;


		}

		//front left angle sensor
		else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) 
		{

			Debug.DrawLine(sensorStartPos, hit.point);
		//	Debug.Log("front left angle sensor");
		
			s4 = true;
			}
	
		//front center sensor
		if (avoidMultiplier == 0) 
		{
			if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) 
			{

				Debug.DrawLine(sensorStartPos, hit.point);
				//Debug.Log("front center sensor");
			
				s5 = true;


			}

		}

	//	return result;
	 

 }


	void controls()
	{
		float thedistance;
		//checkwaypoint calculation( next node ) 
		if (Vector3.Distance (transform.position, nodes [currectNode].position) < 1f) {
			if (currectNode == nodes.Count - 1) {
				currectNode = nodes.Count - 1;
				wheelFL.motorTorque = 0;
				wheelFR.motorTorque = 0;
			
			} else {

				currectNode++;
			}
		}
	

			
		avoiding = false;

		//steer calculation 
		Vector3 relativeVector = transform.InverseTransformPoint (nodes [currectNode].position);
		newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;


		//drive calculation. 
		currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
		if (currentSpeed < maxSpeed ) {
			power = maxMotorTorque;

		} else {
			wheelFL.motorTorque = 0;
			wheelFR.motorTorque = 0;
		}

		//sensors calculations.
		//front right sensor
		if (s1 == true) {
			avoiding = true;
			thedistance = hit.distance;
			print (thedistance);
			Debug.Log (hit.point);
			avoidMultiplier -= 1f;

		}

		//front right angle  sensor
		if (s2 == true) {
			avoiding = true;
			thedistance = hit.distance;
			print (thedistance);
			Debug.Log (hit.point);
			avoidMultiplier -= 0.8f;


		} 


		//front left sensor
		if (s3 == true) {
			avoiding = true;
			thedistance = hit.distance;
			print (thedistance);
			Debug.Log (hit.point);

			avoidMultiplier += 1f;

		}


		//front left angle sensor
		if (s4 == true) {
			avoiding = true;
			thedistance = hit.distance;
			print (thedistance);
			Debug.Log (hit.point);
			avoidMultiplier += 0.8f;
		}

		//front center sensor
		if (s5 == true)
			avoiding = true;
		thedistance = hit.distance;
		print (thedistance);
		Debug.Log (hit.point);
		if (hit.normal.x < 0) {
			avoidMultiplier = -1f;
		} else {
			avoidMultiplier = 1f;

		}


		if (avoiding == true) {
			//	print (avoidMultiplier);

			outAngle = maxSteerAngle * avoidMultiplier; // new steer calculated according to the obstacle found. 
	
		    
			totalSteer = outAngle;
		} else {
			totalSteer = newSteer;
		}
	
		/*if (currectNode == nodes.Count - 1)
			{
			wheelFL.motorTorque = minimum;;
			wheelFR.motorTorque = minimum;
            }
	    } */

	
}
	void actuation( )
	{


		//Apply steer

		wheelFL.steerAngle = totalSteer;
		wheelFR.steerAngle = totalSteer; 

		//apply motor torque(power) 
		wheelFL.motorTorque = power;
		wheelFR.motorTorque = power;

	
	}
//	private void LerpToSteerAngle() {
		//wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
		//wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
	//}
}

