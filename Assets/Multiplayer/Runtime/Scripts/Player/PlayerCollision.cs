using Unity.Netcode;
using UnityEngine;

public class PlayerCollision : NetworkBehaviour
{
    [SerializeField] private PlayerNetworkManager playerNetworkManager;

    private GameObject col;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner)
        {
            return;
        }

        col = other.gameObject;
        if (col.CompareTag("Fireball"))
        {
            playerNetworkManager.Health.Value -= 0.2f;
        }
    }
}
