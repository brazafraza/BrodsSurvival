using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class PostProcessingHandler : MonoBehaviour
{
    private Water water;
    public Water Water => water;
    private PostProcessVolume volume;

    public PostProcessProfile normalPostProcessing;
    public PostProcessProfile waterPostProcessing;
    public PostProcessProfile cavePostProcessing;

    public Ambience amb;

    private void Start()
    {
        volume = GetComponent<PostProcessVolume>();
    }

    private void Update()
    {
        if (amb.isInCave)
        {
            volume.profile = cavePostProcessing;
            return;
        }
        if (water == null)
            volume.profile = normalPostProcessing;
        else
            volume.profile = waterPostProcessing;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Water>() != null)
            water = other.GetComponent<Water>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Water>() != null)
            if (water == other.GetComponent<Water>())
                water = null;
    }

}
