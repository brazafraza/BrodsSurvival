using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public enum GraphicsQuality { VeryLow, Low, Medium, High, VeryHigh, Ultra }

    public static GraphicsQuality graphicsQuality = GraphicsQuality.Ultra;

    public static float fov = 60f;

    public static bool postProcessing = true;
}
