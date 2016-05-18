using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public StatsController stats;
    public GameObject bloodSplatter;

    public InputComponent input;
    public PlayerController player;

    // Use this for initialization
    void Start()
    {
        stats = GetComponent<StatsController>();
        stats.speed.val = 10;
        stats.attack.val = 1;
        stats.health.onMinimum = OnDeath;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        if (input.isSlashing)
        {
            OnHit(player);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().OnHit();
            OnDeath(stats.health);
        }
    }

    public void OnHit(PlayerController player)
    {
        stats.health.Decrease(player.stats.attack.val);
    }

    public void OnDeath(Stat parent)
    {
        Instantiate(bloodSplatter, this.transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
