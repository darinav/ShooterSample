using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_PauseManager : MonoBehaviour
{
    public GameObject PausePanel;
    public TP_Camera CameraScript;
    public Canvas HUDCanvas;
    public Button Resume;
    public Button Quit;

    void Awake()
    {
        PausePanel = GameObject.Find("PausePanel");
        PausePanel.SetActive(false);
        HUDCanvas = HUDCanvas.GetComponent<Canvas>();
        Resume = Resume.GetComponent<Button>();
        Quit = Quit.GetComponent<Button>();
    }
	
	void Update()
    {
	    if(Input.GetKey(KeyCode.Escape) && !PausePanel.activeSelf)
        {
            CameraScript.enabled = false;
            PausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
	}

    public void OnResumePress()
    {
        CameraScript.enabled = true;
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnQuitPress()
    {
        Application.LoadLevel(0);
    }
}
