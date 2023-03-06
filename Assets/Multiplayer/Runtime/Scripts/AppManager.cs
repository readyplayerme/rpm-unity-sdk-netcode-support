using System;
using ReadyPlayerMe;
using ReadyPlayerMe.AvatarCreator;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    private const string AVATAR_CREATOR_EXAMPLE = "AvatarCreatorExample";

    [SerializeField] private Loading loading;
    
    public static string AvatarUrl;

    private UIManager uiManager;
    private AvatarCreatorManager avatarCreatorManager;

    private NetworkType networkType;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        uiManager.OnButton += LoadScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        uiManager.OnButton -= LoadScene;
    }

    private void LoadScene(NetworkType network)
    {
        loading.SetActive(true);
        networkType = network;
        SceneManager.LoadScene(AVATAR_CREATOR_EXAMPLE);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == AVATAR_CREATOR_EXAMPLE)
        {
            loading.SetActive(false);
            avatarCreatorManager = FindObjectOfType<AvatarCreatorManager>();
            avatarCreatorManager.Saved += OnAvatarCreatorManagerSaved;
        }
        else if (arg0.name == "Game")
        {
            switch (networkType)
            {
                case NetworkType.Host:
                    NetworkManager.Singleton.StartHost();
                    break;
                case NetworkType.Client:
                    NetworkManager.Singleton.StartClient();
                    break;
                case NetworkType.Server:
                    NetworkManager.Singleton.StartServer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void OnAvatarCreatorManagerSaved(string avatarId)
    {
        avatarCreatorManager.Saved -= OnAvatarCreatorManagerSaved;
        AvatarUrl = $"{Endpoints.AVATAR_API_V1}/{avatarId}.glb";
       SceneManager.LoadSceneAsync("Game");
    }
}
