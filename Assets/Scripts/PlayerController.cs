using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public StatsController stats;
    public GameManager gameManager;

	// Use this for initialization
	void Start () {
        stats = GetComponent<StatsController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stats.health.onMinimum = OnDeath;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnDeath(Stat parent)
    {
        //
        gameManager.OnLose();
    }

    public void OnHit()
    {
        GameObject.Find("UI Canvas").GetComponent<HealthScript>().DecreaseHearts();
        stats.health.Decrease(1);
    }
}
