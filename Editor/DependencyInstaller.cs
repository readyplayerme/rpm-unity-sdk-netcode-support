using ReadyPlayerMe.Core.Editor;
using UnityEditor;

public abstract class DependencyInstaller
{
    private const string NETCODE_SUPPORT_PACKAGE_NAME = "com.readyplayerme.netcodesupport";
    private const string NETCODE_HELPER_PACKAGE = "https://github.com/Unity-Technologies/com.unity.multiplayer.samples.coop.git?path=/Packages/com.unity.multiplayer.samples.coop#main";
    private const string DIALOG_MESSAGE = "This package depends on - com.unity.multiplayer.samples.coop. Do you want to install it?";

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        PackageManagerEventListener.OnPackageImported += OnPackageImported;
    }

    ~DependencyInstaller()
    {
        PackageManagerEventListener.OnPackageImported -= OnPackageImported;
    }

    private static void OnPackageImported(string packageName)
    {
        if (packageName != NETCODE_SUPPORT_PACKAGE_NAME)
        {
            return;
        }

        if (EditorUtility.DisplayDialog("Ready Player Me", DIALOG_MESSAGE, "OK", "Cancel"))
        {
            if (!PackageManagerHelper.IsPackageInstalled(NETCODE_HELPER_PACKAGE))
            {
                PackageManagerHelper.AddPackage(NETCODE_HELPER_PACKAGE);
            }
        }
    }
}
