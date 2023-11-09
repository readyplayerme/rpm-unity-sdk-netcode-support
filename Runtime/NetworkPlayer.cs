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
        [SerializeField] private AvatarConfig config;

        public static string InputUrl = string.Empty;
        public NetworkVariable<FixedString64Bytes> avatarUrl = new NetworkVariable<FixedString64Bytes>(writePerm: NetworkVariableWritePermission.Owner);
        public event Action OnPlayerLoadComplete;

        private Animator animator;

        private Transform leftEye;
        private Transform rightEye;

        private SkinnedMeshRenderer[] skinnedMeshRenderers;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            leftEye = AvatarBoneHelper.GetLeftEyeBone(transform, true);
            rightEye = AvatarBoneHelper.GetRightEyeBone(transform, true);

            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
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
                leftEye.transform.localPosition = AvatarBoneHelper.GetLeftEyeBone(args.Avatar.transform, true).localPosition;
                rightEye.transform.localPosition = AvatarBoneHelper.GetRightEyeBone(args.Avatar.transform, true).localPosition;

                AvatarMeshHelper.TransferMesh(args.Avatar, skinnedMeshRenderers, animator);
                Destroy(args.Avatar);
                OnPlayerLoadComplete?.Invoke();
            };
        }
    }
}
