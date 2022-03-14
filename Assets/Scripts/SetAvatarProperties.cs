using System.IO;
using UnityEngine;

public class SetAvatarProperties : MonoBehaviour
{
    [SerializeField] private bool isFemale;
    [SerializeField] private string scaleFile = "zbroja_skala.txt";
    [SerializeField] private string widthFile = "zbroja_szerokosc.txt";
    [SerializeField] private string verticalOffsetFile = "zbroja_wysokosc.txt";
    [SerializeField] private string armScaleFile = "rece_skala.txt";
    [SerializeField] private AvatarController avatarController;
    [SerializeField] private AvatarScaler avatarScaler;
    string VerticalOffsetFilePath => Path.Combine(Application.dataPath, verticalOffsetFile);
    string ArmScaleFilePath => Path.Combine(Application.dataPath, armScaleFile);
    string ScaleFilePath => Path.Combine(Application.dataPath, scaleFile);
    string WidthFilePath => Path.Combine(Application.dataPath, widthFile);

    void Start()
    {
        if (!string.IsNullOrEmpty(verticalOffsetFile) && File.Exists(VerticalOffsetFilePath))
        {
            var content = File.ReadAllText(VerticalOffsetFilePath);
            bool result = float.TryParse(content, out var offset);
            if (result && avatarController != null)
            {
                avatarController.verticalOffset = offset;
            }
        }
        
        if (!string.IsNullOrEmpty(armScaleFile) && File.Exists(ArmScaleFilePath))
        {
            var content = File.ReadAllText(ArmScaleFilePath);
            bool result = float.TryParse(content, out var armScaleFactor);
            if (result && avatarScaler != null)
            {
                avatarScaler.armScaleFactor = armScaleFactor;
            }
        }
        
        if (!string.IsNullOrEmpty(scaleFile) && File.Exists(ScaleFilePath))
        {
            var content = File.ReadAllText(ScaleFilePath);
            bool result = float.TryParse(content, out var scaleFactor);
            if (result && avatarController != null)
            {
                avatarScaler.bodyScaleFactor = scaleFactor;
                avatarScaler.ScaleAvatar(0,false);
                var sizeController = FindObjectOfType<SizeController>();
                if (isFemale)
                {
                    sizeController.femaleDefaultScale = scaleFactor;
                }
                else
                {
                    sizeController.maleDefaultScale = scaleFactor;
                }
            }
        }
        
        if (!string.IsNullOrEmpty(widthFile) && File.Exists(WidthFilePath))
        {
            var content = File.ReadAllText(WidthFilePath);
            bool result = float.TryParse(content, out var widthFactor);
            if (result && avatarController != null)
            {
                avatarScaler.bodyWidthFactor = widthFactor;
                avatarScaler.ScaleAvatar(0,false);
                var sizeController = FindObjectOfType<SizeController>();
                if (isFemale)
                {
                    sizeController.femaleDefaultWidth = widthFactor;
                }
                else
                {
                    sizeController.maleDefaultWidth = widthFactor;
                }
            }
        }
    }
}
