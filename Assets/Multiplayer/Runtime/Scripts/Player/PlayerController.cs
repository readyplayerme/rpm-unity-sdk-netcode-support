using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
    private readonly NetworkVariable<bool> isWalking =
        new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAvatarLoader playerAvatarLoader;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform fireballSpawnTransform;

    private void Update()
    {
        if (playerAvatarLoader.Animator != null)
        {
            playerAvatarLoader.Animator.SetBool(IsWalking, isWalking.Value);
        }

        if (!IsOwner)
        {
            return;
        }

        if (PlayerInput.IsHoldingSpace)
        {
            SpawnFireballServerRpc();
        }

        isWalking.Value = playerMovement.ProcessMovement();
    }

    [ServerRpc]
    private void SpawnFireballServerRpc()
    {
        var fireball = Instantiate(fireballPrefab);
        fireball.transform.position = fireballSpawnTransform.position;
        var fireballComponent = fireball.GetComponent<Fireball>();
        fireballComponent.SetDirection(transform.forward);

        fireball.GetComponent<NetworkObject>().Spawn();
    }
}
