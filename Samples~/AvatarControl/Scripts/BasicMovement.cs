using UnityEngine;
using Unity.Netcode;

namespace ReadyPlayerMe.NetcodeSupport
{
    [RequireComponent(typeof(NetworkObject))]
    public class BasicMovement : NetworkBehaviour
    {
        [SerializeField] private new GameObject camera;
        
        private Animator animator;
        
        private readonly static int WALK_ANIM = Animator.StringToHash("Walking");

        private void Start()
        {
            animator = GetComponent<Animator>();
            
            if (IsOwner) camera.SetActive(true);
        }
        
        private void Update()
        {
            if (IsOwner)
            {
                var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
                var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
            
                transform.Rotate(0, x, 0);
                transform.Translate(0, 0, z);

                animator.SetBool(WALK_ANIM, z != 0);
            }
        }
    }
}
