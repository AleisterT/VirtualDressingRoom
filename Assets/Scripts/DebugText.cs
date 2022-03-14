using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    [SerializeField] private Text text;

    private static DebugText _instance;

    private static DebugText Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DebugText>();
                _instance.gameObject.SetActive(Application.isEditor || Debug.isDebugBuild);
            }
            return _instance;
        }
    }

    public static void Log(object obj)
    {
        if (Application.isEditor || Debug.isDebugBuild)
        {
            Instance.text.text = obj.ToString();
        }
    }

    private void Awake()
    {
        _instance = this;
        gameObject.SetActive(Application.isEditor || Debug.isDebugBuild);
    }
}
