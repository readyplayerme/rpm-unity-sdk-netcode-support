using System;
using UnityEngine;
using UnityEngine.UI;

public enum NetworkType
{
    Host,
    Client,
    Server
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button serverButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private GameObject networkUI;
    
    public Action<NetworkType> OnButton;

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
        OnButton?.Invoke(NetworkType.Client);
        networkUI.SetActive(false);
    }

    private void OnServerButton()
    {
        OnButton?.Invoke(NetworkType.Server);
        networkUI.SetActive(false);
    }

    private void OnHostButton()
    {
        OnButton?.Invoke(NetworkType.Host);
        networkUI.SetActive(false);
    }
}
