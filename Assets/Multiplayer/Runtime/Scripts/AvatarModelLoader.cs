using System;
using System.Threading.Tasks;
using ReadyPlayerMe.AvatarLoader;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer.Runtime.Scripts
{
    public class AvatarModelLoader: NetworkBehaviour
    {
        private readonly Vector3 hostPosition = new Vector3(-4.5f, 0, 0);
        private readonly Vector3 clientPosition = new Vector3(4.5f, 0, 0);
        
        [SerializeField] private RuntimeAnimatorController animatorController;

        private readonly NetworkVariable<FixedString128Bytes> avatarUrl =
            new NetworkVariable<FixedString128Bytes>(writePerm: NetworkVariableWritePermission.Owner);
        
        public Animator Animator => animator;
        
        private Animator animator;
        private GameObject avatar;
        
        public override async void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                transform.name = "Owner";
                await Task.Delay(TimeSpan.FromSeconds(1));
                avatarUrl.Value = AppManager.AvatarUrl;

                transform.position = IsHost ? hostPosition : clientPosition;
                transform.rotation = Quaternion.Euler(0, 180, 0);

                Load(avatarUrl.Value.ToString());
            }
            else
            {
                transform.name = "Client";
                if (!IsHost)
                {
                    Load(avatarUrl.Value.ToString());
                }
           
                avatarUrl.OnValueChanged = (value, newValue) =>
                {
                    transform.position = new Vector3(4.5f, 0, 0);

                    transform.position = IsHost ? hostPosition : clientPosition;
                    transform.rotation = Quaternion.Euler(0, 180, 0);

                    Load(newValue.ToString());
                };
            }
        }

        private void Load(string url)
        {
            Debug.Log(name + " : " + url);
            var avatarObjectLoader = new AvatarObjectLoader();
            avatarObjectLoader.OnCompleted += OnAvatarLoaded;
            avatarObjectLoader.LoadAvatar(url);
        }

        private void OnAvatarLoaded(object sender, CompletionEventArgs e)
        {
            avatar = e.Avatar;
            avatar.transform.SetParent(transform);
            avatar.transform.localPosition = Vector3.zero;
            avatar.transform.localRotation = Quaternion.identity;

            animator = avatar.GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
        }
    }
}
