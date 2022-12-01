using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;

public class PullCommonFiles : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        var inPackages = importedAssets.Any(path => path.StartsWith("Packages/com.totem.totemcore")) ||
            deletedAssets.Any(path => path.StartsWith("Packages/com.totem.totemcore")) ||
            movedAssets.Any(path => path.StartsWith("Packages/com.totem.totemcore")) ||
            movedFromAssetPaths.Any(path => path.StartsWith("Packages/com.totem.totemcore"));

        if (inPackages)
        {
            //InitializeOnLoad();
        }
    }

    //[InitializeOnLoadMethod]
    private static void InitializeOnLoad()
    {
        var listRequest = Client.List(true);
        while (!listRequest.IsCompleted)
            Thread.Sleep(100);

        if (listRequest.Error != null)
        {
            Debug.Log("Error: " + listRequest.Error.message);
            return;
        }

        string pathToResources = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Resources");
        if (!Directory.Exists(pathToResources))
        {
            Directory.CreateDirectory(pathToResources);
        }
        string pathToCommonFilesFolder = Path.Combine(pathToResources, "totem-common-files");
        if (Directory.Exists(pathToCommonFilesFolder))
        {
            return;
        }

        string gitCommand = "git";
        string gitPullArgument = @$"clone https://github.com/Totem-gdn/totem-common-files.git {pathToCommonFilesFolder}";

        var process = System.Diagnostics.Process.Start(gitCommand, gitPullArgument);

        Debug.Log("Common files pulled");
    }
}