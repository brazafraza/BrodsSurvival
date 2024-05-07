using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavableObject : MonoBehaviour
{
    public int ID;
    [Tooltip("If marked true it will create the object when loading. Use it if this object will be created during runtime.")]
    public bool instantiate;
}
