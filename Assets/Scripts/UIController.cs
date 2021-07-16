using UnityEngine;

public class UIController : MonoBehaviour
{
	public void SpawnMonsterClicked()
	{
		GameController.LocalPlayerController.SpawnMonster();
	}
}
