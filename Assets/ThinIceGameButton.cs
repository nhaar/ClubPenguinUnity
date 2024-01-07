using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThinIceGameButton : MonoBehaviour
{
    public Sprite Button;

    public Sprite Hovered;

    public Sprite BaseImage;

    public string Text;

    public int FontSize;

    public Font Font;

    private void Start()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { OnPointerExit(); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnter(); });
        trigger.triggers.Add(entry);

        gameObject.AddComponent<ThinIceImage>();
        gameObject.GetComponent<ThinIceImage>().SourceImage = Button;
        gameObject.GetComponent<ThinIceImage>().BaseImage = BaseImage;

        GameObject textObject = new GameObject("ButtonText");
        textObject.AddComponent<Text>();
        textObject.GetComponent<Text>().text = Text;
        textObject.GetComponent<Text>().font = Font;
        textObject.GetComponent<Text>().fontSize = FontSize;
        textObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        textObject.transform.SetParent(gameObject.transform);
        textObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void AddClickFunction(Action callback)
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { callback(); });
        trigger.triggers.Add(entry);
    }

    public void OnPointerEnter()
    {
        GetComponent<Image>().sprite = Hovered;
    }

    public void OnPointerExit()
    {
        GetComponent<Image>().sprite = Button;
    }
}
