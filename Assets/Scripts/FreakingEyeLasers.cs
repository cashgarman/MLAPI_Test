using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class FreakingEyeLasers : NetworkBehaviour
{
    [SerializeField] private GameObject _lasers;

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
        SetEyeLasersVisibilityClientRpc(visible);
    }

    [ClientRpc]
    private void SetEyeLasersVisibilityClientRpc(bool visible)
    {
        _lasers.SetActive(visible);
    }
}
