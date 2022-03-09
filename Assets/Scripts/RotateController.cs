using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RotateController : MonoBehaviour
{
    [SerializeField] private KinectManager kinectManager;
    [SerializeField] private Image timer;
    [SerializeField] private int rotateModeTimeSeconds = 10;
    
    private IDisposable _timerDisposable = null;

    private void Awake()
    {
        DisableRotationHandler();
    }

    public void StartRotationMode()
    {
        _timerDisposable?.Dispose();
        timer.gameObject.SetActive(true);
        timer.fillAmount = 1;
        kinectManager.allowTurnArounds = true;

        _timerDisposable = Observable.Interval(TimeSpan.FromSeconds(1)).Take(rotateModeTimeSeconds + 1)
            .DoOnCancel(DisableRotationHandler)
            .DoOnCompleted(DisableRotationHandler)
            .Subscribe(i =>
            {
                timer.fillAmount = Mathf.Clamp01((rotateModeTimeSeconds - i) / (float) rotateModeTimeSeconds);
            });
    }

    private void DisableRotationHandler()
    {
        if (timer != null)
        {
            timer.gameObject.SetActive(false);
        }
        kinectManager.allowTurnArounds = false;
    }

    private void OnDestroy()
    {
        _timerDisposable?.Dispose();
    }
}
