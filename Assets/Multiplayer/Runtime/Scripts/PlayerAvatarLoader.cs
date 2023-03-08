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
        Animator.runtimeAnimatorController = animatorController;
        
        var leftHand = RecursiveFindChild(avatar.transform, "LeftHand").gameObject;
        leftHand.AddComponent<BoxCollider>().size = colliderSize;
        leftHand.tag = COLLIDER_TAG;

        var rightHand = RecursiveFindChild(avatar.transform, "RightHand").gameObject;
        rightHand.gameObject.AddComponent<BoxCollider>().size = colliderSize;
        rightHand.tag = COLLIDER_TAG;

        Completed?.Invoke();
    }

    private Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            var found = RecursiveFindChild(child, childName);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }
}
