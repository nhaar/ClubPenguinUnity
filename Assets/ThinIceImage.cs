using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThinIceImage : MonoBehaviour
{
    public Sprite SourceImage;

    public Sprite BaseImage;

    void Start()
    {
        Image image = gameObject.AddComponent<Image>();
        image.sprite = SourceImage;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(SourceImage.rect.width, SourceImage.rect.height) * ThinIceBaseImage.GetScale(BaseImage);
    }

    public void ChangeImage(Sprite sprite)
    {
        Image image = GetComponent<Image>();
        if (image == null)
        {
            SourceImage = sprite;
        }
        else
        {
            image.sprite = sprite;
        }
    }
}
