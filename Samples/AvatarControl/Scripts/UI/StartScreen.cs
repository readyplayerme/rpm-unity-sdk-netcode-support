using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.Multiplayer
{
    public class StartScreen : MonoBehaviour
    {
        [SerializeField] private InputField playerName;
        [SerializeField] private InputField avatarUrl;
        [SerializeField] private Button startButton;

        public Action<string, string> OnStart;

        private void Update()
        {
            if (string.IsNullOrEmpty(playerName.text))
            {
                startButton.interactable = false;
                return;
            }

            if (!string.IsNullOrEmpty(avatarUrl.text) && Uri.IsWellFormedUriString(avatarUrl.text, UriKind.Absolute))
            {
                startButton.interactable = true;
            }
            else
            {
                startButton.interactable = false;
            }
        }

        private void OnEnable()
        {
            startButton.onClick.AddListener(OnStartButton);
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveListener(OnStartButton);
        }

        private void OnStartButton()
        {
            OnStart?.Invoke(playerName.text, avatarUrl.text);
        }
    }
}
