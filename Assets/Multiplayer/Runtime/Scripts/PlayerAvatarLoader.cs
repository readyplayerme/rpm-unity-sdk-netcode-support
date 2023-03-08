using System;
using ReadyPlayerMe.AvatarLoader;
using UnityEngine;

public class PlayerAvatarLoader : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController animatorController;

    public Animator Animator { get; private set; }
    public Action Completed;

    public void Load(string url)
    {
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
