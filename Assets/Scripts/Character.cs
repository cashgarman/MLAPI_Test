using System.Linq;
using MLAPI;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Character : NetworkBehaviour
{
    [SerializeField] private Transform _head;
    
    private GameObject _thirdPersonCharacterObject;

    public static Character LocalCharacter => FindObjectsOfType<Character>().FirstOrDefault(character => character.IsLocalPlayer);
    public Vector3 Position => _thirdPersonCharacterObject.transform.position;
    public Transform Head => _head;

    void Start()
    {
        // Grab the third person character for this character
        _thirdPersonCharacterObject = GetComponentInChildren<Animator>().gameObject;
        Debug.Log($"Found 3rd person character object for {NetworkObject.OwnerClientId} client character: {_thirdPersonCharacterObject}");
        
        // If this isn't the local player
        if (!IsLocalPlayer)
        {
            // Remove the camera
            Destroy(GetComponentInChildren<Camera>().gameObject);
            
            // Disable the player controls for this character
            GetComponentInChildren<ThirdPersonUserControl>().enabled = false;
            GetComponentInChildren<ThirdPersonCharacter>().enabled = false;
        }
    }
}
