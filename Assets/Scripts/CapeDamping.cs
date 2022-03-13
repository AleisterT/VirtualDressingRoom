using System;
using System.Collections;
using System.Collections.Generic;
using AnimeRx;
using UniRx;
using UnityEngine;

public class CapeDamping : MonoBehaviour
{
    [SerializeField] private UserDetectionController userDetectionController;
    [SerializeField] private Cloth cloth;
    [SerializeField] private KinectManager kinectManager;
    private float _initialDamping;

    private IDisposable _firstDisposable;
    private IDisposable _secondDisposable;
    
    // Start is called before the first frame update
    void Awake()
    {
        _initialDamping = cloth.damping;
    }

    private void OnEnable()
    {
        _firstDisposable?.Dispose();
        _secondDisposable?.Dispose();
        cloth.enabled = false;
        cloth.damping = 1;
        _firstDisposable = Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ =>
        {
            cloth.enabled = userDetectionController.NumUsers > 0;
            // cloth.enabled = kinectManager.GetUsersCount() > 0;
            _secondDisposable = Observable.NextFrame().Subscribe(__ =>
            {
                cloth.damping = _initialDamping;
            });
        });
    }

    private void OnDisable()
    {
        _firstDisposable?.Dispose();
        _secondDisposable?.Dispose();
        cloth.enabled = false;
    }
}
