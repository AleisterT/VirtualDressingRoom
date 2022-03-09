using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HeightSetupController : MonoBehaviour
{
    [SerializeField] private KinectManager kinectManager;
    [SerializeField] private InputField inputField;

    private const string SensorHeightFileName = "sensor_height.txt";
    string SensorHeightFilePath => Path.Combine(Application.dataPath, SensorHeightFileName);

    // Start is called before the first frame update
    void Awake()
    {

        bool heightSet = false;
        if (File.Exists(SensorHeightFilePath))
        {
            var content = File.ReadAllText(SensorHeightFilePath);
            bool result = float.TryParse(content, out var heightCm);
            if (result)
            {
                kinectManager.sensorHeight = heightCm / 100f;
                gameObject.SetActive(false);
                heightSet = true;
            }
        }

        if (!heightSet)
        {
            Observable.NextFrame().Subscribe(_ =>
            {
                inputField.Select();
                inputField.ActivateInputField();
            });
        }
        
        
    }

    public void Confirm()
    {
        var result = float.TryParse(inputField.text, out var heightCm);
        if (result)
        {
            kinectManager.sensorHeight = heightCm / 100f;
            gameObject.SetActive(false);
            File.WriteAllText(SensorHeightFilePath,inputField.text);
        }
    }
    
}
