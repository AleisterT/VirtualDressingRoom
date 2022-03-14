using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeController : MonoBehaviour
{
    [SerializeField] public float maleDefaultScale = 1.05f;
    [SerializeField] public float femaleDefaultScale = 1.1f;
    [SerializeField] public float maleDefaultWidth = 1.05f;
    [SerializeField] public float femaleDefaultWidth = 1.1f;
    
    [SerializeField] private AvatarScaler maleScaler;
    [SerializeField] private AvatarScaler femaleScaler;
    [SerializeField] private List<Cloth> cloths;
    [SerializeField] private UserDetectionController userDetectionController;
    
    public float min = 0.5f;
    public float max = 1.5f;
    public float changeFactor = 0.1f;

    private void Awake()
    {
        userDetectionController.UserJoined += Reset;
        userDetectionController.UserLeft += Reset;
    }

    private void OnDestroy()
    {
        userDetectionController.UserJoined -= Reset;
        userDetectionController.UserLeft -= Reset;
    }

    public void IncreaseScale()
    {
        maleScaler.bodyScaleFactor = Mathf.Clamp(maleScaler.bodyScaleFactor + changeFactor, min, max);
        femaleScaler.bodyScaleFactor = Mathf.Clamp(maleScaler.bodyScaleFactor + changeFactor, min, max);
        maleScaler.ScaleAvatar(0,false);
        femaleScaler.ScaleAvatar(0,false);
    }
    
    public void DecreaseScale()
    {
        maleScaler.bodyScaleFactor = Mathf.Clamp(maleScaler.bodyScaleFactor - changeFactor, min, max);
        femaleScaler.bodyScaleFactor = Mathf.Clamp(maleScaler.bodyScaleFactor - changeFactor, min, max);
        maleScaler.ScaleAvatar(0,false);
        femaleScaler.ScaleAvatar(0,false);
    }
    
    public void IncreaseWidth()
    {
        maleScaler.bodyWidthFactor = Mathf.Clamp(maleScaler.bodyWidthFactor + changeFactor, min, max);
        femaleScaler.bodyWidthFactor = Mathf.Clamp(maleScaler.bodyWidthFactor + changeFactor, min, max);
        maleScaler.ScaleAvatar(0,false);
        femaleScaler.ScaleAvatar(0,false);
    }
    
    public void DecreaseWidth()
    {
        maleScaler.bodyWidthFactor = Mathf.Clamp(maleScaler.bodyWidthFactor - changeFactor, min, max);
        femaleScaler.bodyWidthFactor = Mathf.Clamp(maleScaler.bodyWidthFactor - changeFactor, min, max);
        maleScaler.ScaleAvatar(0,false);
        femaleScaler.ScaleAvatar(0,false);
    }

    public void Reset()
    {
        maleScaler.bodyScaleFactor = maleDefaultScale;
        maleScaler.bodyWidthFactor = maleDefaultWidth;
        femaleScaler.bodyScaleFactor = femaleDefaultScale;
        femaleScaler.bodyWidthFactor = femaleDefaultWidth;
        maleScaler.ScaleAvatar(0,false);
        femaleScaler.ScaleAvatar(0,false);

        foreach (var cloth in cloths)
        {
            cloth.ClearTransformMotion();
        }
    }
}
