using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.NetcodeSupport.Editor
{
    public static class TransferPrefab
    {
        private const string PREFAB_PATH = "Assets/Ready Player Me/Resources/RPM_Netcode_Character.prefab";

        [MenuItem("Ready Player Me/Transfer Netcode Prefab", priority = 34)]
        public static void Transfer()
        {
            string[] guids = AssetDatabase.FindAssets("t:prefab RPM_Netcode_Character");

            if (guids.Length == 0)
            {
                Debug.Log("RPM_Netcode_Character prefab not found. Please reimport Netcode Support package.");
            }
            else
            {
                if (AssetDatabase.LoadAssetAtPath("Assets/Ready Player Me/Resources/RPM_Character.prefab", typeof(GameObject)))
                {
                    if (!EditorUtility.DisplayDialog("Warning", "RPM_Character prefab already exists. Do you want to overwrite it?", "Yes", "No"))
                    {
                        return;
                    }
                }

                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                AssetDatabase.CopyAsset(path, PREFAB_PATH);
                Selection.activeObject = AssetDatabase.LoadAssetAtPath(PREFAB_PATH, typeof(GameObject));
                Debug.Log($"Netcode prefab transferred to {PREFAB_PATH}");
            }
        }
    }
}
