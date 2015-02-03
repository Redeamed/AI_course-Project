using UnityEngine;
using System.Collections;

public class AstroidManager : MonoBehaviour {
    public GameObject astroid;
    public int numAstroid = 10;
	// Use this for initialization
	void Start () {

        StartCoroutine(Init());
	}

    // Update is called once per frame
    void Update()
    {
	}

    IEnumerator Init()
    {
        for (int i = 0; i < numAstroid; ++i)
        {
            NewAstroid();
        //float wait = Random.Range(50f,1000f);
            yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        }
    }

    public void NewAstroid() 
    {
        Vector3 pos = new Vector3(Random.Range(-100, -200), Random.Range(-50, 50), Random.Range(-25, 25));
        GameObject tempAstroid = Instantiate(astroid,pos,Quaternion.identity) as GameObject;
        tempAstroid.rigidbody.velocity = new Vector3(Random.Range(3,7), 0, 0);
        float scale = Random.Range(1,5);
        tempAstroid.transform.localScale *= scale;
        tempAstroid.GetComponent<astroidCode>().SetAM(this);
    }
}
