using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 6f;

    private bool isWalking;

    public bool ProcessMovement()
    {
        var direction = new Vector3(PlayerInput.HorizontalAxis, 0, 0);

        if (direction.magnitude > 0f)
        {
            isWalking = true;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            var move = direction * (speed * Time.deltaTime);
            controller.Move(new Vector3(move.x, 0, 0));
        }
        else
        {
            isWalking = false;
        }

        return isWalking;
    }
}
