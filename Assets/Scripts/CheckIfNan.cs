using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfNan : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (float.IsNaN(target.anchoredPosition.x))
        {
            Debug.LogError(Time.time);
            Debug.Break();
        }
    }
}
