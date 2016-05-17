using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{

    public StatsController stats;
    private GameManager gameManager;

    public Vector3 startPoint;
    public Vector3 endPoint;

    bool inPhase = false;
    float phaseLerpIncreasing = 1;

    private float phaseLerp = 0;
    private float phaseLerpInc = 0.5f;
    private float currPhaseTime = 0;
    public float phaseTime = 5;

    // Use this for initialization
    void Start()
    {
        stats = GetComponent<StatsController>();
        stats.health.onMinimum = OnDeath;

        startPoint = transform.position;
        endPoint = new Vector3(0, 0, 12);

        gameManager = GameObject.FindGameObjectWithTag("GameManager")
            .GetComponent<GameManager>();

        StartVulnerablePhase();
    }

    // Update is called once per frame
    void Update()
    {
        if (inPhase)
        {
            if (phaseLerpIncreasing == 1)
            {
                transform.position = Vector3.Lerp(startPoint, endPoint, phaseLerp);
                phaseLerp += phaseLerpInc * Time.deltaTime;
                if (phaseLerp >= 1)
                {
                    phaseLerp = 0;
                    phaseLerpIncreasing = 0;
                }
            }
            else if (phaseLerpIncreasing == -1)
            {
                transform.position = Vector3.Lerp(endPoint, startPoint, phaseLerp);
                phaseLerp += phaseLerpInc * Time.deltaTime;

                if (phaseLerp >= 1)
                {
                    phaseLerp = 0;
                    phaseLerpIncreasing = 0;
                    EndVulnerablePhase();
                }
            }
            else if (phaseLerpIncreasing == 0)
            {
                if (currPhaseTime > phaseTime)
                {
                    currPhaseTime = 0;
                    phaseLerpIncreasing = -1;
                }
            }
            currPhaseTime += Time.deltaTime;
        }
    }

    public void StartVulnerablePhase()
    {
        inPhase = true;
        List<SphereCollider> colliderList = new List<SphereCollider>();
        GetComponents<SphereCollider>(colliderList);

        foreach (SphereCollider sc in colliderList)
        {
            sc.enabled = true;
        }
    }

    public void EndVulnerablePhase()
    {
        inPhase = false;
        List<SphereCollider> colliderList = new List<SphereCollider>();
        GetComponents<SphereCollider>(colliderList);

        foreach (SphereCollider sc in colliderList)
        {
            sc.enabled = false;
        }
    }

    public void OnHit(PlayerController player)
    {
        stats.health.Decrease(player.stats.attack.val);
    }

    public void OnDeath(Stat parent)
    {
        gameObject.SetActive(false);
        gameManager.OnWin();
    }
}
