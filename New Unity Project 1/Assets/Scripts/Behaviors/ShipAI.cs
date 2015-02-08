using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ShipAI : MonoBehaviour
{
    Vector3 desDir, curVel;
    float desSpeed = 0.0f, curSpeed = 0.0f;
    public float maxSpeed = 2.0f, acceleration = 0.1f, slowingRadius = 5.0f;
    public GameObject moveTarget, fleeTarget,target;
    public List<GameObject> avoidTarget;
    public string behaviors = "Seek";
    float radar = .1f;
    Ray colCheck;
    RaycastHit hit;
    scr_Message fireMessage;
    FireBehavior fireBehavior;

    void Start() 
    { 
        colCheck = new Ray(transform.position, transform.position + (transform.forward));
        avoidTarget = new List<GameObject>();

        fireMessage = gameObject.AddComponent<scr_Message>();
        fireMessage.name = "Fire";
        fireBehavior = gameObject.GetComponent<FireBehavior>();
    }

    void Update()
    {
        if (fireBehavior == true)
        {
            fireBehavior.addMessage(fireMessage);
        }


        Calculate();
        StartCoroutine(behaviors, 1);
        if (Physics.Raycast(colCheck, out hit,5))
        {
            if (hit.transform.gameObject == moveTarget) { return; }
            avoidTarget.Add(hit.transform.gameObject);
            Debug.DrawRay(colCheck.origin, colCheck.direction * 5, Color.red);
        }
        else
        {
            Debug.DrawRay(colCheck.origin, colCheck.direction * 5, Color.white);
        }
        if (avoidTarget.Count > 0)
        {
            StartCoroutine("Avoid", 1);
        }

        ApplySteering();
    }

    void Calculate()
    {
        desDir = Vector3.Normalize(moveTarget.transform.position- transform.position);
        transform.LookAt(transform.position + curVel);
        //Vector3 rayDir = new Vector3();
        //rayDir.y = (transform.forward.y * Mathf.Cos(radar) - (transform.forward.z * Mathf.Sin(radar)));
        //rayDir.z = (transform.forward.y * Mathf.Sin(radar) + (transform.forward.z * Mathf.Cos(radar)));
        colCheck = new Ray(transform.position, transform.forward);
        radar += 1f;
    }
    void ApplySteering() 
    {
        rigidbody.velocity = curVel;
    }
    void Seek(float mod)
    {

        Vector3 desVel = ((desDir * maxSpeed) * mod);
        if (rigidbody.velocity != desVel)
        {
            curVel += desDir * acceleration;
        }
        curSpeed = curVel.magnitude;
        if (curSpeed > maxSpeed) { curVel *= maxSpeed / curSpeed; }
    }
    void Flee(float mod)
    {
       // if ((fleeTarget.transform.position - transform.position).magnitude > 5) { return; }
        Vector3 fleeDir = Vector3.Normalize(fleeTarget.transform.position - transform.position);

        Vector3 desVel = ((fleeDir * maxSpeed) * mod);
        if (rigidbody.velocity != desVel)
        {
            curVel -= fleeDir * acceleration;
        }
        curSpeed = curVel.magnitude;
        if (curSpeed > maxSpeed) { curVel *= maxSpeed / curSpeed; }
    }
    void Avoid(float mod)
    {
        List<GameObject> removeTargets = new List<GameObject>();
        foreach(GameObject avoid in avoidTarget)
        {
            if (avoid == null || (avoid.transform.position - transform.position).magnitude > 8  )
        {
            removeTargets.Add(avoid);
            continue; 
        }

            Vector3 fleeDir = Vector3.Normalize(avoid.transform.position - transform.position);

        Vector3 desVel = ((fleeDir * maxSpeed) * mod);
        if (rigidbody.velocity != desVel)
        {
            curVel -= fleeDir * acceleration;
        }
        curSpeed = curVel.magnitude;
        if (curSpeed > maxSpeed) { curVel *= maxSpeed / curSpeed; }
    }
        foreach (GameObject remove in removeTargets)
        {
            avoidTarget.Remove(remove);
        }
    }
    void Arrive(float mod)
    {
        float dis = (moveTarget.transform.position - transform.position).magnitude;
        if ( dis > slowingRadius) 
        { Seek(mod); }
        else 
        {
            Vector3 desVel = ((desDir * maxSpeed) * (dis/slowingRadius));
            if (rigidbody.velocity != desVel)
            {
                curVel += desDir * acceleration ;
            }
            curSpeed = curVel.magnitude;
             desSpeed = desVel.magnitude;
             if (curSpeed > desSpeed) { curVel *= desSpeed / curSpeed; }

        }
    }
    void Wander(float mod) 
    {
        if (target.transform.parent != null)
        {
            target.transform.position = transform.position +transform.forward * 3 + Random.onUnitSphere * 3;
            moveTarget = target;
            target.GetComponent<TargetAI>().noParent();
        }
        else 
        {
            Arrive(mod);
        }
    }
    void onTriggerEnter(Collider other)
    {
        ShipAI otherAI = other.gameObject.GetComponent<ShipAI>();
        if (otherAI.moveTarget == this.gameObject)
        {
            this.fleeTarget = other.gameObject;
        }
    }
}



