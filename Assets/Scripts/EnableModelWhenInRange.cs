using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class EnableModelWhenInRange : MonoBehaviour
{
    [SerializeField] private HandCursor cursor;
    [SerializeField] private UserDetectionController userDetectionController;
    [SerializeField] private List<GameObject> elements;
    // Start is called before the first frame update
    void Start()
    {
        userDetectionController.UserJoined += UserJoinedHandler;
        userDetectionController.UserLeft += UserLeftHandler;
        Observable.NextFrame().Subscribe(_ =>
        {
            cursor.Hide();
            SetElementsActive(false);
        });
    }

    private void OnDestroy()
    {
        userDetectionController.UserJoined -= UserJoinedHandler;
        userDetectionController.UserLeft -= UserLeftHandler;
    }

    private void UserJoinedHandler()
    {
        SetElementsActive(true);
    }

    private void UserLeftHandler()
    {
        SetElementsActive(false);
    }

    private void SetElementsActive(bool active)
    {
        foreach (var element in elements)
        {
            element.SetActive(active);
        }
    }
}
