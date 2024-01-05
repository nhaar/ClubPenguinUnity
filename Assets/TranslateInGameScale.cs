using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateInGameScale : MonoBehaviour
{
    public Sprite ReferenceSprite;

    public Sprite CurrentSprite;

    public float TranslateX;

    public float TranslateY;

    void Start()
    {
        float scale = ResizeToGameScale.GetScale(ReferenceSprite);

        float movementX = (TranslateX - 0.5f) * ReferenceSprite.rect.width + 0.5f * CurrentSprite.rect.width;
        float movementY = (0.5f - TranslateY) * ReferenceSprite.rect.height - 0.5f * CurrentSprite.rect.height;
        transform.Translate(new Vector3(movementX * scale, movementY * scale, 0));
    }
}
