using UnityEngine;
using System.Collections;

public class CanvasToggler : MonoBehaviour
{
    Canvas canvasObject;
	// Use this for initialization
	void Start ()
    {
        canvasObject = GetComponent<Canvas>();   
	}

    public void Toggle()
    {
        canvasObject.enabled = !canvasObject.enabled;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
