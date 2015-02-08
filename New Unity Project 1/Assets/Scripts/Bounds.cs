using UnityEngine;
using System.Collections;

public class Bounds : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerExit(Collider other)
	{
        ShipAI s = other.GetComponent<ShipAI>();
        if (s != null)
        {
            s.target.transform.position = transform.position;
        }
	}
}
