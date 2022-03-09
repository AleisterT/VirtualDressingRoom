using System;
using UnityEngine;

public class UserDetectionController : MonoBehaviour
{
    private int _previousNumUsers = 0;

    public event Action UserJoined;
    public event Action UserLeft;

    void Update()
    {
        var currentNumUsers = KinectManager.Instance.GetUsersCount();

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