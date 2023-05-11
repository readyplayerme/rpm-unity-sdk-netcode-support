using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.Multiplayer
{
    public enum NetworkType
    {
        Host,
        Client,
        Server
    }

    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private InputField playerNameField;
        [SerializeField] private GameObject playerNameUI;
        [SerializeField] private GameObject networkUI;

        public Action<NetworkType, string> OnButton;

        private string playerName;

        private void OnEnable()
        {
            startButton.onClick.AddListener(OnStartButton);
            hostButton.onClick.AddListener(OnHostButton);
            clientButton.onClick.AddListener(OnClientButton);
        }

        private void OnDisable()
        {
            hostButton.onClick.RemoveListener(OnHostButton);
            clientButton.onClick.RemoveListener(OnClientButton);
        }

        private void OnStartButton()
        {
            playerNameUI.SetActive(false);
            networkUI.SetActive(true);
            playerName = playerNameField.text;
        }

        private void OnClientButton()
        {
            OnButton?.Invoke(NetworkType.Client, playerName);
            networkUI.SetActive(false);
        }

        private void OnHostButton()
        {
            OnButton?.Invoke(NetworkType.Host, playerName);
            networkUI.SetActive(false);
        }
    }
}
