using UnityEngine;
using System.Collections;

public class PlayerColliderController : MonoBehaviour {
    StatsController stats;

	// Use this for initialization
	void Start ()
    {
        stats = GetComponent<StatsController>();
        stats.health.onMinimum = OnDeath;
	}

    void OnDeath(Stat health)
    {
        Debug.LogWarning("PLAYER IS DEAD");
    }
	
	// Update is called once per frame
	void Update ()
    {

	}
   

    void OnHitEnemy(EnemyController enemy)
    {
        stats.health.Decrease(enemy.stats.attack.val);
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Enemy")
        { 
            OnHitEnemy(collider.gameObject.GetComponent<EnemyController>());
        }
    }
}
