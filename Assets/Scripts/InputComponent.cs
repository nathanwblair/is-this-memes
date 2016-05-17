﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputComponent : MonoBehaviour
{
    //game objects
    private Camera camera;
    private PlayerController player;

    //slash variables
    public GameObject emitterPrefab;
    public class Slash
    {
        public Slash(Vector2 touchStart, Vector2 touchEnd, GameObject emitter)
        {
            this.touchStart = touchStart;
            this.touchEnd = touchEnd;
            this.emitter = emitter;
            lerpAmount = 0;
            dead = false;
            hasDirection = false;
        }
        public Vector2 touchStart, touchEnd;
        public GameObject emitter;
        public float lerpAmount;

        public bool hasDirection;
        public Vector2 direction;

        public void Kill()
        {
            dead = true;
            lifeLeft = 0.5f;
        }
        public bool dead;
        public float lifeLeft;
    }
    List<Slash> slashList;

    private float slashZLocation = 20;

    //game vars
    public float slashTimeout;

    //vars
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private float slashTime;

    //debug vars

    // Use this for initialization
    void Start()
    {
        touchStart = new Vector2();
        touchEnd = new Vector2();
        slashList = new List<Slash>();

        player = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerController>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (slashTime >= slashTimeout)
        {
            slashList[slashList.Count - 1].Kill();
        }

        if (Input.touchCount > 0)
        {
            //slash begins
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchStart = Input.GetTouch(0).position;
                //do the slash
                Slash e = new Slash(
                    new Vector2(touchStart.x, touchStart.y),
                    new Vector2(touchStart.x, touchStart.y),
                    Instantiate(emitterPrefab));
                slashList.Add(e);

                slashTime = 0;
            }
            //slash in progress
            if (slashList.Count > 0 && Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved
                   && !slashList[slashList.Count - 1].dead)
                {
                    slashTime += Time.deltaTime;
                    touchEnd = Input.GetTouch(0).position;
                    slashList[slashList.Count - 1].touchEnd = touchEnd;

                    //raycast at finger location
                    RaycastHit rayHit;
                    Ray raycastOrigin =
                        camera.ScreenPointToRay(new Vector3(touchEnd.x, touchEnd.y, 1));

                    Physics.Raycast(raycastOrigin, out rayHit, 1000.0f);
                    if (rayHit.collider != null)
                    {
                        if (rayHit.collider.tag == "Enemy")
                        {
                            rayHit.collider.GetComponent<EnemyController>().OnHit(player);
                        }
                        else if (rayHit.collider.tag == "Boss")
                        {
                            rayHit.collider.GetComponent<BossController>().OnHit(player);
                        }
                    }

                    //store direction
                    if (!slashList[slashList.Count - 1].hasDirection)
                    {
                        slashList[slashList.Count - 1].direction = (touchEnd - touchStart).normalized;
                    }

                    //check angle for rejection
                    float deltaAngle = Vector2.Dot(slashList[slashList.Count - 1].direction, Input.GetTouch(0).deltaPosition.normalized);
                    if (deltaAngle <= -0.25f)
                    {
                        slashList[slashList.Count - 1].Kill();
                    }
                }
                //slash ends
                else if (Input.GetTouch(0).phase == TouchPhase.Ended
                    && !slashList[slashList.Count - 1].dead)
                {
                    //if the slash was fine
                    slashTime = 0;
                    slashList[slashList.Count - 1].Kill();
                }
            }
        }
        UpdateSlash();
    }

    void UpdateSlash()
    {
        for (int i = 0; i < slashList.Count; ++i)
        {
            Vector3 lineEnd = camera.ScreenToWorldPoint(new Vector3(slashList[i].touchEnd.x, slashList[i].touchEnd.y, slashZLocation));
            slashList[i].emitter.transform.position = lineEnd;

            if (slashList[i].dead)
            {
                slashList[i].lifeLeft -= Time.deltaTime;
            }
            if (slashList[i].dead && (slashList[i].lifeLeft <= 0))
            {
                Destroy(slashList[i].emitter);
                slashList.RemoveAt(i);
            }
        }
    }

}