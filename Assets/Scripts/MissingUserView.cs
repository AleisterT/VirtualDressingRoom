using System;
using System.Collections;
using System.Collections.Generic;
using AnimeRx;
using UniRx;
using UnityEngine;

public class MissingUserView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private UserDetectionController userDetectionController;
    [SerializeField] private float animationTime = 0.5f;

    private CompositeDisposable _disposables = new CompositeDisposable();
    private IDisposable _animationdDisposable = null;

    private void OnEnable()
    {
        Observable.FromEvent(h => userDetectionController.UserJoined += h, h => userDetectionController.UserJoined -= h)
            .Subscribe(UserJoinedHandler)
            .AddTo(_disposables);
        
        Observable.FromEvent(h => userDetectionController.UserLeft += h, h => userDetectionController.UserLeft -= h)
            .Subscribe(UserLeftHandler)
            .AddTo(_disposables);
    }

    private void OnDisable()
    {
        _disposables.Clear();
        _animationdDisposable?.Dispose();
    }

    private void UserJoinedHandler(Unit u)
    {
        _animationdDisposable?.Dispose();
        _animationdDisposable = Anime.Play(canvasGroup.alpha, 0, Easing.Linear(animationTime))
            .Subscribe(a => canvasGroup.alpha = a);
    }

    private void UserLeftHandler(Unit u)
    {
        _animationdDisposable?.Dispose();
        _animationdDisposable = Anime.Play(canvasGroup.alpha, 1, Easing.Linear(animationTime))
            .Subscribe(a => canvasGroup.alpha = a);
    }
}