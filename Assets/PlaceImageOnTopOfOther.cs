using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceImageOnTopOfOther : MonoBehaviour
{
    public Image OtherImage;

    public bool GlueRightEdge;

    public bool GlueTopEdge;

    void Start()
    {
        RectTransform imageRectTran = gameObject.GetComponent<RectTransform>();
        RectTransform otherRectTran = OtherImage.GetComponent<RectTransform>();

        // divided by 2 to account for the fact that the position is the center of the image
        Vector2 delta = (imageRectTran.sizeDelta - otherRectTran.sizeDelta) / 2;

        Vector2 position = new Vector2(Utils.AssertCorrectSign(delta.x, !GlueRightEdge), Utils.AssertCorrectSign(delta.y, !GlueTopEdge));
        Utils.ChangePosition(gameObject, position);
    }
}
