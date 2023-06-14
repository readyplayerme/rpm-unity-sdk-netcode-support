using Unity.Netcode;
using UnityEngine;

namespace ReadyPlayerMe.Multiplayer
{
    [RequireComponent(typeof(Animator))]
    public class PlayerNetworkAnimator : MonoBehaviour
    {
        private static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        [ServerRpc]
        public void HandleAnimationServerRpc(bool isWalking)
        {
            animator.SetBool(IsWalking, isWalking);
        }
    }
}
