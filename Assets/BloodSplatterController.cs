using UnityEngine;
using System.Collections;

public class BloodSplatterController : MonoBehaviour {

    float timeAlive = 1.167f;
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeAlive -= Time.deltaTime;
        if (timeAlive <= 0)
        {
            Destroy(gameObject);
        }
	}
}
