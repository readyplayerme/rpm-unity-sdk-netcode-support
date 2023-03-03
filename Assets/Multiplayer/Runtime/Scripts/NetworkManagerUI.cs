using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button serverButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private GameObject networkUI;
    [SerializeField] private Loading loading;

    public Action OnButton;

    private void OnEnable()
    {
        hostButton.onClick.AddListener(OnHostButton);
        serverButton.onClick.AddListener(OnServerButton);
        clientButton.onClick.AddListener(OnClientButton);
    }

    private void OnDisable()
    {
        hostButton.onClick.RemoveListener(OnHostButton);
        serverButton.onClick.RemoveListener(OnServerButton);
        clientButton.onClick.RemoveListener(OnClientButton);
    }

    private void OnClientButton()
    {
        NetworkManager.Singleton.StartClient();
        OnButton?.Invoke();
        loading.SetActive(true);
        networkUI.SetActive(false);
    }

    private void OnServerButton()
    {
        NetworkManager.Singleton.StartServer();
        OnButton?.Invoke();
        loading.SetActive(true);
        networkUI.SetActive(false);
    }

    private void OnHostButton()
    {
        NetworkManager.Singleton.StartHost();
        OnButton?.Invoke();
        loading.SetActive(true);
        networkUI.SetActive(false);
    }
}
