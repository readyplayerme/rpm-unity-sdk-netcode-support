using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace ReadyPlayerMe.Multiplayer
{
    public class NetworkPlayer : NetworkBehaviour
    {
        private readonly NetworkVariable<FixedString128Bytes> avatarUrl =
            new NetworkVariable<FixedString128Bytes>(writePerm: NetworkVariableWritePermission.Owner);

        [SerializeField] private PlayerAvatarLoader playerAvatarLoader;

        public Action OnPLayerLoadComplete;

        private void OnEnable()
        {
            playerAvatarLoader.Completed += OnPlayerLoadComplete;
        }

        private void OnDisable()
        {
            playerAvatarLoader.Completed -= OnPlayerLoadComplete;
        }

        public void LoadAvatar(string url)
        {
            avatarUrl.Value = url;
            playerAvatarLoader.Load(avatarUrl.Value.ToString());
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                return;
            }
            
            if (avatarUrl.Value.IsEmpty)
            {
                avatarUrl.OnValueChanged = (value, newValue) =>
                {
                    playerAvatarLoader.Load(avatarUrl.Value.ToString());
                };
                return;
            }

            playerAvatarLoader.Load(avatarUrl.Value.ToString());
        }

        private void OnPlayerLoadComplete()
        {
            OnPLayerLoadComplete?.Invoke();
        }
    }
}
