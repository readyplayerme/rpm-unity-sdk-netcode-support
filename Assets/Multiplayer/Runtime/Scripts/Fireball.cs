using Unity.Netcode;
using UnityEngine;

public class Fireball : NetworkBehaviour
{
    [SerializeField] private float speed = 1;

    public GameObject player;
    
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {
        if (IsHost)
        {
            Invoke(nameof(Destroy), 3f);
        }
    }

    public void Update()
    {
        transform.Translate(direction * speed);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        CancelInvoke();
    }

    private void Destroy()
    {
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
}
