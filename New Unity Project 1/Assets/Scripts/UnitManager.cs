using UnityEngine;
using System.Collections;

public class UnitManager : MonoBehaviour {
    
    public static UnitManager singleton;

    public GameObject[] unitClusters;
    void Awake() 
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else 
        {
            Debug.Log("can not create multiple Unit Managers");
            Destroy(this);
        }
        unitClusters = GameObject.FindGameObjectsWithTag("Cluster");
        
    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
