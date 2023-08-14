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
        public event Action OnPLayerLoadComplete;

        private Animator animator;

        private Transform leftEye;
        private Transform rightEye;

        private SkinnedMeshRenderer[] skinnedMeshRenderers;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            leftEye = transform.Find(FULL_BODY_LEFT_EYE_BONE_NAME);
            rightEye = transform.Find(FULL_BODY_RIGHT_EYE_BONE_NAME);

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
                leftEye.transform.localPosition = args.Avatar.transform.Find(FULL_BODY_LEFT_EYE_BONE_NAME).localPosition;
                rightEye.transform.localPosition = args.Avatar.transform.Find(FULL_BODY_RIGHT_EYE_BONE_NAME).localPosition;

                TransferMesh(args.Avatar);
            };
        }

        //TODO: Multiple mesh transfer support.
        private void TransferMesh(GameObject source)
        {
            var sourceAnimator = source.GetComponentInChildren<Animator>();
            SkinnedMeshRenderer[] sourceMeshes = source.GetComponentsInChildren<SkinnedMeshRenderer>();

            for (var i = 0; i < sourceMeshes.Length; i++)
            {
                Mesh mesh = sourceMeshes[i].sharedMesh;
                skinnedMeshRenderers[i].sharedMesh = mesh;

                Material[] materials = sourceMeshes[i].sharedMaterials;
                skinnedMeshRenderers[i].sharedMaterials = materials;
            }

            Avatar avatar = sourceAnimator.avatar;
            animator.avatar = avatar;
            OnPLayerLoadComplete?.Invoke();
            Destroy(source);
        }
    }
}
