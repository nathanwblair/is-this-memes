using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
    public StatsController stats;

	// Use this for initialization
	void Start () {
        stats = GetComponent<StatsController>();
        stats.speed.val = 10;
        stats.attack.val = 1;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
