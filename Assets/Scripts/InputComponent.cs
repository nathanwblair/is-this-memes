using UnityEngine;
using System.Collections;

public class InputComponent : MonoBehaviour
{
    Vector2 touchStart;
    Vector2 touchCurrent;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchStart = Input.GetTouch(0).position;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touchCurrent = Input.GetTouch(0).position;
            }

            Debug.DrawLine(touchStart, touchCurrent, Color.red);
        }
    }
}