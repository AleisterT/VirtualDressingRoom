using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.Kinect;
using AnimeRx;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] public CanvasGroup canvasGroup;
    [SerializeField] private float animationTime = 0.5f;
    
    private BodyFrameReader _bodyFrameReader = null;
    private Body[] _bodies = new Body[0];
    private IDisposable _animationdDisposable;

    private BodyFrameReader BodyReader
    {
        get
        {
            if (_bodyFrameReader == null)
            {
                var device = KinectSensor.GetDefault();
                if (device == null)
                {
                    return null;
                }
                _bodyFrameReader = device.BodyFrameSource.OpenReader();
            }
            return _bodyFrameReader;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var sensor = KinectSensor.GetDefault();

        if (sensor == null)
        {
            return;
        }

        var bodyFrame = BodyReader?.AcquireLatestFrame();
        
        if (bodyFrame == null)
        {
            return;
        }

        var numBodies = BodyReader.BodyFrameSource.BodyCount;
        if (_bodies.Length < numBodies)
        {
            _bodies = new Body[numBodies];

        }
        
        bodyFrame.GetAndRefreshBodyData(_bodies);
        bodyFrame.Dispose();
        bodyFrame = null;
        
        if (_bodies.Length == 0)
        {
            return;
        }
        
        var body = _bodies.FirstOrDefault(b=>b.IsTracked);

        if (body == null)
        {
            return;
        }
        
        var hand = body.Joints[JointType.HandTipRight];
        var head = body.Joints[JointType.Head];
        
        var colorCameraHandPos = sensor.CoordinateMapper.MapCameraPointToColorSpace(hand.Position);
        var colorCameraHeadPos = sensor.CoordinateMapper.MapCameraPointToColorSpace(head.Position);

        if (colorCameraHandPos.Y < colorCameraHeadPos.Y && canvasGroup.alpha >= 1)
        {
            FadeOut();
        }

    }

    public void FadeOut()
    {
        _animationdDisposable?.Dispose();
        _animationdDisposable = Anime.Play(canvasGroup.alpha, 0, Easing.Linear(animationTime))
            .Subscribe(a => canvasGroup.alpha = a);
    }

    public  void FadeIn()
    {
        _animationdDisposable?.Dispose();
        _animationdDisposable = Anime.Play(canvasGroup.alpha, 1, Easing.Linear(animationTime))
            .Subscribe(a => canvasGroup.alpha = a);
    }
}
