using System.Text;
using MLAPI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private GameObject _menuPanel;
	[SerializeField] private GameObject _menuCamera;

	public void OnHostGameClicked()
	{
		NetworkManager.Singleton.ConnectionApprovalCallback += (connectionData, clientID, callback) =>
		{
			var password = Encoding.ASCII.GetString(connectionData);
			Debug.Log($"Got a connection from client with ID {clientID} with password: {password}");
			callback(true, null, password == "HackThePlanet", Vector3.zero, Quaternion.identity);
		};
		NetworkManager.Singleton.StartHost();
		_menuPanel.SetActive(false);
		_menuCamera.SetActive(false);
	}
	
	public void OnJoinGameClicked()
	{
		NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes("HackThePlanet");
		NetworkManager.Singleton.StartClient();
		_menuPanel.SetActive(false);
		_menuCamera.SetActive(false);
	}
}
