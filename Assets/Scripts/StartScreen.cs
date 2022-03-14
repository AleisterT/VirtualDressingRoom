using System;
using System.Linq;
using Windows.Kinect;
using AnimeRx;
using UniRx;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private UserDetectionController userDetectionController;
    [SerializeField] public CanvasGroup canvasGroup;
    [SerializeField] private float animationTime = 0.5f;
    
    private BodyFrameReader _bodyFrameReader = null;
    private Body[] _bodies = new Body[0];
    private IDisposable _fadeInAnimationdDisposable;
    private IDisposable _fadeoutAnimationdDisposable;

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

    private void Awake()
    {
        userDetectionController.UserJoined += NumUserChangedHandler;
        userDetectionController.UserLeft += NumUserChangedHandler;
    }

    private void OnDestroy()
    {
        userDetectionController.UserJoined -= NumUserChangedHandler;
        userDetectionController.UserLeft -= NumUserChangedHandler;
    }

    private void NumUserChangedHandler()
    {
        TryFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        var sensor = KinectSensor.GetDefault();

        if (sensor == null)
        {
            return;
        }

        if (userDetectionController.NumUsers == 0)
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
            TryFadeOut();
        }

    }

    public void TryFadeOut()
    {
        if (_fadeoutAnimationdDisposable != null)
        {
            return;
        }
        _fadeInAnimationdDisposable?.Dispose();
        _fadeoutAnimationdDisposable = Anime.Play(canvasGroup.alpha, 0, Easing.Linear(animationTime))
            .DoOnCompleted(()=>_fadeoutAnimationdDisposable = null)
            .DoOnCancel(()=>_fadeoutAnimationdDisposable = null)
            .DoOnError(e=>_fadeoutAnimationdDisposable = null)
            .Subscribe(a => canvasGroup.alpha = a);
    }

    public  void TryFadeIn()
    {
        if (_fadeInAnimationdDisposable != null)
        {
            return;
        }
        _fadeoutAnimationdDisposable?.Dispose();
        _fadeInAnimationdDisposable = Anime.Play(canvasGroup.alpha, 1, Easing.Linear(animationTime))
            .DoOnCompleted(()=>_fadeInAnimationdDisposable = null)
            .DoOnCancel(()=>_fadeInAnimationdDisposable = null)
            .DoOnError(e=>_fadeInAnimationdDisposable = null)
            .Subscribe(a => canvasGroup.alpha = a);
    }
}
