using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    [SerializeField] private Text text;

    private static DebugText _instance;
    private static DebugText Instance => _instance ? _instance : (_instance = FindObjectOfType<DebugText>());

    public static void Log(object obj) => Instance.text.text = obj.ToString();
}
