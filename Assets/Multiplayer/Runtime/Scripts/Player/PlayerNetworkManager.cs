using Unity.Netcode;
using UnityEngine;

namespace ReadyPlayerMe.Multiplayer
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        private readonly Vector3 hostPosition = new Vector3(-2.5f, 0, 0);
        private readonly Vector3 clientPosition = new Vector3(2.5f, 0, 0);

        private readonly NetworkVariable<PlayerNetworkData> playerNetworkData =
            new NetworkVariable<PlayerNetworkData>(writePerm: NetworkVariableWritePermission.Owner);
        public readonly NetworkVariable<float> Health
            = new NetworkVariable<float>(1, writePerm: NetworkVariableWritePermission.Owner);

        [SerializeField] private PlayerAvatarLoader playerAvatarLoader;

        private PlayerData playerData;

        private void OnEnable()
        {
            playerAvatarLoader.Completed += OnPlayerLoadComplete;
        }

        private void OnDisable()
        {
            playerAvatarLoader.Completed -= OnPlayerLoadComplete;
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                playerNetworkData.Value = new PlayerNetworkData
                {
                    AvatarUrl = GameManager.Instance.AvatarUrl,
                    Name = GameManager.Instance.PlayerName
                };

                transform.name = GameManager.Instance.PlayerName;
                transform.position = IsHost ? hostPosition : clientPosition;
                transform.rotation = Quaternion.Euler(0, 180, 0);

                playerAvatarLoader.Load(GameManager.Instance.AvatarUrl);
            }
            else
            {
                if (!IsHost)
                {
                    playerAvatarLoader.Load(playerNetworkData.Value.AvatarUrl.ToString());
                }

                playerNetworkData.OnValueChanged = (value, newValue) =>
                {
                    transform.name = playerNetworkData.Value.Name.ToString();
                    transform.position = IsHost ? hostPosition : clientPosition;
                    transform.rotation = Quaternion.Euler(0, 180, 0);

                    playerAvatarLoader.Load(newValue.AvatarUrl.ToString());
                };
            }

            Health.OnValueChanged = (value, newValue) =>
            {
                playerData.Damage(newValue);
            };
        }

        public override void OnNetworkDespawn()
        {
            GameManager.Instance.RemovePlayer(playerData);
        }

        private void OnPlayerLoadComplete()
        {
            playerData = gameObject.AddComponent<PlayerData>();
            playerData.AvatarUrl = playerNetworkData.Value.AvatarUrl.ToString();
            playerData.Name = playerNetworkData.Value.Name.ToString();
            if (IsHost)
            {
                playerData.IsPlayer1 = IsOwner;
            }
            else
            {
                playerData.IsPlayer1 = !IsOwner;
            }

            GameManager.Instance.AddPlayer(playerData);
        }
    }
}
