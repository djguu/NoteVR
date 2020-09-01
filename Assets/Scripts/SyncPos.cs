using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncPos : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [Client]
    // Update is called once per frame
    void Update()
    {
        if(!isServer){
            CmdSyncPos(transform.localPosition, transform.localRotation);
        }
        
        Debug.Log("hasAuthority" + hasAuthority);
        // Debug.Log("isClient" + isClient);
        // Debug.Log("isClientOnly" + isClientOnly);
        // Debug.Log("isServer" + isServer);
        // Debug.Log("isServerOnly" + isServerOnly);
    }

    [Command(ignoreAuthority=true)]
    void CmdSyncPos(Vector3 localpos, Quaternion localrot){
        RpcSyncPos(localpos, localrot);
    }

    [ClientRpc]
    void RpcSyncPos(Vector3 localpos, Quaternion localrot){
        transform.localPosition = localpos;
        transform.localRotation = localrot;
    }
}
