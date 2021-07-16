using System;
using MLAPI;
using MLAPI.Messaging;

public class PlayerController : NetworkBehaviour
{
	private GameController _game;

	private void Awake()
	{
		_game = FindObjectOfType<GameController>();
	}

	public void SpawnMonster()
	{
		OnSpawnMonsterServerRpc();
	}

	[ServerRpc]
	private void OnSpawnMonsterServerRpc()
	{
		_game.SpawnMonsterForPlayerServerRpc(NetworkObject.OwnerClientId);
	}
}