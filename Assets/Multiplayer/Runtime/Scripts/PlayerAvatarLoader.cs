using System;
using ReadyPlayerMe.AvatarLoader;
using UnityEngine;

public class PlayerAvatarLoader : MonoBehaviour
{
    private const string COLLIDER_TAG = "Hands";

    [SerializeField] private RuntimeAnimatorController animatorController;

    private readonly Vector3 colliderSize = new Vector3(0.1f, 0.25f, 0.1f);

    public Animator Animator { get; private set; }
    public Action Completed;

    public void Load(string url)
    {
        Debug.Log(name + " : " + url);
        var avatarObjectLoader = new AvatarObjectLoader();
        avatarObjectLoader.OnCompleted += OnAvatarLoaded;
        avatarObjectLoader.LoadAvatar(url);
    }

    private void OnAvatarLoaded(object sender, CompletionEventArgs e)
    {
        var avatar = e.Avatar;
        avatar.transform.SetParent(transform);
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localRotation = Quaternion.identity;

        Animator = avatar.GetComponent<Animator>();
        Animator.applyRootMotion = false;
        Animator.runtimeAnimatorController = animatorController;

        Completed?.Invoke();
    }
}
