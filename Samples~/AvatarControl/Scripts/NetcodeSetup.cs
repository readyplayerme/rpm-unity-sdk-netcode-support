using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace ReadyPlayerMe.NetcodeSupport
{
    public class NetcodeSetup : MonoBehaviour
    {
        [SerializeField] private GameObject connectionPanel;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

        [SerializeField] private GameObject startPanel;
        [SerializeField] private Button startButton;
        [SerializeField] private InputField urlField;

        private void Start()
        {
            hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
            clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
            startButton.onClick.AddListener(() =>
            {
                NetworkPlayer.InputUrl = urlField.text;
                startPanel.SetActive(false);
                connectionPanel.SetActive(true);
            });
            
            NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
            {
                connectionPanel.SetActive(false);
            };
        }
    }
}
