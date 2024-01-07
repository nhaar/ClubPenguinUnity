using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFramerate : MonoBehaviour
{
    public int Framerate = 30;

    void Start()
    {
        Application.targetFrameRate = Framerate;
    }
}
