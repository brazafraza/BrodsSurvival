using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraFOV_Handler : MonoBehaviour
{
    private Camera cam;
    public Weapon weapon;
    public float fovTransitionSpeed = 10f;
    [HideInInspector]public float defaultFOV;

    private void Start()
    {
        cam = GetComponent<Camera>();
        defaultFOV = Settings.fov;
        cam.fieldOfView = defaultFOV;
    }

    private void Update()
    {
        defaultFOV = Settings.fov;
        GetComponent<PostProcessLayer>().enabled = Settings.postProcessing;

        if(weapon != null)
        {
            if (weapon.isAiming)
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, weapon.weaponData.zoomFOV, fovTransitionSpeed * Time.deltaTime);
            else
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, fovTransitionSpeed * Time.deltaTime);
        }
        else
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, fovTransitionSpeed * Time.deltaTime);
    }
}
