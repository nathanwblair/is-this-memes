using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthScript : MonoBehaviour
{

    float numHearts = 3;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DecreaseHearts()
    {
        if (numHearts == 3)
        {
            transform.FindChild("Player Health Broken Image 1").GetComponent<Image>().enabled = true;
            transform.FindChild("Player Health Image 1").GetComponent<Image>().enabled = false;
            numHearts--;
        }
        else if (numHearts == 2)
        {
            transform.FindChild("Player Health Broken Image 2").GetComponent<Image>().enabled = true;
            transform.FindChild("Player Health Image 2").GetComponent<Image>().enabled = false;
        }
    }
}
