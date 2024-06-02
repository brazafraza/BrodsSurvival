using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{
    public AudioClip caveAmb;
    public AudioClip islandAmb;
    public AudioSource audioS;

    public bool isInCave = false;

    // Start is called before the first frame update
    void Start()
    {
       // audioS = GetComponent<AudioSource>();
        audioS.PlayOneShot(islandAmb);
    }

    private void Update()
    {
        if(!audioS.isPlaying)
            audioS.PlayOneShot(islandAmb);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (audioS.isPlaying)
            audioS.Stop();
        Debug.Log("EnteredCave");
        audioS.PlayOneShot(caveAmb);
        isInCave = true;
        //change to cave post process
    }

    private void OnTriggerExit(Collider other)
    {
        audioS.PlayOneShot(islandAmb);
        isInCave = false;
        //change to normal post processing
    }

}
