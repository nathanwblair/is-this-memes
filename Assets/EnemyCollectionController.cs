using UnityEngine;
using System.Collections;

public class EnemyCollectionController : MonoBehaviour {
    public int Count
    {
        get
        {
            int result = 0;

            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                    result++;
            }

            return result;
        }
    }

    public void Add(GameObject enemy)
    {
        enemy.transform.parent = transform;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
