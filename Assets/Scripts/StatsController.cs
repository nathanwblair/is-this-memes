using UnityEngine;
using System.Collections;

[System.Serializable]
public class Stat
{
    public float val;
    public Vector2 range;

    public delegate void OnEvent(Stat parent);

    public OnEvent onMinimum = DefaultEvent;
    public OnEvent onAboveMaximum = DefaultEvent;

    public static void DefaultEvent(Stat parent)
    {
        // pass (By Python™)
    }

    public Stat(float _val, float _min=0, float _max=100)
    {
        val = _val;
        range = new Vector2(_min, _max);
    }

    public float Increase(float input)
    {
        val += input;

        Validate();

        return val;
    }

    public float Decrease(float input)
    {
        return Increase(input * -1.0f);
    }

    public void Validate()
    {
        if (val <= range.x)
        {
            onMinimum(this);
        }
        else if (val > range.y)
        {
            onAboveMaximum(this);
        }
    }
}

public class StatsController : MonoBehaviour {
    public Stat attack = new Stat(10, 0, 10);
    public Stat health = new Stat(1, 0, 1);
    public Stat speed = new Stat(20, 0, 100);

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
