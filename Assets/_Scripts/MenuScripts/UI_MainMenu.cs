using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_MainMenu : MonoBehaviour
{
    public Canvas MenuCanvas;
    public GameObject MenuPanel;
    public GameObject SettingsPanel;
    public Button Play;
    public Button Settings;
    public Button Quit;

    void Awake()
    {
        MenuCanvas = MenuCanvas.GetComponent<Canvas>();
        MenuPanel = GameObject.Find("MenuPanel");
        SettingsPanel = GameObject.Find("SettingsPanel");
        Play = GetComponent<Button>();
        Settings = GetComponent<Button>();
        Quit = GetComponent<Button>();

        SettingsPanel.SetActive(false);
    }

    public void OnPlayPress()
    {
        Application.LoadLevel(1);
    }

    public void OnSettingsPress()
    {
        SettingsPanel.SetActive(true);
        MenuPanel.SetActive(false);
    }

    public void OnQuitPress()
    {
        Application.Quit();
    }

    public void OnBackPress()
    {
        SettingsPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }
}
