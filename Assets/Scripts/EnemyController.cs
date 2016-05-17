using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
    public StatsController stats;

	// Use this for initialization
	void Start () {
        stats = GetComponent<StatsController>();
        stats.speed.val = 10;
        stats.attack.val = 1;
        stats.health.onMinimum = OnDeath;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            gameObject.SetActive(false);
    }

    public void OnHit(PlayerController player)
    {

        stats.health.Decrease(player.stats.attack.val);
    }

    public void OnDeath(Stat parent)
    {
        gameObject.SetActive(false);
    }


}
