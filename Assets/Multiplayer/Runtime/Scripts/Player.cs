using System;
using System.Threading.Tasks;
using ReadyPlayerMe.AvatarLoader;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private readonly NetworkVariable<FixedString128Bytes> avatarUrl =
        new NetworkVariable<FixedString128Bytes>(writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<bool> isPlayerWalking = 
        new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);
    
    private readonly Vector3 hostPosition = new Vector3(-4.5f, 0, 0);
    private readonly Vector3 clientPosition = new Vector3(4.5f, 0, 0);

    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 6f;
    [SerializeField] private RuntimeAnimatorController animatorController;

    private static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));

    private Animator animator;
    private Loading loading;

    private GameObject avatar;

    private void Awake()
    {
        loading = FindObjectOfType<Loading>();
    }

    public override async void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            transform.name = "Owner";
            await Task.Delay(TimeSpan.FromSeconds(1));
            avatarUrl.Value = AppManager.AvatarUrl;

            transform.position = IsHost ? hostPosition : clientPosition;
            transform.rotation = Quaternion.Euler(0, 180, 0);

            Load(avatarUrl.Value.ToString());
        }
        else
        {
            transform.name = "Client";
            if (!IsHost)
            {
                Load(avatarUrl.Value.ToString());
            }
           
            avatarUrl.OnValueChanged = (value, newValue) =>
            {
                transform.position = new Vector3(4.5f, 0, 0);

                transform.position = IsHost ? hostPosition : clientPosition;
                transform.rotation = Quaternion.Euler(0, 180, 0);

                Load(newValue.ToString());
            };
        }
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool(IsWalking, isPlayerWalking.Value);
        }

        if (!IsOwner)
        {
            return;
        }

        if (avatar == null)
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

    private void Load(string url)
    {
        Debug.Log(name + ": " + url);
        var avatarObjectLoader = new AvatarObjectLoader();
        avatarObjectLoader.OnCompleted += OnAvatarLoaded;
        avatarObjectLoader.LoadAvatar(url);
    }

    private void OnAvatarLoaded(object sender, CompletionEventArgs e)
    {
        avatar = e.Avatar;
        avatar.transform.SetParent(transform);
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localRotation = Quaternion.identity;

        animator = avatar.GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;
        loading.SetActive(false);
    }
}
