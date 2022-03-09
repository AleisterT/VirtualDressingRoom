using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public bool resetOnStay = false;
    public float clickTime = 1f;
    public UnityEvent OnClick;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private bool clicked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var cursor = other.GetComponent<HandCursor>();
        if (cursor == null || !cursor.gameObject.activeInHierarchy || !cursor.IsVisible)
        {
            return;
        }

        cursor.fillImage.fillAmount = 0;
        OnEnter?.Invoke();
        clicked = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (clicked)
        {
            return;
        }
        var cursor = other.GetComponent<HandCursor>();
        if (cursor == null || !cursor.gameObject.activeInHierarchy || !cursor.IsVisible)
        {
            return;
        }
        cursor.fillImage.fillAmount += (Time.deltaTime * (1/clickTime));
        
        if (cursor.fillImage.fillAmount >= 1)
        {
            cursor.fillImage.fillAmount = 0;
            clicked = true;
            OnClick?.Invoke();
            if (resetOnStay)
            {
                OnTriggerEnter2D(other);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var cursor = other.GetComponent<HandCursor>();
        if (cursor == null || !cursor.gameObject.activeInHierarchy || !cursor.IsVisible)
        {
            return;
        }
        cursor.fillImage.fillAmount = 0;
        OnExit?.Invoke();
        clicked = false;
    }
}
