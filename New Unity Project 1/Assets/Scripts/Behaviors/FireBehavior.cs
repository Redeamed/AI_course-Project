using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireBehavior : MonoBehaviour {
    public GameObject projectile;
    public float reloadTime = 1.0f;
    float count = 0.0f;
	// Use this for initialization
    List<scr_Message> messages;
	void Start () 
    {
        messages = new List<scr_Message>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (messages.Count > 0)
        {

            StartCoroutine(messages[0].name);
            /*
            switch (messages[0].arguments.Count) 
            {
            case 0:
            StartCoroutine(messages[0].name);
            break;
                default:
            Debug.Log("Total Arguments unaccounted for" + this.name);
            break;
        }*/
            messages.Remove(messages[0]);
        }
        if (count > 0)
        {
            count -= Time.deltaTime;
        }
	}
    void Fire()
    {
        if (count <= 0.0f)
        {
            GameObject temp;

            temp = Instantiate(projectile, transform.position + transform.forward, transform.rotation) as GameObject;
            count = reloadTime;

        }
    }
    public bool addMessage(scr_Message msg)
    {
        bool success = true;
        messages.Add(msg);
        return success;
    }
}
