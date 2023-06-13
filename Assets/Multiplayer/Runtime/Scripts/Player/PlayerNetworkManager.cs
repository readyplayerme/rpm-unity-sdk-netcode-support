using Unity.Collections;
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

        private readonly NetworkVariable<FixedString128Bytes> playerName =
            new NetworkVariable<FixedString128Bytes>(writePerm: NetworkVariableWritePermission.Owner);
        public readonly NetworkVariable<float> Health
            = new NetworkVariable<float>(1, writePerm: NetworkVariableWritePermission.Owner);

        [SerializeField] private NetworkPlayer networkPlayer;
        private PlayerData playerData;

        private void OnEnable()
        {
            networkPlayer.OnPLayerLoadComplete += OnPlayerLoadComplete;
        }

        private void OnDisable()
        {
            networkPlayer.OnPLayerLoadComplete -= OnPlayerLoadComplete;
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                networkPlayer.LoadAvatar(GameManager.Instance.AvatarUrl);
                playerName.Value = GameManager.Instance.PlayerName;
                transform.name = GameManager.Instance.PlayerName;
                SetPlayerPositionAndRotation();
            }
            else
            {
                playerName.OnValueChanged = (value, newValue) =>
                {
                    transform.name = playerName.Value.ToString();
                    SetPlayerPositionAndRotation();
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
            playerData.Name = playerName.Value.ToString();
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
