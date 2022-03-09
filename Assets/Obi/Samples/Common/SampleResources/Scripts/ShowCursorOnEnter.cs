using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursorOnEnter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var cursor = other.GetComponent<HandCursor>();
        if (cursor == null)
        {
            return;
        }
        cursor.Show();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var cursor = other.GetComponent<HandCursor>();
        if (cursor == null)
        {
            return;
        }
        cursor.Hide();
    }
}
