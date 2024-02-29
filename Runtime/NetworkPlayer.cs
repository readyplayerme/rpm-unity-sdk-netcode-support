using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using ReadyPlayerMe.Core;

namespace ReadyPlayerMe.NetcodeSupport
{
    /// <summary>
    /// Used on Ready Player Me 
    /// </summary>
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkPlayer : NetworkBehaviour
    {
        private const string FULL_BODY_LEFT_EYE_BONE_NAME = "Armature/Hips/Spine/Spine1/Spine2/Neck/Head/LeftEye";
        private const string FULL_BODY_RIGHT_EYE_BONE_NAME = "Armature/Hips/Spine/Spine1/Spine2/Neck/Head/RightEye";

        [SerializeField] private AvatarConfig config;

        public static string InputUrl = string.Empty;
        public NetworkVariable<FixedString64Bytes> avatarUrl = new NetworkVariable<FixedString64Bytes>(writePerm: NetworkVariableWritePermission.Owner);
        public event Action OnPlayerLoadComplete;
        
        private Transform leftEye;
        private Transform rightEye;

        private void Awake()
        {
            leftEye = transform.Find(FULL_BODY_LEFT_EYE_BONE_NAME);
            rightEye = transform.Find(FULL_BODY_RIGHT_EYE_BONE_NAME);
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                avatarUrl.Value = InputUrl;
                avatarUrl.OnValueChanged += (value, newValue) =>
                {
                    LoadAvatar(newValue.ToString());
                };

                LoadAvatar(InputUrl);
            }
            else if (Uri.IsWellFormedUriString(avatarUrl.Value.ToString(), UriKind.Absolute))
            {
                LoadAvatar(avatarUrl.Value.ToString());
            }

            avatarUrl.OnValueChanged += (value, newValue) =>
            {
                LoadAvatar(newValue.ToString());
            };
        }

        private void LoadAvatar(string url)
        {
            var loader = new AvatarObjectLoader();
            loader.LoadAvatar(url);
            loader.AvatarConfig = config;
            loader.OnCompleted += (sender, args) =>
            {
                leftEye.transform.localPosition = args.Avatar.transform.Find(FULL_BODY_LEFT_EYE_BONE_NAME).localPosition;
                rightEye.transform.localPosition = args.Avatar.transform.Find(FULL_BODY_RIGHT_EYE_BONE_NAME).localPosition;

                AvatarMeshHelper.TransferMesh(args.Avatar, gameObject);
                Destroy(args.Avatar);
                OnPlayerLoadComplete?.Invoke();
            };
        }
    }
}
