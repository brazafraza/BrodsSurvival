using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbob : MonoBehaviour
{
    private Player player;
    private Animator anim;

    private bool walking;
    private bool running;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        walking = player.walking;
        running = player.running;

        UpdateHeadbob();
            
    }
    private void UpdateHeadbob()
    {
        anim.SetBool("Walk", walking);
        anim.SetBool("Run", running);
    }


}
