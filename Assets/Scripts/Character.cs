using System;
using System.Linq;
using DitzelGames.FastIK;
using MLAPI;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Character : NetworkBehaviour
{
    [SerializeField] private Transform _head;
    
    private GameObject _thirdPersonCharacterObject;
    [SerializeField] private FastIKFabric _rightHandIK;
    [SerializeField] private FastIKFabric _leftHandIK;
    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _leftHand;

    public static Character LocalCharacter => FindObjectsOfType<Character>().FirstOrDefault(character => character.IsLocalPlayer);
    public Vector3 Position => _thirdPersonCharacterObject.transform.position;
    public Transform Head => _head;
    public Transform RightHand => _rightHand;
    public Transform LeftHand => _rightHand;

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

        if (IsServer)
        {
            //Debug.Log($"Spawning the left and right hand IK targets");
            //_leftHandIK.Target.Value.GetComponent<NetworkObject>().Spawn();
            //_rightHandIK.Target.Value.GetComponent<NetworkObject>().Spawn();
        }
    }

    private void Update()
    {
        if (!IsLocalPlayer)
            return;

        if (Input.GetKey(KeyCode.R))
        {
            var nearestOtherCharacter = Scenario.GetNearestOtherCharacter(this);
            if (!nearestOtherCharacter)
            {
                Debug.LogWarning($"Couldn't find another non-local character to target with right hand IK");
                return;
            }
            
            _rightHandIK.Enabled.Value = true;
            _rightHandIK.TargetPosition.Value = nearestOtherCharacter.RightHand.position;
        }
        else
        {
            _rightHandIK.Enabled.Value = false;
        }
    }
}
