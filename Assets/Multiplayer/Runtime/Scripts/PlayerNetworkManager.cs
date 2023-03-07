using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkManager : NetworkBehaviour
{
    private readonly Vector3 hostPosition = new Vector3(-2.5f, 0, 0);
    private readonly Vector3 clientPosition = new Vector3(2.5f, 0, 0);

    [SerializeField] private PlayerAvatarLoader playerAvatarLoader;

    private readonly NetworkVariable<PlayerData> playerData = new NetworkVariable<PlayerData>(writePerm: NetworkVariableWritePermission.Owner);

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
            playerData.Value = new PlayerData
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
                playerAvatarLoader.Load(playerData.Value.AvatarUrl.ToString());
            }

            playerData.OnValueChanged = (value, newValue) =>
            {
                transform.name = playerData.Value.Name.ToString();
                transform.position = IsHost ? hostPosition : clientPosition;
                transform.rotation = Quaternion.Euler(0, 180, 0);

                playerAvatarLoader.Load(newValue.AvatarUrl.ToString());
            };
        }
    }

    private void OnPlayerLoadComplete()
    {
        Debug.Log("Player " + name + " load complete");
        GameManager.Instance.AddRegisteredPlayer(new Player
        {
            AvatarUrl = playerData.Value.AvatarUrl.ToString(),
            Name = playerData.Value.Name.ToString(),
            IsPlayer1 = (IsHost && IsOwner) || (IsClient && !IsOwner),
            Transform = transform
        });
    }
}
