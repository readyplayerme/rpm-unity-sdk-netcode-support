using Multiplayer.Runtime.Scripts;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private readonly NetworkVariable<bool> isPlayerWalking = 
        new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);
    
    [SerializeField] private float speed = 6f;

    [SerializeField] private AvatarModelLoader avatarModelLoader;
    [SerializeField] private CharacterController controller;
    
    private static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));

    private void Update()
    {
        if (avatarModelLoader.Animator != null)
        {
            avatarModelLoader.Animator.SetBool(IsWalking, isPlayerWalking.Value);
        }

        if (!IsOwner)
        {
            return;
        }

        if (avatarModelLoader.Animator == null)
        {
            return;
        }

        var direction = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector3.left;
            isPlayerWalking.Value = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector3.right;
            isPlayerWalking.Value = true;
        }
        else
        {
            isPlayerWalking.Value = false;
        }

        if (direction.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            var move = direction * (speed * Time.deltaTime);
            controller.Move(new Vector3(move.x, move.y, 0));
        }
    }
}
