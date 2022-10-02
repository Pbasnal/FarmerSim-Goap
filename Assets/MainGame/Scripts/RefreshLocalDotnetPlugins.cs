using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RefreshLocalDotnetPlugins : MonoBehaviour
{
    [Header("basePath: AstarAlgo/FarmerSim")]
    public List<string> localDotnetPluginsPath;

    public string pluginsFolder;
    private string basePath = $"{Directory.GetCurrentDirectory()}";

    public void RefreshLibraries()
    {
        basePath = $"{Directory.GetCurrentDirectory()}";
        foreach (var libraryPath in localDotnetPluginsPath)
        {
            var pluginDllPath = $"{basePath}/{libraryPath}/bin/Debug/net5.0/";
            var allFiles = Directory.GetFiles(pluginDllPath, "*.*", SearchOption.AllDirectories);

            foreach (var filePath in allFiles)
            {
                Debug.Log($"Copying file: {filePath}");
                File.Copy(filePath, pluginsFolder, true); 
            }
        }
    }
}
