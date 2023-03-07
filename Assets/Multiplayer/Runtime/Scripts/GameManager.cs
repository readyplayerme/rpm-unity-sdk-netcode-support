using System;
using System.Collections.Generic;
using ReadyPlayerMe;
using ReadyPlayerMe.AvatarCreator;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string AVATAR_CREATOR_EXAMPLE_SCENE = "AvatarCreatorExample";
    private const string GAME_SCENE = "Game";
    private const int MAX_PLAYERS = 2;
    
    [SerializeField] private Loading loading;

    public string AvatarUrl { get; private set; }
    public string PlayerName { get; private set; }

    private MenuUI menuUI;
    private AvatarCreatorManager avatarCreatorManager;

    private NetworkType networkType;

    private static List<Player> registeredPlayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        menuUI = FindObjectOfType<MenuUI>();
        registeredPlayer = new List<Player>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        menuUI.OnButton += LoadScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        menuUI.OnButton -= LoadScene;
    }

    public void AddRegisteredPlayer(Player player)
    {
        registeredPlayer.Add(player);
        FindObjectOfType<CameraController>().players.Add(player.Transform);

        if (registeredPlayer.Count == MAX_PLAYERS)
        {
            loading.SetActive(false);
        }
    }

    private void LoadScene(NetworkType network, string playerName)
    {
        loading.SetActive(true);
        networkType = network;
        PlayerName = playerName;
        SceneManager.LoadScene(AVATAR_CREATOR_EXAMPLE_SCENE);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        switch (arg0.name)
        {
            case AVATAR_CREATOR_EXAMPLE_SCENE:
                loading.SetActive(false);
                avatarCreatorManager = FindObjectOfType<AvatarCreatorManager>();
                avatarCreatorManager.Saved += OnAvatarCreatorManagerSaved;
                break;
            case GAME_SCENE:
                StartNetwork();
                break;
        }
    }

    private void OnAvatarCreatorManagerSaved(string avatarId)
    {
        avatarCreatorManager.Saved -= OnAvatarCreatorManagerSaved;
        AvatarUrl = $"{Endpoints.AVATAR_API_V1}/{avatarId}.glb";
        SceneManager.LoadSceneAsync(GAME_SCENE);
        loading.SetActive(true);
    }

    private void StartNetwork()
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