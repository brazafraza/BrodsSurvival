using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Transform sun;
    


    [Header("Cycle")]
    public float timeOfDay = 1350f;
    public float cycleDuration = 2700f;
    public float dayStartTime = 750f;
    public float dayEndTime = 2150f;
    [Space]
    public float cycleSpeed = 1f;
    [Header("Lighting")]
    public float dayTimeSunIntensity = 1f;
    public float nightTimeSunIntensity = 0f;
    [Space]
    public float dayTimeAmbienIntensity = 1f;
    public float nightTimeAmbienIntensity = 0.15f;
    [Space]
    [Space]
    public Material skyboxDay;
    public Material skyboxNight;
    public Color dayTimeColor;
    public Color nightTimeColor;
    public float intensityChangeSpeed = 1f;

    [HideInInspector] public bool isNightTime;
    [HideInInspector] public float transitionDuration = 10f;

    private void Start()
    {
        if (!isNightTime)
            sun.GetComponentInChildren<Light>().intensity = dayTimeSunIntensity;
        else
            sun.GetComponentInChildren<Light>().intensity = nightTimeSunIntensity;
    }

    private void Update()
    {
        if (!isNightTime)
        {
            sun.GetComponentInChildren<Light>().intensity = Mathf.Lerp(sun.GetComponentInChildren<Light>().intensity, dayTimeSunIntensity, intensityChangeSpeed * Time.deltaTime);
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, dayTimeAmbienIntensity, intensityChangeSpeed * Time.deltaTime);
            //set fog colour
            RenderSettings.fogDensity = 0.00045f;
            RenderSettings.fogColor = Color.grey;
            if (skyboxDay != null)
            {
                // StartCoroutine(TransitionSkyboxDay());
                RenderSettings.skybox = skyboxDay;

            }
        }
    
        else
        {
            sun.GetComponentInChildren<Light>().intensity = Mathf.Lerp(sun.GetComponentInChildren<Light>().intensity, nightTimeSunIntensity, intensityChangeSpeed * Time.deltaTime);
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, nightTimeAmbienIntensity, intensityChangeSpeed * Time.deltaTime);
            
            RenderSettings.fogDensity = 0.1f;
            RenderSettings.fogColor = Color.black;
            if (skyboxNight != null)
            {

                // StartCoroutine(TransitionSkyboxNight());
                RenderSettings.skybox = skyboxNight;

              
            }
        }

        if (timeOfDay > cycleDuration)
            timeOfDay = 0;

        if (timeOfDay > dayStartTime && timeOfDay < dayEndTime)
            timeOfDay += cycleSpeed * Time.deltaTime;
        else
            timeOfDay += (cycleSpeed * 2) * Time.deltaTime;

        UpdateLighting();

    }

    //IEnumerator TransitionSkyboxDay()
    //{
    //    float elapsedTime = 750f;

    //    while (elapsedTime < transitionDuration)
    //    {
    //        float t = elapsedTime / transitionDuration;
    //        RenderSettings.skybox.Lerp(skyboxNight, skyboxDay, t);

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // Ensure that the transition ends with the day skybox
    //    RenderSettings.skybox = skyboxDay;
    //}

    //IEnumerator TransitionSkyboxNight()
    //{
    //    float elapsedTime = 2150f;

    //    while (elapsedTime < transitionDuration)
    //    {
    //        float t = elapsedTime / transitionDuration;
    //        RenderSettings.skybox.Lerp(skyboxNight, skyboxDay, t);

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // Ensure that the transition ends with the night skybox
    //    RenderSettings.skybox = skyboxNight;
    //}


    public void UpdateLighting()
    {
        sun.localRotation = Quaternion.Euler((timeOfDay * 360 / cycleDuration), 0, 0);
        if (timeOfDay < dayStartTime || timeOfDay > dayEndTime)
            isNightTime = true;
        else
            isNightTime = false;
    }
}
