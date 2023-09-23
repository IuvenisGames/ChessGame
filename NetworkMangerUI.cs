using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkMangerUI : MonoBehaviour
{
    public Button server;
    public Button client;
    public Button host;
    
    private void Awake()
    {
        server.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        host.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        client.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
