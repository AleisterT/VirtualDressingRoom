using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Editor
{
    public class PostprocessCopyConfigs
    {
        [PostProcessBuild(0)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
            CopyConfigs(pathToBuiltProject);
        }

        private static void CopyConfigs(string pathToBuiltProject)
        {
            var configsDir = $"{Application.dataPath}/../Configs";
            var targetDir = $"{Path.GetDirectoryName(pathToBuiltProject)}/Wirtualne lustro_Data";

            var configs = Directory.GetFiles(configsDir);

            foreach (var configPath in configs)
            {
                var destFilePath = $"{targetDir}/{Path.GetFileName(configPath)}";
                File.Copy(configPath,destFilePath);
            }
        }
    }
}