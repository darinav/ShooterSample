using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResolutionManager : MonoBehaviour
{    
    public Slider ResolutionSlider;
    public Text ResolutionText;
    int[] lowestres = { 960, 540 };
    int[] lowres = { 1280, 720 };
    int[] midres = { 1600, 900 };
    int[] highres = { 1920, 1080 };
    Resolution desirableResolution;
    GameObject myEventSystem;

    public void OnTogglePress()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    void Start()
    {
        ResolutionText.text = Screen.width + "x" + Screen.height;
        myEventSystem = GameObject.Find("EventSystem");
    }

    public void ValueChangeCheck()
    {
        if(ResolutionSlider.value == 0)
        {
            desirableResolution.width = lowestres[0];
            desirableResolution.height = lowestres[1];
        }
        else if (ResolutionSlider.value == 1)
        {
            desirableResolution.width = lowres[0];
            desirableResolution.height = lowres[1];
        }
        else if (ResolutionSlider.value == 2)
        {
            desirableResolution.width = midres[0];
            desirableResolution.height = midres[1];
        }
        else if (ResolutionSlider.value == 3)
        {
            desirableResolution.width = highres[0];
            desirableResolution.height = highres[1];
        }
        ResolutionText.text = desirableResolution.width + "x" + desirableResolution.height;
    }        

    public void OnApplyPress()
    {
        Screen.SetResolution(desirableResolution.width, desirableResolution.height, Screen.fullScreen);
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
}
