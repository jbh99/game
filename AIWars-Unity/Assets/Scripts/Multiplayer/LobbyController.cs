using System.Threading.Tasks;
using UnityEngine;
#if UGS
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
#endif

namespace AIWars.Multiplayer
{
    // Wraps Unity Gaming Services Lobby + Relay. Compile-guarded with `UGS` define.
    public class LobbyController : MonoBehaviour
    {
#if UGS
        Lobby _current;
#endif

        public async Task InitializeAsync()
        {
#if UGS
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
#else
            await Task.Yield();
            Debug.LogWarning("[Lobby] UGS scripting define is OFF.");
#endif
        }

        public async Task<string> CreateLobbyAsync(string lobbyName, int maxPlayers, string mode)
        {
#if UGS
            var alloc = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);

            var options = new CreateLobbyOptions
            {
                IsPrivate = false,
                Data = new System.Collections.Generic.Dictionary<string, DataObject>
                {
                    { "Mode",     new DataObject(DataObject.VisibilityOptions.Public, mode) },
                    { "JoinCode", new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
                }
            };
            _current = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            _ = Heartbeat();
            return joinCode;
#else
            await Task.Yield();
            return null;
#endif
        }

#if UGS
        async Task Heartbeat()
        {
            while (_current != null)
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(_current.Id);
                await Task.Delay(15000);
            }
        }
#endif
    }
}
