using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    public float drink = 20f;

    public void Drink(PlayerStats stats)
    {
        stats.thirst += drink;
    }
}
