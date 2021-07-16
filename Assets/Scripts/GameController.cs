using System.Collections;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    [SerializeField] private Monster _monsterPrefab;

    public static PlayerController LocalPlayerController => FindObjectsOfType<PlayerController>()
        .FirstOrDefault(player => player.IsLocalPlayer);

    [ServerRpc]
    public void SpawnMonsterForPlayerServerRpc(ulong clientID)
    {
    	Debug.Log($"Player {clientID} is trying to spawn a monster (still on the server)");
        
        // Find a free spawn point
        var freeSpawnPoint = FindObjectsOfType<SpawnPoint>()
            .FirstOrDefault(spawnPoint => spawnPoint.transform.childCount == 0);
        if (!freeSpawnPoint)
        {
            Debug.LogError($"Couldn't find a free spawn point to spawn a monster for client {clientID}");
            return;
        }
        
        // Spawn the monster for all clients
        var spawnedMonster = Instantiate(_monsterPrefab, freeSpawnPoint.transform.position, freeSpawnPoint.transform.rotation, freeSpawnPoint.transform);
        spawnedMonster.NetworkObject.Spawn();
        
        // Change the ownership of the spawned monster to the spawning client
        // HACK: Apparently you can't change ownership of a spawned object immediately (https://stackoverflow.com/questions/67491276/unity-mlapis-networkobject-changeowner-gives-keynotfoundexception)
        StartCoroutine(ChangeObjectOwnership(spawnedMonster.NetworkObject, clientID));

        Debug.Log($"Spawned monster {spawnedMonster.name} and gave ownership to {clientID}");
    }

    private IEnumerator ChangeObjectOwnership(NetworkObject networkObject, ulong clientID)
    {
        yield return new WaitForEndOfFrame();
        networkObject.ChangeOwnership(clientID);
    }
}