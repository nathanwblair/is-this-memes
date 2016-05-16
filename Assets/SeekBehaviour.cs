using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SeekBehaviour : MonoBehaviour
{
    StatsController stats;
    Rigidbody rb;
    // Tag to find target(s) with (eg. Player or Enemy)
    public List<string> targetQuery = new List<string>();

    delegate float Function(float input);

    Function dropoff;
    float weight = 1.0f;

    float DefaultDropoff(float input)
    {
        return 1.0f;
    }

    // Use this for initialization
    void Start ()
    {
        stats = GetComponent<StatsController>();
        rb = GetComponent<Rigidbody>();

        targetQuery.Add("Player");
        dropoff = DefaultDropoff;
	}

    List<Vector3> FindTargets(string tag)
    {
        var targetGameObjects = GameObject.FindGameObjectsWithTag(tag);

        List<Vector3> result = new List<Vector3>();

        foreach(var go in targetGameObjects)
        {
            result.Add(go.transform.position);
        }

        return result;
    }
	
    void Seek(string tag)
    {
        var targets = FindTargets(tag);

        foreach (var target in targets)
        {
            var distance = Vector3.Distance(target, transform.position);
            var globalWeight = dropoff(distance) * weight;

            var direction = (target - transform.position).normalized;

            var velocity = globalWeight * direction * stats.speed.val;

            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        foreach(var tag in targetQuery)
        {
            Seek(tag);
        }
	}
}
