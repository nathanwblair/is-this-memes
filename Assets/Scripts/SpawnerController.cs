using UnityEngine;
using System;
using System.Collections;

public class Timer
{

    private DateTime startTime;
    private DateTime stopTime;
    private bool running = false;


    public void Start()
    {
        this.startTime = DateTime.Now;
        this.running = true;
    }


    public void Stop()
    {
        this.stopTime = DateTime.Now;
        this.running = false;
    }

    // elaspsed time in seconds
    public float GetElapsedTime()
    {
        TimeSpan interval;

        if (running)
            interval = DateTime.Now - startTime;
        else
            interval = stopTime - startTime;

        return (float)interval.TotalSeconds;
    }
}
public class SpawnerController : MonoBehaviour
{
    Timer spawnTimer = new Timer();

    float currentSpawnDelay;
    public Vector2 spawnDelayRange = new Vector2(1, 3);

    public GameObject spawnPrefab;

    SocketContainerController sockets;


    // Use this for initialization
    void Start ()
    {
        sockets = GetComponentInChildren<SocketContainerController>();
        Reset();
	}

    void Reset()
    {
        currentSpawnDelay = UnityEngine.Random.Range(spawnDelayRange.x, spawnDelayRange.y);
        spawnTimer.Start();
    }

    void Spawn()
    {
        var spawnPoint = sockets.RandomSpawnPoint();

        GameObject.Instantiate(spawnPrefab, spawnPoint, spawnPrefab.transform.rotation);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (spawnTimer.GetElapsedTime() > currentSpawnDelay)
        {
            Spawn();
            Reset();
        }
	}
}
