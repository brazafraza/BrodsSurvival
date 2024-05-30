using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{
    public AudioClip caveAmb;
    public AudioClip islandAmb;
    public AudioSource audioS;

    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        audioS.PlayOneShot(islandAmb);
    }

    private void Update()
    {
        audioS.PlayOneShot(islandAmb);
    }

    private void OnTriggerEnter(Collider other)
    {
        audioS.PlayOneShot(caveAmb);
    }

    private void OnTriggerExit(Collider other)
    {
        audioS.PlayOneShot(islandAmb);
    }

}
