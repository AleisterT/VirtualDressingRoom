using System;
using System.Linq;
using Windows.Kinect;
using UnityEngine;

public class UserDetectionController : MonoBehaviour
{
    public int NumUsers => _previousNumUsers;
    private int _previousNumUsers = 0;

    public event Action UserJoined;
    public event Action UserLeft;
    
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

    void Update()
    {
        UpdateWithCache();
    }

    private void UpdateWithCache()
    {
        if (_previousNumUsers > 0 && KinectManager.Instance.GetUsersCount() == 0)
        {
            UserLeft?.Invoke();
            _previousNumUsers = 0;
            return;
        }
        
        var sensor = KinectSensor.GetDefault();

        if (sensor == null)
        {
            if (_previousNumUsers > 0 && KinectManager.Instance.GetUsersCount() == 0)
            {
                UserLeft?.Invoke();
                _previousNumUsers = 0;
            }
            return;
        }

        var newBodyFrame = BodyReader?.AcquireLatestFrame();
        if (newBodyFrame != null)
        {
            var numBodies = BodyReader.BodyFrameSource.BodyCount;
            if (_bodies.Length < numBodies)
            {
                _bodies = new Body[numBodies];

            }
        
            newBodyFrame.GetAndRefreshBodyData(_bodies);
            newBodyFrame.Dispose();
            newBodyFrame = null;
        
            if (_bodies.Length == 0)
            {
                if (_previousNumUsers > 0 && KinectManager.Instance.GetUsersCount() == 0)
                {
                    UserLeft?.Invoke();
                    _previousNumUsers = 0;
                }
                return;
            }
        
            var body = _bodies.FirstOrDefault(b=>b.IsTracked);

            if (body == null)
            {
                if (_previousNumUsers > 0 && KinectManager.Instance.GetUsersCount() == 0)
                {
                    UserLeft?.Invoke();
                    _previousNumUsers = 0;
                }
                return;
            }
            
            if (_previousNumUsers == 0 && KinectManager.Instance.GetUsersCount() > 0)
            {
                UserJoined?.Invoke();
                _previousNumUsers = 1;
            }
        }
    }

    private void UpdateNoCache()
    {
        var sensor = KinectSensor.GetDefault();

        if (sensor == null)
        {
            if (_previousNumUsers > 0)
            {
                UserLeft?.Invoke();
                _previousNumUsers = 0;
            }
            return;
        }

        var bodyFrame = BodyReader?.AcquireLatestFrame();
        
        if (bodyFrame == null)
        {
            if (_previousNumUsers > 0)
            {
                UserLeft?.Invoke();
                _previousNumUsers = 0;
            }
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
            if (_previousNumUsers > 0)
            {
                UserLeft?.Invoke();
                _previousNumUsers = 0;
            }
            return;
        }
        
        var body = _bodies.FirstOrDefault(b=>b.IsTracked);

        if (body == null)
        {
            if (_previousNumUsers > 0)
            {
                UserLeft?.Invoke();
                _previousNumUsers = 0;
            }
            return;
        }

        if (_previousNumUsers == 0)
        {
            UserJoined?.Invoke();
            _previousNumUsers = 1;
        }
    }
}