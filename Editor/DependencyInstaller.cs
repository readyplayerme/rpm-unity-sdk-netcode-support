using ReadyPlayerMe.Core.Editor;
using ReadyPlayerMe.NetcodeSupport.Editor;
using UnityEditor;

public class DependencyInstaller
{
    [InitializeOnLoadMethod]
    static void Initialize()
    {
        PackageImporterListener.OnPackageImported += OnPackageImported;
    }

    ~DependencyInstaller()
    {
        PackageImporterListener.OnPackageImported -= OnPackageImported;
    }

    private static void OnPackageImported()
    {
        if (EditorUtility.DisplayDialog("Ready Player Me", "This package depends on - com.unity.multiplayer.samples.coop. Do you want to install it?", "OK", "Cancel"))
        {
            PackageManagerHelper.AddPackage("https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop.git?path=/Packages/com.unity.multiplayer.samples.coop#main");
        }
    }
}
