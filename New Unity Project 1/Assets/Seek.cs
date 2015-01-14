using UnityEngine;
using System.Collections;

public class Seek : MonoBehaviour {
    public GameObject targetUnit;
    public float accelerate = .1f;
    public float maxVel = 10;
	public string AIState = "Seek";
	public float arriveDis = 10;

	Ray currVel;
	Ray desVel;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine (transform.position, transform.position + rigidbody.velocity, Color.red);
        switch (AIState) 
		{
		case "Seek":
			SeekAI();
			break;
		case "Flee":
			FleeAI();
			break;
		case "Arrive":
			ArriveAI ();
			break;
		default:
			Debug.Log (AIState + " Is not a recognized AIState");
			break;
		}
        
	}
	void SeekAI()
	{
		Vector3 desiredVel = Vector3.Normalize(targetUnit.transform.position - transform.position);
		Debug.DrawLine (transform.position, transform.position + desiredVel*maxVel, Color.green);
		if (Vector3.Distance(transform.position, targetUnit.transform.position) > rigidbody.velocity.magnitude)
		{

				rigidbody.AddForce(desiredVel * accelerate);
			float mag = rigidbody.velocity.magnitude;
			if(mag > maxVel)
			{
				rigidbody.velocity *= maxVel/mag;
			}
			
		}
	}
	void FleeAI()
	{
		Vector3 desiredVel = Vector3.Normalize(targetUnit.transform.position - transform.position);
		Debug.DrawLine (transform.position, transform.position + desiredVel, Color.green);
		if (Vector3.Distance(transform.position, targetUnit.transform.position) > rigidbody.velocity.magnitude)
		{
			if (rigidbody.velocity.magnitude < maxVel)
			{
				rigidbody.AddForce(desiredVel * -accelerate);
			}
		}
	}
	void ArriveAI()
	{
		
		Vector3 desiredVel = Vector3.Normalize(targetUnit.transform.position - transform.position);

		if (Vector3.Distance (transform.position, targetUnit.transform.position) > arriveDis) 
		{
			SeekAI ();
		} else
		{
			if(Vector3.Angle(desiredVel,rigidbody.velocity) < 5)
			   {
				rigidbody.AddForce(desiredVel * -accelerate);
				}
		}
	}
}
