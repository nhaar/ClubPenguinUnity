using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeToGameScale : MonoBehaviour
{
    /// <summary>
    /// A sprite that is supposed to fill the height of the screen
    /// </summary>
    public Sprite ReferenceSprite;

    /// <summary>
    /// Sprite that is being resized
    /// </summary>
    public Sprite CurrentSprite;

    private readonly float _ppu = 100f;

    void Start()
    {
        float scale = GetScale(ReferenceSprite) / _ppu;
        Debug.Log(scale);
        Debug.Log(CurrentSprite.rect.width);
        transform.localScale = new Vector3(scale * CurrentSprite.rect.width, scale * CurrentSprite.rect.height, transform.localScale.z);
        Debug.Log(transform.localScale);
    }

    public static float GetScale(Sprite referenceSprite)
    {
        Debug.Log(Screen.height);
        Debug.Log(referenceSprite.rect.height);
        return Screen.height / referenceSprite.rect.height;
    }
}
