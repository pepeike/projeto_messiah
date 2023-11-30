using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Resolução : MonoBehaviour {

    Resolution[] resolutions;

    public Dropdown resolutionDropdown;
    //public DropdownField resolutionField;
    

    void Start() {
        //resolutions = Screen.resolutions;

        ////resolutionDropdown.ClearOptions();

        //List<string> options = new List<string>();

        //int currentResolutionIndex = 0;
        //for (int i = 0; i < resolutions.Length; i++) {
        //    string option = resolutions[i].width + "x" + resolutions[i].height;
        //    options.Add(option);

        //    if (resolutions[i].width == Screen.currentResolution.width &&
        //            resolutions[i].height == Screen.currentResolution.height) {
        //        currentResolutionIndex = i;
        //    }
        //}

        

        //resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        //resolutionDropdown.RefreshShownValue();
    }
    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullscreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }
}
