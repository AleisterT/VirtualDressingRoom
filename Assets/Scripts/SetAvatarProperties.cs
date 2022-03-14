using System.IO;
using UnityEngine;

public class SetAvatarProperties : MonoBehaviour
{
    [SerializeField] private string verticalOffsetFile = "zbroja_wysokosc.txt";
    [SerializeField] private string armScaleFile = "rece_skala.txt";
    [SerializeField] private AvatarController avatarController;
    [SerializeField] private AvatarScaler avatarScaler;
    string VerticalOffsetFilePath => Path.Combine(Application.dataPath, verticalOffsetFile);
    string ArmScaleFilePath => Path.Combine(Application.dataPath, armScaleFile);

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
    }
}
