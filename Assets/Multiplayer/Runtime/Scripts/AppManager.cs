using ReadyPlayerMe;
using ReadyPlayerMe.AvatarCreator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    [SerializeField] private Button start;

    public static string AvatarUrl;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        start.onClick.AddListener(OnStart);
    }

    private void OnStart()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(1);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "AvatarCreatorExample")
        {
            var avatar = FindObjectOfType<AvatarCreatorManager>();
            avatar.Saved += OnAvatarSaved;
        }
    }

    private void OnAvatarSaved(string avatarId)
    {
        AvatarUrl = $"{Endpoints.AVATAR_API_V1}/{avatarId}.glb";
        SceneManager.LoadScene(2);
    }
}
