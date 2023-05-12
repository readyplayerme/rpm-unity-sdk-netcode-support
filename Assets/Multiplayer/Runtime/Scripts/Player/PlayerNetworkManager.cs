using Unity.Netcode;
using UnityEngine;

namespace ReadyPlayerMe.Multiplayer
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        private readonly Vector3 hostPosition = new Vector3(-2.5f, 0, 0);
        private readonly Quaternion hostRotation = Quaternion.Euler(0, 90, 0);

        private readonly Vector3 clientPosition = new Vector3(2.5f, 0, 0);
        private readonly Quaternion clientRotation = Quaternion.Euler(0, -90, 0);

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
                SetPlayerPositionAndRotation();

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
                    SetPlayerPositionAndRotation();

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
            playerData = new PlayerData();
            playerData.Transform = transform;
            playerData.Name = playerNetworkData.Value.Name.ToString();
            playerData.IsPlayer1 = CheckIfPlayer1();

            GameManager.Instance.AddPlayer(playerData);
        }

        private void SetPlayerPositionAndRotation()
        {
            if (IsHost)
            {
                transform.SetPositionAndRotation(hostPosition, hostRotation);
            }
            else
            {
                transform.SetPositionAndRotation(clientPosition, clientRotation);
            }
        }

        private bool CheckIfPlayer1()
        {
            if (IsHost)
            {
                return IsOwner;
            }

            return !IsOwner;
        }
    }
}
