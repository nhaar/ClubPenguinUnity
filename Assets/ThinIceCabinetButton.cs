using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class controls the "physical" arcade buttons
/// </summary>
public class ThinIceCabinetButton : MonoBehaviour
{
    /// <summary>
    /// Sprite for the button when it's not pressed
    /// </summary>
    public Sprite Still;

    /// <summary>
    /// Sprite for the button when it's pressed
    /// </summary>
    public Sprite Pressed;

    /// <summary>
    /// Key that controls the button
    /// </summary>
    public KeyCode Key;

    public Vector2 Position;

    public bool UseArcadeCenter;

    public Sprite BaseImage;

    /// <summary>
    /// Represents whether or not the button is "physically" pressed
    /// </summary>
    private bool _pressed;

    private ThinIceImage _thinIceImage;

    private Image _image;

    /// <summary>
    /// Represents the difference between the size of the button when it's pressed and when it's not
    /// </summary>
    private Vector2 _delta;

    void Start()
    {
        _thinIceImage = gameObject.AddComponent<ThinIceImage>();

        _pressed = false;
        _delta = Still.rect.size - Pressed.rect.size;
        _thinIceImage.SourceImage = Still;
        _thinIceImage.BaseImage = BaseImage;

        RectTransform rectTransform = gameObject.transform.parent.GetComponent<RectTransform>();
        float center = UseArcadeCenter ? rectTransform.anchoredPosition.x : 0;
        Utils.ChangePosition(gameObject, Position + new Vector2(center, 0));
    }

    void Update()
    {
        bool pressed = Input.GetKey(Key);
        // changes the sprite and recenters whenever the button pressed state is updated
        if (_pressed != pressed)
        {
            _pressed = !_pressed;
            DisplaceButton(_pressed);
            gameObject.GetComponent<Image>().sprite = _pressed ? Pressed : Still;
        }
    }

    /// <summary>
    /// Recenters the button so that the bottom edge of the button is always at the same position
    /// </summary>
    /// <param name="isOriginallyStill"></param>
    void DisplaceButton(bool isOriginallyStill)
    {
        int sign = isOriginallyStill ? 1 : -1;

        // displacement is half due to position being center-centric
        // signs are just to make sure the button moves in the right direction in each axis
        // and gets properly reversed
        Vector3 displacement = new Vector3(_delta.x, -_delta.y, 0) / 2 * sign;
        transform.position = transform.position + displacement;
    }
}
