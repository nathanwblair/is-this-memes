using UnityEngine;
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

    float slashZLocation = 20;

    private float lerpIncrement = 1;

    //game vars
    public float slashTimeout;

    //vars
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private float slashTime;

    //debug vars
    float debugSlashTimeout = 0;

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
        //slash debugging
        //if (debugSlashTimeout <= 0)
        //{
        //    Vector2 start = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
        //    Vector2 end = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.width));
        //    Slash e = new Slash(
        //        start,
        //        end,
        //        Instantiate(emitterPrefab));
        //    slashList.Add(e);
        //
        //    debugSlashTimeout = Random.Range(0, 1);
        //}
        //debugSlashTimeout -= Time.deltaTime;
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
            else if (Input.GetTouch(0).phase == TouchPhase.Moved
                && !slashList[slashList.Count-1].dead)
            {
                slashTime += Time.deltaTime;
                touchEnd = Input.GetTouch(0).position;
                slashList[slashList.Count - 1].touchEnd = touchEnd;

                //raycast at finger location
                RaycastHit rayHit;
                Physics.Raycast(
                    new Vector3(touchEnd.x, touchEnd.y, slashZLocation),
                    camera.transform.forward,
                    out rayHit,
                    200);

                if (rayHit.collider.tag == "Enemy")
                {
                    //todo: collision, pass direction to check against 'weak' spot
                    rayHit.collider.GetComponent<EnemyController>().OnHit(player);
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
        UpdateSlash();
    }

    void UpdateSlash()
    {
        for (int i = 0; i < slashList.Count; ++i)
        {
            Vector3 lineEnd = camera.ScreenToWorldPoint(new Vector3(slashList[i].touchEnd.x, slashList[i].touchEnd.y, slashZLocation));
            //slashList[i].emitter.transform.position = Vector3.Lerp(lineStart, lineEnd, slashList[i].lerpAmount);
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