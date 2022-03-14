using System;
using System.Linq;
using UnityEngine;

public class UserDetectionController : MonoBehaviour
{
    private int _previousNumUsers = 0;
    public int NumUsers => KinectManager.Instance.avatarControllers[0].playerId > 0 ? 1 : 0;

    public event Action UserJoined;
    public event Action UserLeft;

    void Update()
    {
        var currentNumUsers = KinectManager.Instance.avatarControllers[0].playerId > 0 ? 1 : 0;

        if (currentNumUsers > 0 && _previousNumUsers == 0)
        {
            UserJoined?.Invoke();
        }

        if (currentNumUsers == 0 && _previousNumUsers > 0)
        {
            UserLeft?.Invoke();
        }

        _previousNumUsers = currentNumUsers;
    }
}