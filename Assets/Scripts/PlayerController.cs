using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public StatsController stats;

	// Use this for initialization
	void Start () {
        stats = GetComponent<StatsController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
