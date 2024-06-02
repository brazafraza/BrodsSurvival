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
        audioS.PlayOneShot(islandAmb);
    }

    private void Update()
    {
        if (!audioS.isPlaying && !isInCave)
        {
            audioS.PlayOneShot(islandAmb);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ensure the trigger is from the player or intended object
        if (other.CompareTag("Player"))
        {
            if (audioS.isPlaying)
                audioS.Stop();

            Debug.Log("EnteredCave");
            audioS.PlayOneShot(caveAmb);
            isInCave = true;
            //change to cave post process
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Ensure the exit is from the player or intended object
        if (other.CompareTag("Player"))
        {
            if (audioS.isPlaying)
                audioS.Stop();

            audioS.PlayOneShot(islandAmb);
            isInCave = false;
            //change to normal post processing
        }
    }
}
