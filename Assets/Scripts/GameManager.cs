using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class GameManager : MonoBehaviour
{

    GameObject pauseMenuCanvas;
    GameObject uiCanvas;
    CanvasToggler canvasToggler;

    public bool paused = false;

    // Use this for initialization
    void Start()
    {
        pauseMenuCanvas = GameObject.FindGameObjectWithTag("PauseCanvas") ;
        uiCanvas = GameObject.FindGameObjectWithTag("UICanvas");
        canvasToggler = pauseMenuCanvas.GetComponent<CanvasToggler>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameObject.FindGameObjectsWithTag("Derp").Length);
    }

    public void OnPause()
    {
        canvasToggler.Toggle();
        Time.timeScale = 0;
        paused = true;
    }
    public void OnResume()
    {
        canvasToggler.Toggle();
        Time.timeScale = 1;
        paused = false;
    }

    public void OnLose()
    {
        //todo: lose
    }

    public void OnWin()
    {
        //todo: win
    }
}
