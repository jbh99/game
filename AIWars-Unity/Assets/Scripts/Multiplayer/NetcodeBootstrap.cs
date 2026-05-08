using UnityEngine;
#if NGO
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
#endif

namespace AIWars.Multiplayer
{
    // Wraps Netcode for GameObjects so the project compiles even before NGO is installed.
    // Add the scripting define `NGO` after installing com.unity.netcode.gameobjects.
    public class NetcodeBootstrap : MonoBehaviour
    {
        public static NetcodeBootstrap Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartHost()
        {
#if NGO
            if (NetworkManager.Singleton != null) NetworkManager.Singleton.StartHost();
#else
            Debug.LogWarning("[Net] NGO scripting define is OFF. Install com.unity.netcode.gameobjects and add `NGO` to player settings.");
#endif
        }

        public void StartClient()
        {
#if NGO
            if (NetworkManager.Singleton != null) NetworkManager.Singleton.StartClient();
#else
            Debug.LogWarning("[Net] NGO scripting define is OFF.");
#endif
        }
    }
}
