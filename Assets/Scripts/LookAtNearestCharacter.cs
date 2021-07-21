using DitzelGames.FastIK;
using UnityEngine;

public class LookAtNearestCharacter : MonoBehaviour
{
    private Character _character;

    void Start()
    {
        _character = GetComponentInParent<Character>();
    }

    void LateUpdate()
    {
        // Find the nearest non-local character
        var nearestOtherCharacter = Scenario.GetNearestOtherCharacter(_character);
        if (!nearestOtherCharacter)
        {
            Debug.LogWarning($"Couldn't find another non-local character to look at");
            return;
        }
        
        // Face this object (probably the head of this character) to face the other character's head
        GetComponent<FastIKLook>().Target = nearestOtherCharacter.Head;
    }
}