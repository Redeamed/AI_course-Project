using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    public float speed = 20.0f;
    public float durration = 2.0f;
    
	// Use this for initialization
	void Start () {
	rigidbody.velocity = transform.forward * speed;
	}
    
	
	// Update is called once per frame
	void Update () {
        durration -= Time.deltaTime;
        if (durration <= 0)
        {
            Destroy(this.gameObject);
        }
	}
}
