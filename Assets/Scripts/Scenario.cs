using System.Collections.Generic;
using System.Linq;
using MLAPI;
using UnityEngine;

public class Scenario : NetworkBehaviour
{
	public static IEnumerable<Character> Characters => FindObjectsOfType<Character>();

	public static Character GetNearestOtherCharacter(Character thisCharacter)
	{
		return Characters
			.Where(character => character.NetworkObject != thisCharacter.NetworkObject)
			.OrderBy(otherCharacter => Vector3.Distance(thisCharacter.Position, otherCharacter.Position))
			.FirstOrDefault();
	}
}