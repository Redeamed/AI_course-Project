using UnityEngine;
using System.Collections;

public class TargetAI : MonoBehaviour {

    GameObject owner;
	// Use this for initialization
	void Start () {
        owner = transform.parent.gameObject;
        ShipAI s = owner.GetComponent<ShipAI>();
        s.target = this.gameObject;
        if(s.moveTarget == null)s.moveTarget = this.gameObject;
	}

    public void noParent() { transform.parent = null; }
    public void Parent() { transform.parent = owner.transform; }

    void OnTriggerEnter(Collider other)
    {
        //if (owner == null) { Destroy(this.gameObject); }
        if (other.gameObject == owner)
        {
            Parent();
        }
    }
    void OnTriggerExit(Collider other)
    {
        //if (owner == null) { Destroy(this.gameObject); }

            noParent();
        
    }
}
