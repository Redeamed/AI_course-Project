using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ShipAI : MonoBehaviour
{
    public float max_Health = 5;
    float health;
    Vector3 desDir, curVel;
    float desSpeed = 0.0f, curSpeed = 0.0f;
    public float maxSpeed = 2.0f, acceleration = 0.1f, slowingRadius = 5.0f;
    public GameObject moveTarget, fleeTarget,target;
    public List<GameObject> avoidTarget;
    public GameObject myCluster;
    public string behaviors = "Seek";
    Ray colCheck;
    RaycastHit hit;
    scr_Message fireMessage;
    FireBehavior fireBehavior;

    List<scr_Message> messages;

    void Start() 
    {
        health = max_Health;
        colCheck = new Ray(transform.position, transform.position + (transform.forward));
        avoidTarget = new List<GameObject>();

        messages = new List<scr_Message>();

        fireMessage = gameObject.AddComponent<scr_Message>();
        fireMessage.name = "Fire";
        fireBehavior = gameObject.GetComponent<FireBehavior>();
    }
    public void AddMessage(scr_Message message)
    {
        messages.Add(message);
    }
    public void PriorityMessage(scr_Message message)
    {
        messages.Insert(0, message);
    }
    void doMessage()
    {
        if (messages.Count > 0)
        {

            StartCoroutine(messages[0].name);
            messages.Remove(messages[0]);
        }
    }
    void Update()
    {
        
        


        Calculate();
        StartCoroutine(behaviors, 1);
        if (Physics.Raycast(colCheck, out hit,5))
        {
            if (hit.transform.gameObject == moveTarget) { return; }
            AvoidAdd(hit.transform.gameObject);
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
        if (behaviors == "Arrive" && target == moveTarget)
        {
            FindTarget();
        }
        moveTarget.transform.position = target.transform.position;

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
    void Attack(float mod)
    {
        if (target == moveTarget || target == null)
        {
            FindTarget();
        }
        Arrive(mod);
        Vector3 tempVec = moveTarget.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, tempVec) <= 5)
        {
            fireBehavior.addMessage(fireMessage);
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


    void FindTarget() 
    {
        foreach (UnitCluster.ClusterStatus cs in myCluster.GetComponent<UnitCluster>().clusterStatus)
        {
            if (cs.ally)
            {
                continue;
            }
            else
            {
                GameObject[] allyArray = cs.cluster.GetComponent<UnitCluster>().units;
                int possibleTargets = allyArray.Length;
                target = allyArray[Random.Range(0, possibleTargets - 1)];
            }
        }
    }
    void onTriggerEnter(Collider other)
    {
        AvoidAdd(other.gameObject);
    }
    void AvoidAdd(GameObject avoid)
    {
        ShipAI otherAI = avoid.GetComponent<ShipAI>();
        if (otherAI.moveTarget == this.gameObject)
        {
            this.fleeTarget = avoid.gameObject;
        }
        else
        {
            if (avoidTarget.Contains(avoid.gameObject) == false && avoid.gameObject != fireBehavior.projectile)
            {
                avoidTarget.Add(avoid.gameObject);
            }
        }
    }
}



