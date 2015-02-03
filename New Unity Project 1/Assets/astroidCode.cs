using UnityEngine;
using System.Collections;

public class astroidCode : MonoBehaviour {

    AstroidManager astroidManager;
    public float life = 60.0f;
	// Use this for initialization
	void Start () {
        InvokeRepeating("CheckStatus", 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CheckStatus() 
    {
        life -= 1.0f;
        if (life <= 0)
        {
            astroidManager.NewAstroid();
            Destroy(this.gameObject);
        }
    }

    public void SetAM(AstroidManager am) { astroidManager = am; }
}
