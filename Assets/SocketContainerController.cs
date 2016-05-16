using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SocketContainerController : MonoBehaviour
{
    public List<Transform> sockets;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame           oo
	void Update ()
    {
	
	}

    public Vector3 RandomSpawnPoint()
    {
        var triangle = RandomSocketTriangle();

        var lerp1 = Random.value;
        var lerp2 = Random.value;
        var lerp3 = Random.value;

        var first = Vector3.Lerp(triangle[0].position, triangle[1].position, lerp1);
        var second = Vector3.Lerp(first, triangle[2].position, lerp2);

        return second;
    }

    public void Validate()
    {
        sockets = Sockets();
    }

    public List<Transform> Sockets()
    {
        var result = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            result.Add(transform.GetChild(i));
        }

        return result;
    }

    public static int Closer(Transform t1, Transform t2, Transform self)
    {
        return Vector3.Distance(t1.position, 
                                self.position)
            .CompareTo(
                Vector3.Distance(
                    t2.position,
                    self.position));
    } 

    public List<Transform> SocketTriangle(Transform rootSocket)
    {
        Validate();

        var closestSockets = new List<Transform>(sockets);
        closestSockets.Sort((t1, t2) => Closer(t1, t2, rootSocket));

        return new List<Transform> { rootSocket, closestSockets[0], closestSockets[1] };
    }

    public List<Transform> RandomSocketTriangle()
    {
        return SocketTriangle(RandomSocket());
    }

    public Transform RandomSocket()
    {
        if (sockets.Count == 0)
        {
            Validate();

            if (sockets.Count == 0)
                return null;
        }
        var rootSocketIndex = Random.Range(0, sockets.Count - 1);
        var result = sockets[rootSocketIndex];

        return result;
    }
}
