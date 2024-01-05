using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThinIceArcade : MonoBehaviour
{
    public Image ArcadeShadows;

    public Image ArcadeBase;

    public Image RightButton;

    public Image LeftButton;

    public Image UpButton;

    public Image DownButton;

    /// <summary>
    /// Defines the scale between the size of the exported shapes (which MUST have been all exported with
    /// the same zoom from FFDEC) and the size of the screen
    /// </summary>
    private float _imagesScale;

    /// <summary>
    /// Positions images initially
    /// </summary>
    void Start()
    {
        // "Arcade Shadows" is used as the image that should fill it vertically
        _imagesScale = ScreenResolution.ScreenHeight / ArcadeShadows.sprite.rect.height;

        // list all images so they can all be scaled
        List<Image> images = new List<Image>()
        {
            ArcadeShadows, ArcadeBase, RightButton, LeftButton, UpButton, DownButton,
        };
        foreach (Image image in images)
        {
            ScaleImage(image);
        }

        // left button uses the same sprite as right button but rotated
        MirrorImage(LeftButton);

        // the arcade shadows is nothing but a placeholder for the position of arcade base
        // (probably was changed during development)
        FixImageOnTopOfOther(ArcadeBase, ArcadeShadows, false, false);
        
        
        // using the position of the arcade as reference to place the buttons
        RectTransform arcadeBaseRectTran = ArcadeBase.GetComponent<RectTransform>();
        float middleX = arcadeBaseRectTran.anchoredPosition.x;

        // x coordinate frame is centered on the middle of the arcade base, then converted back
        float sideButtonX = 200 - middleX;
        float sideButtonY = -493;
        ChangePosition(RightButton, middleX + sideButtonX, sideButtonY);
        ChangePosition(LeftButton, middleX - sideButtonX, sideButtonY);

        ChangePosition(DownButton, middleX, -522);
        ChangePosition(UpButton, middleX, -468);
    }

    /// <summary>
    /// Scales image according to the scale defined in _imagesScale
    /// </summary>
    /// <param name="image"></param>
    void ScaleImage(Image image)
    {
        RectTransform transform = image.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(_imagesScale * image.sprite.rect.width, _imagesScale * image.sprite.rect.height);
    }

    /// <summary>
    /// Mirrors an image through the x axis
    /// </summary>
    /// <param name="image"></param>
    void MirrorImage(Image image)
    {
        Vector3 currentScale = image.transform.localScale;
        currentScale.x = -currentScale.x;
        image.transform.localScale = currentScale;
    }

    /// <summary>
    /// Change the position of an image
    /// </summary>
    /// <param name="image"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void ChangePosition(Image image, float x, float y)
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(x, y);
    }

    /// <summary>
    /// Assert that a float has the correct sign
    /// </summary>
    /// <param name="number"></param>
    /// <param name="isPositiveNumber"></param>
    /// <returns></returns>
    float AssertCorrectSign(float number, bool isPositiveNumber)
    {
        if ((number < 0 && isPositiveNumber) || (number > 0 && !isPositiveNumber))
        {
            number *= -1;
        }
        return number;
    }

    /// <summary>
    /// Moves image on top of other image so they share edges (depending on the sign of the displacements)
    /// 
    /// A positive displacement will move the image to the right or up edge of the other image
    /// </summary>
    /// <param name="image"></param>
    /// <param name="other"></param>
    /// <param name="isPositiveXDisplacement"></param>
    /// <param name="isPositiveYDisplacement"></param>
    /// <remarks>
    /// Currently assumes all images are centered initially.
    /// Plan to crop the image edges so they work perfectly.
    /// </remarks>
    void FixImageOnTopOfOther(Image image, Image other, bool isPositiveXDisplacement, bool isPositiveYDisplacement)
    {
        RectTransform imageRectTran = image.GetComponent<RectTransform>();
        RectTransform otherRectTran = other.GetComponent<RectTransform>();

        // divided by 2 to account for the fact that the position is the center of the image
        float deltaX = (imageRectTran.sizeDelta.x - otherRectTran.sizeDelta.x) / 2;
        float deltaY = (imageRectTran.sizeDelta.y - otherRectTran.sizeDelta.y) / 2;

        ChangePosition(image, AssertCorrectSign(deltaX, isPositiveXDisplacement), AssertCorrectSign(deltaY, isPositiveYDisplacement));
    }
}
