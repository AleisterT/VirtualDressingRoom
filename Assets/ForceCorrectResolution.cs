using System;
using System.Threading.Tasks;
using UnityEngine;

public class ForceCorrectResolution : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private float _lastCheckTime = 0;
    private void Update()
    {
        var timePassed = Time.realtimeSinceStartup - _lastCheckTime;
        if (timePassed < 1f)
        {
            return;
        }
        _lastCheckTime = Time.realtimeSinceStartup;
        bool isCorrectRes = Screen.width == 1080 && Screen.height == 1920;
        if (!isCorrectRes)
        {
            Screen.SetResolution(1080,1920,FullScreenMode.ExclusiveFullScreen);
        }
    }
}