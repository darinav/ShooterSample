using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResolutionManager : MonoBehaviour
{    
    public Slider ResolutionSlider;
    int[] lowestres = { 960, 540 };
    int[] lowres = { 1280, 720 };
    int[] midres = { 1600, 900 };
    int[] highres = { 1920, 1080 };
    Resolution desirableResolution;
    
    public void OnTogglePress()
    {
        Screen.fullScreen = !Screen.fullScreen;
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
    }        

    public void OnApplyPress()
    {
        Screen.SetResolution(desirableResolution.width, desirableResolution.height, Screen.fullScreen);
        UI_MainMenu.CurrentRes = Screen.currentResolution;
    }
}
