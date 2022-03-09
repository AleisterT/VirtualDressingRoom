using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour
{
    [SerializeField] private RectTransform pointer;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;

    private void Update()
    {
        image.color = RectTransformUtility.RectangleContainsScreenPoint(rectTransform,
            RectTransformUtility.WorldToScreenPoint(null, pointer.position), null) ? Color.green : Color.white;
    }
}
