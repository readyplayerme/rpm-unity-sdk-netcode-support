using UnityEngine;

namespace Multiplayer.Runtime.Scripts
{
    public class PlayerCollision : MonoBehaviour
    {
        [SerializeField] private PlayerNetworkManager playerNetworkManager;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Hands"))
            {
                playerNetworkManager.Health.Value -= 0.2f;
            }
        }
    }
}
