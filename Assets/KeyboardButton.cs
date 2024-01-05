using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardButton : MonoBehaviour
{
    public Image ButtonImage;

    public Sprite StillButton;

    public Sprite PressedButton;

    public KeyCode Key;

    void Update()
    {
        ButtonImage.sprite = Input.GetKey(Key) == true ? PressedButton : StillButton;
    }
}
