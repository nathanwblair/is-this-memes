using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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

    private float bossFightTime;
    float bossDelayScale;

    public List<int> enemyWaves = new List<int>();
    private Queue<int> _enemyWaves;

    float currentSpawnDelay;
    public Vector2 spawnDelayRange = new Vector2(1, 3);

    public GameObject spawnPrefab;

    SocketContainerController sockets;

    private uint spawnsSincePause = 0;

    private float delayScale = 1.0f;

    public float delayDropoff = 0.7f;

    private EnemyCollectionController enemies;

    void OnEnable()
    {
        _enemyWaves = new Queue<int>(enemyWaves);
    }

    // Use this for initialization
    void Start ()
    {
        sockets = GetComponentInChildren<SocketContainerController>();
        enemies = GameObject.Find("Enemies").GetComponent<EnemyCollectionController>();
        Reset();

        bossFightTime = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>().phaseTime;
        bossDelayScale = bossFightTime / spawnDelayRange.x;
    }

    void Reset(float pause=1.0f)
    {
        delayScale = Mathf.Max(pause, delayScale);

        if (pause > 1.0f)
            spawnsSincePause = 0;

        var minDelay = spawnDelayRange.x * delayScale;
        var maxDelay = Mathf.Max(spawnDelayRange.y, minDelay);

        currentSpawnDelay = UnityEngine.Random.Range(minDelay, maxDelay);
        spawnTimer.Start();
    }

    void NewWave()
    {
        Reset(bossDelayScale);
        if (_enemyWaves.Count > 1)
           _enemyWaves.Dequeue();
    }

    void TryNewWave()
    {
        var areNoEnemies = enemies.Count == 0;
        var areAllEnemiesSpawned = spawnsSincePause >= _enemyWaves.Peek();

        if (areNoEnemies && areAllEnemiesSpawned)
        {
            NewWave();
        }
    }

    void Spawn()
    {

        if (spawnsSincePause < _enemyWaves.Peek())
        {
            spawnsSincePause++;

            if (delayScale > 1)
            {
                float delayScaleScale = Mathf.Min(1.0f, delayDropoff / ((float)spawnsSincePause * (float)spawnsSincePause));
                delayScale *= delayScaleScale;
            }

            if (delayScale < 1)
                delayScale = 1.0f;

            var spawnPoint = sockets.RandomSpawnPoint();

            var spawn = (GameObject)GameObject.Instantiate(spawnPrefab, spawnPoint, spawnPrefab.transform.rotation);
            spawn.SetActive(true);
            enemies.Add(spawn);
        }
        else
        {
            TryNewWave();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (spawnTimer.GetElapsedTime() > currentSpawnDelay)
        {
            Spawn();
            Reset();
        }

        TryNewWave();
	}
}
