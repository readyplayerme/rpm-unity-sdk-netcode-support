using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : NetworkBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
    private readonly NetworkVariable<bool> isPlayerWalking =
        new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);

    [SerializeField] private float speed = 6f;
    [FormerlySerializedAs("playerNetworkManager"),SerializeField] private PlayerAvatarLoader playerAvatarLoader;
    [SerializeField] private CharacterController controller;

    private void Update()
    {
        if (playerAvatarLoader.Animator != null)
        {
            playerAvatarLoader.Animator.SetBool(IsWalking, isPlayerWalking.Value);
        }

        if (!IsOwner)
        {
            return;
        }

        if (playerAvatarLoader.Animator == null)
        {
            return;
        }

        var horizontalAxis = Input.GetAxis("Horizontal");
        var direction = new Vector3(horizontalAxis, 0, 0);

        if (direction.magnitude > 0f)
        {
            isPlayerWalking.Value = true;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            var move = direction * (speed * Time.deltaTime);
            controller.Move(new Vector3(move.x, move.y, 0));
        }
        else
        {
            isPlayerWalking.Value = false;
        }
    }
}
