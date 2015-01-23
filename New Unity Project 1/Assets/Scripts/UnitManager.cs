using UnityEngine;
using System.Collections;

public class UnitManager : MonoBehaviour {
	static UnitManager singlton;

	void Awake()
	{
		if (singlton == null)
		{
			singlton = this;
		}
		else
		{
			Debug.Log ("can not create another singltion: " + this.name);
			Destroy(this);
		}
	}

	public GameObject allyShip, enemyShip, astroid;
	public GameObject[] allies, enemies;
    public int numAllies, numEnemies, numAstroids, spawnRange = 20;


	// Use this for initialization
	void Start () 
	{
		allies = new GameObject[numAllies];
		enemies = new GameObject[numEnemies];

		for (int i = 0; i < numAllies; ++i) 
		{
			allies[i] = Instantiate(allyShip,Random.onUnitSphere * spawnRange,Quaternion.identity) as GameObject;
		}
		for (int i = 0; i < numEnemies; ++i) 
		{
            enemies[i] = Instantiate(enemyShip, Random.onUnitSphere * spawnRange, Quaternion.identity) as GameObject;
            enemies[i].GetComponent<ShipAI>().moveTarget = allies[Random.Range(0,numAllies-1)];
        }
        for (int i = 0; i < numAstroids; ++i)
        {
            GameObject a = Instantiate(astroid, Random.onUnitSphere * spawnRange, Quaternion.identity) as GameObject;
            a.rigidbody.AddTorque(Random.insideUnitSphere);
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
