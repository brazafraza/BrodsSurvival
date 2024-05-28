using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public Dropdown graphicsQuality;
    public Slider fieldOfView;
    public Toggle postProcessing;
    public GameMenu gameMenu;
    private void Start()
    {
       

        graphicsQuality.value =(int)Settings.graphicsQuality;
        fieldOfView.value = Settings.fov;
        postProcessing.isOn = Settings.postProcessing;

        ApplyChanges();
    }
    private void Update()
    {
        Settings.graphicsQuality = (Settings.GraphicsQuality)graphicsQuality.value;
        Settings.fov = fieldOfView.value;
        Settings.postProcessing = postProcessing.isOn;

        


    }

    public void Close()
    {
        gameObject.SetActive(false);
        gameMenu.settingsMenuActive = false;
    }

    public void ApplyChanges()
    {
        QualitySettings.SetQualityLevel((int)Settings.graphicsQuality);

    }

}
