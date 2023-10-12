using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private const string GAME_SCENE = "FightingGame";
        private const int MIN_PLAYERS = 2;

        [SerializeField] private Loading loading;

        public string AvatarUrl { get; private set; }
        public string PlayerName { get; private set; }

        private NetworkSelectionScreen networkSelectionScreen;
        private StartScreen startScreen;
        private HUD hud;
        private CameraController cameraController;

        private NetworkType networkType;

        private static List<PlayerData> players;

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

            startScreen = FindObjectOfType<StartScreen>();
            networkSelectionScreen = FindObjectOfType<NetworkSelectionScreen>();

            players = new List<PlayerData>();
        }

        private void OnEnable()
        {
            startScreen.OnStart += OnStart;
            networkSelectionScreen.OnButton += LoadScene;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            startScreen.OnStart -= OnStart;
            networkSelectionScreen.OnButton -= LoadScene;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void AddPlayer(PlayerData playerData)
        {
            players.Add(playerData);
            if (cameraController == null)
            {
                cameraController = FindObjectOfType<CameraController>();
            }
            cameraController.players.Add(playerData.Transform);
            if (hud == null)
            {
                hud = FindObjectOfType<HUD>();
            }
            hud.SetPlayerName(playerData.IsPlayer1, playerData.Name);
            playerData.HealthChanged += health => hud.SetHealth(playerData.IsPlayer1, health);

            if (players.Count >= MIN_PLAYERS)
            {
                loading.SetActive(false);
            }
        }

        public void RemovePlayer(PlayerData playerData)
        {
            players.Remove(playerData);
            cameraController.players.Remove(playerData.Transform);
        }

        private void OnStart(string playerName, string avatarUrl)
        {
            PlayerName = playerName;
            AvatarUrl = avatarUrl;
            startScreen.gameObject.SetActive(false);
            networkSelectionScreen.gameObject.SetActive(true);
            NetworkPlayer.InputUrl = avatarUrl;
        }

        private void LoadScene(NetworkType network)
        {
            loading.SetActive(true);
            networkSelectionScreen.gameObject.SetActive(false);
            networkType = network;
            SceneManager.LoadScene(GAME_SCENE);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == GAME_SCENE)
            {
                loading.SetActive(false);
                StartNetwork();
            }
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
}
