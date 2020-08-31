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
        CmdSyncPos(transform.localPosition, transform.localRotation);
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
