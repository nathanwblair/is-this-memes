using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public StatsController stats;
    public GameManager gameManager;

    private bool toggling = false;
    private float toggleDelay = 0;
    private float absoluteToggleDelay = 0.016f * 6;

	// Use this for initialization
	void Start () {
        stats = GetComponent<StatsController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stats.health.onMinimum = OnDeath;

        //GetComponentInChildren<CanvasToggler>().Toggle();
    }
	
	// Update is called once per frame
	void Update () {
        if (toggling)
        {
            toggleDelay += Time.deltaTime;
            if (toggleDelay > absoluteToggleDelay)
            {
                toggling = false;
                GetComponentInChildren<CanvasToggler>().Toggle();
            }
        }
	}

    public void OnDeath(Stat parent)
    {
        //
        gameManager.OnLose();
    }

    public void OnHit()
    {
        toggleDelay = 0;
        GetComponentInChildren<CanvasToggler>().Toggle();
        toggling = true;

        GameObject.Find("UI Canvas").GetComponent<HealthScript>().DecreaseHearts();
        stats.health.Decrease(1);
    }
}
