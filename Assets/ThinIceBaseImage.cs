using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThinIceBaseImage : Image
{
    public float ImagesScale { get; private set; }

    public static float GetScale(Sprite baseImage)
    {
        return ScreenResolution.ScreenHeight / baseImage.rect.height;
    }

    protected override void Start()
    {
        base.Start();
        RectTransform rectTransform = GetComponent<RectTransform>();
        ImagesScale = ScreenResolution.ScreenHeight / sprite.rect.height;
        rectTransform.sizeDelta = new Vector2(sprite.rect.width * ImagesScale, ScreenResolution.ScreenHeight);
    }
}
