using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyFullscreen : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
    }
}
