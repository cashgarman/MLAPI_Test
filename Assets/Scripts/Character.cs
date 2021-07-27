using System;
using System.Linq;
using Dissonance;
using DitzelGames.FastIK;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Character : NetworkBehaviour, IDissonancePlayer
{
    [SerializeField] private Transform _head;
    
    private GameObject _thirdPersonCharacterObject;
    [SerializeField] private FastIKFabric _rightHandIK;
    [SerializeField] private FastIKFabric _leftHandIK;
    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private DissonanceComms _dissonanceCommsPrefab;
    private DissonanceComms _comms;

    public static Character LocalCharacter => FindObjectsOfType<Character>().FirstOrDefault(character => character.IsLocalPlayer);
    public string PlayerId { get; set; }
    public Vector3 Position => _thirdPersonCharacterObject.transform.position;
    public Quaternion Rotation => _thirdPersonCharacterObject.transform.rotation;
    public NetworkPlayerType Type => IsLocalPlayer ? NetworkPlayerType.Local : NetworkPlayerType.Remote;
    public bool IsTracking { get; set; }
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
        // If this is the local player
        else
        {
            // Create the dissonance object in the scene
            Debug.Log($"Creating Dissonance Comms object");
            Instantiate(_dissonanceCommsPrefab);
            
            //A client is starting. Start tracking if the name has been properly initialised.
            if (!string.IsNullOrEmpty(PlayerId))
                StartTracking();

            // Setup the Dissonance voice comms
            _comms = FindObjectOfType<DissonanceComms>();
            if (_comms == null)
                throw new Exception("Cannot find DissonanceComms component in scene");

            Debug.Log($"Tracking `NetworkStart for LocalPlayer` Name={_comms.LocalPlayerName}");

            // This method is called on the client which has control authority over this object. This will be the local client of whichever player we are tracking.
            Debug.Log($"_comms.LocalPlayerName: {_comms.LocalPlayerName}");
            if (_comms.LocalPlayerName != null)
                SetPlayerName(_comms.LocalPlayerName);

            //Subscribe to future name changes (this is critical because we may not have run the initial set name yet and this will trigger that initial call)
            _comms.LocalPlayerNameChanged += SetPlayerName;
        }
    }
    
    public void OnDestroy()
    {
        if (_comms != null)
            _comms.LocalPlayerNameChanged -= SetPlayerName;
    }

    public void OnEnable()
    {
        _comms = FindObjectOfType<DissonanceComms>();
    }

    public void OnDisable()
    {
        if (IsTracking)
            StopTracking();
    }

    private void SetPlayerName(string playerName)
    {
        //We need the player name to be set on all the clients and then tracking to be started (on each client).
        //To do this we send a command from this client, informing the server of our name. The server will pass this on to all the clients (with an RPC)
        // Client -> Server -> Client

        //We need to stop and restart tracking to handle the name change
        if (IsTracking)
            StopTracking();

        //Perform the actual work
        PlayerId = playerName;
        StartTracking();

        //Inform the server the name has changed
        if (IsLocalPlayer)
            SetPlayerNameServerRpc(playerName);
    }

    [ServerRpc]
    private void SetPlayerNameServerRpc(string playerName)
    {
        PlayerId = playerName;

        // Now call the RPC to inform clients they need to handle this changed value
        SetPlayerNameClientRpc(playerName);
    }
    
    [ClientRpc]
    private void SetPlayerNameClientRpc(string playerName)
    {
        // received a message from server (on all clients). If this is not the local player then apply the change
        if (!IsLocalPlayer)
            SetPlayerName(playerName);
    }
    
    private void StartTracking()
    {
        if (IsTracking)
            throw new Exception("Attempting to start player tracking, but tracking is already started");

        if (_comms != null)
        {
            Debug.Log($"Dissonance started tracking character {PlayerId} owned by client {OwnerClientId}");
            _comms.TrackPlayerPosition(this);
            IsTracking = true;
        }
    }

    private void StopTracking()
    {
        if (!IsTracking)
            throw new Exception("Attempting to stop player tracking, but tracking is not started");

        if (_comms != null)
        {
            Debug.Log($"Dissonance stopped tracking character {PlayerId} owned by client {OwnerClientId}");
            _comms.StopTracking(this);
            IsTracking = false;
        }
    }
    
    private void Update()
    {
        if (!IsLocalPlayer)
            return;

        // TESTING: Reach out to the nearest other character using IK when R is held
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
