using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Gather Data", menuName = "Char/Gathering/Gather Data")]

public class GatherDataSO : ScriptableObject
{
    public ItemSO item;
    public int amount;
}
