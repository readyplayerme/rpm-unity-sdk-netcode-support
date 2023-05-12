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

    public class NetworkSelectionScreen : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

        public Action<NetworkType> OnButton;

        private void OnEnable()
        {
            hostButton.onClick.AddListener(OnHostButton);
            clientButton.onClick.AddListener(OnClientButton);
        }

        private void OnDisable()
        {
            hostButton.onClick.RemoveListener(OnHostButton);
            clientButton.onClick.RemoveListener(OnClientButton);
        }

        private void OnClientButton()
        {
            OnButton?.Invoke(NetworkType.Client);
        }

        private void OnHostButton()
        {
            OnButton?.Invoke(NetworkType.Host);
        }
    }
}
