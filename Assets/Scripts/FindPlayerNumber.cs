using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FindPlayerNumber : NetworkBehaviour
{
    // [SyncVar(hook=nameof(SetNumber))]
    public int numberOfPlayers; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        // numberOfPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    void SetNumber(int oldNumber, int newNumber){
        // if(isServer){
        //     numberOfPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
        // }
        numberOfPlayers = newNumber;

    }

    // [ClientRpc]

}
