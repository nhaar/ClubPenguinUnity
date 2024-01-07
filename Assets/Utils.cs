using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static float AssertCorrectSign(float number, bool isPositiveNumber)
    {
        if ((number < 0 && isPositiveNumber) || (number > 0 && !isPositiveNumber))
        {
            number *= -1;
        }
        return number;
    }

    public static void ChangePosition(GameObject gameObject, Vector2 position)
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(position.x, position.y);
    }

    public static void RemoveAllChildren(GameObject gameObject)
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}