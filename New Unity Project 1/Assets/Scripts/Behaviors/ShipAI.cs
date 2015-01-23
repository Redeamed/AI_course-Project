using UnityEngine;
using System.Collections;

public class ShipAI : MonoBehaviour
{
    Vector3 desDir, curVel;
    float desSpeed = 0.0f, curSpeed = 0.0f;
    public float maxSpeed = 2.0f, acceleration = 0.1f, slowingRadius = 5.0f;
    public GameObject moveTarget, fleeTarget,target, avoidTarget;
    public string behaviors = "Seek";

    Ray colCheck;
    RaycastHit hit;
    void Start() { colCheck = new Ray(transform.position, transform.position + (transform.forward)); }
    void Update()
    {
        Calculate();
        StartCoroutine(behaviors, 1);
        if (Physics.Raycast(colCheck, out hit,5))
        {
            if (hit.transform.gameObject == moveTarget) { return; }
            avoidTarget = hit.transform.gameObject;
            Debug.DrawRay(colCheck.origin, colCheck.direction * 5, Color.red);
        }
        else
        {
            Debug.DrawRay(colCheck.origin, colCheck.direction * 5, Color.white);
        }
        if (avoidTarget != null)
        {
            StartCoroutine("Avoid", 1);
        }

        ApplySteering();
    }

    void Calculate()
    {
        desDir = Vector3.Normalize(moveTarget.transform.position- transform.position);
        transform.LookAt(transform.position + curVel);
        colCheck = new Ray(transform.position, transform.forward);
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
        if ((avoidTarget.transform.position - transform.position).magnitude > 8)
        {
            avoidTarget = null;
        }
        if (!avoidTarget) { return; }

        Vector3 fleeDir = Vector3.Normalize(avoidTarget.transform.position - transform.position);

        Vector3 desVel = ((fleeDir * maxSpeed) * mod);
        if (rigidbody.velocity != desVel)
        {
            curVel -= fleeDir * acceleration;
        }
        curSpeed = curVel.magnitude;
        if (curSpeed > maxSpeed) { curVel *= maxSpeed / curSpeed; }

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



