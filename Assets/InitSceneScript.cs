using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Kinect;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AwaitSensorAsync();
    }

    private async Task AwaitSensorAsync()
    {

        bool isCorrectRes = Screen.width == 1080 && Screen.height == 1920;
        while (!isCorrectRes)
        {
            Screen.SetResolution(1080,1920,FullScreenMode.ExclusiveFullScreen);
            isCorrectRes = Screen.width == 1080 && Screen.height == 1920;
            await Task.Delay(1000);
        }

        var dllFilePath = $"{Application.dataPath}/../KinectUnityAddin.dll";
        if (!File.Exists(dllFilePath))
        {
            SceneManager.LoadScene(1);
            return;
        }
        KinectSensor sensor = null;
        do
        {
            sensor = KinectSensor.GetDefault();
            await Task.Delay(100);
        } while (sensor == null);
        
        sensor.Open();
        while (!sensor.IsAvailable)
        {
            await Task.Delay(100);
        }
        
        while (!sensor.IsOpen)
        {
            await Task.Delay(100);
        }

        SceneManager.LoadScene(1);
    }
}
