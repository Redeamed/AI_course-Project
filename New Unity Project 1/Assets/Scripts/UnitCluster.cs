using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitCluster : MonoBehaviour {
     struct ClusterStatus
    {
        public GameObject cluster;
        public bool ally;
    }
	public GameObject ship;
    public GameObject[] units;
    public int numUnits, spawnRange = 20;

     List<ClusterStatus> clusterStatus;
	// Use this for initialization
	void Start () 
	{
        
        units = new GameObject[numUnits];
        clusterStatus = new List<ClusterStatus>();
        for (int i = 0; i < numUnits; ++i) 
		{
            units[i] = Instantiate(ship, transform.position + (Random.onUnitSphere * spawnRange), Quaternion.identity) as GameObject;
		}
        foreach (GameObject cluster in UnitManager.singleton.unitClusters)
        {

            if(cluster != this && cluster != null)
            {

                ClusterStatus cs = new ClusterStatus();
                cs.cluster = cluster;
                if (cluster.GetComponent<UnitCluster>().ship == ship)
                {
                    cs.ally = true;
                }
                else
                {
                    cs.ally = false;
                }
                clusterStatus.Add(cs);
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
