using System;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace ReadyPlayerMe.NetcodeSupport.Editor
{
    public class PackageImporterListener
    {
        private const string PACKAGE_NAME = "com.readyplayerme.netcodesupport";
public static event Action OnPackageImported;
        
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            Events.registeringPackages += OnPackagesInstalled;
        }

        static void OnPackagesInstalled(PackageRegistrationEventArgs eventArgs)
        {
            var netCodePackage = eventArgs.added.FirstOrDefault(x => x.name == PACKAGE_NAME);
            if(netCodePackage != null)
            {
                Debug.Log("Installed: " + netCodePackage.name);
                OnPackageImported?.Invoke();
                // AnalyticsEditorLogger.EventLogger.LogPackageInstalled(new PackageCoreInfo()
                // {
                //     Name = netCodePackage.name, 
                //     Id = netCodePackage.packageId
                // });
            }
        }
    }
}
