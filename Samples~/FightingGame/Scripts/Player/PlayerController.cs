using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class PlayerController : NetworkBehaviour
    {
        private static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));

        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private Transform fireballSpawnTransform;

        private bool isWalking;

        private void Update()
        {
            if (IsOwner)
            {
                animator.SetBool(IsWalking, isWalking);

                if (PlayerInput.IsHoldingSpace)
                {
                    SpawnFireballServerRpc();
                }

                isWalking = playerMovement.ProcessMovement();
            }
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
}
