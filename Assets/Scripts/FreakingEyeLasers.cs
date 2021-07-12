using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

public class FreakingEyeLasers : NetworkBehaviour
{
    [SerializeField] private GameObject _lasers;

    private NetworkVariable<float> health = new NetworkVariable<float>(100f);   // Only writable by the server, readable by all clients
    
    void Start()
    {
        _lasers.SetActive(false);
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetMouseButtonDown(0))
                SetEyeLasersVisibilityServerRpc(true);
            if (Input.GetMouseButtonUp(0))
                SetEyeLasersVisibilityServerRpc(false);
        }
    }

    [ServerRpc]
    private void SetEyeLasersVisibilityServerRpc(bool visible)
    {
        // TODO: Check if the player has enough eyeball juice to shoot freaking lasers
        SetEyeLasersVisibilityClientRpc(visible);
    }

    [ClientRpc]
    private void SetEyeLasersVisibilityClientRpc(bool visible)
    {
        _lasers.SetActive(visible);
    }
}
