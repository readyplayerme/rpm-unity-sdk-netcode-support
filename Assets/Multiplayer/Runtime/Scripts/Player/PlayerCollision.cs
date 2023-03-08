using Unity.Netcode;
using UnityEngine;

namespace Multiplayer.Runtime.Scripts
{
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
            if (col.CompareTag("Fireball") && col.GetComponent<Fireball>().player != gameObject)
            {
                playerNetworkManager.Health.Value -= 0.2f;
            }
        }
    }
}
