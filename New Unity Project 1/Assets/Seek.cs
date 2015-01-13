using UnityEngine;
using System.Collections;

public class Seek : MonoBehaviour {
    public GameObject targetUnit;
    public float accelerate = .1f;
    public float maxVel = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 desiredVel = Vector3.Normalize(targetUnit.transform.position - transform.position);
        if (Vector3.Distance(transform.position, targetUnit.transform.position) > rigidbody.velocity.magnitude)
        {
            if (rigidbody.velocity.magnitude < maxVel)
            {
                rigidbody.AddForce(desiredVel * accelerate);
            }
        }
        
	}
}
