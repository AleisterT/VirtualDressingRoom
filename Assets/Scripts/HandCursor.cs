using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.Kinect;
using UnityEngine;
using UnityEngine.UI;

public class HandCursor : MonoBehaviour
{
    [SerializeField] private UserDetectionController userDetectionController;
    [SerializeField] private RectTransform cursor;
    [SerializeField] public Image fillImage;
    private BodyFrameReader _bodyFrameReader = null;
    private Body[] _bodies = new Body[0];
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

    private void OnDestroy()
    {
        userDetectionController.UserLeft -= UserLeftHandler;
        userDetectionController.UserJoined -= UserJoinedHandler;

        _bodyFrameReader?.Dispose();
    }

    void Update()
    {
        if (FindObjectOfType<StartScreen>().canvasGroup.alpha > 0)
        {
            cursor.anchoredPosition = new Vector2(3000,3000);
            return;
        }
        
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

        var colorCameraHandPos = sensor.CoordinateMapper.MapCameraPointToColorSpace(hand.Position);

        _maxX = Mathf.Max(_maxX, colorCameraHandPos.X);
        _minX = Mathf.Min(_minX, colorCameraHandPos.X);
        
        _maxY = Mathf.Max(_maxY, colorCameraHandPos.Y);
        _minY = Mathf.Min(_minY, colorCameraHandPos.Y);

        var desiredPosition = new Vector2(colorCameraHandPos.X,-colorCameraHandPos.Y);

        if (IsValid(desiredPosition.x) && IsValid(desiredPosition.y))
        {
            cursor.anchoredPosition = desiredPosition;
        }
    }

    private bool IsValid(float x) => !float.IsNaN(x) && !float.IsInfinity(x);

    private float _maxX, _maxY, _minX, _minY;

    private void Awake()
    {
        userDetectionController.UserLeft += UserLeftHandler;
        userDetectionController.UserJoined += UserJoinedHandler;
    }

    private void UserJoinedHandler()
    {
    }

    private void UserLeftHandler()
    {
        cursor.anchoredPosition = new Vector2(3000,3000);
    }

    private void OnDisable()
    {
        Debug.Log($"{_maxX} {_minX} {_maxY} {_minY}");
    }

    public bool IsVisible => GetComponentsInChildren<Image>().Any(image => image.enabled);

    public void Show()
    {
        foreach (var image in GetComponentsInChildren<Image>())
        {
            image.enabled = true;
        }
    }
    
    public void Hide()
    {
        foreach (var image in GetComponentsInChildren<Image>())
        {
            image.enabled = false;
        }
    }
}
