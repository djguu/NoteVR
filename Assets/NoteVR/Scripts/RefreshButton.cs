﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RefreshButton : NetworkBehaviour
{
    public Whiteboard whiteboard;
    public float howFarStop;

    private Transform Position;
    private Vector3 StartPos;

    private bool ReverseDirection = false;

    void Start()
    {
        whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard> ();
        Position = GetComponent<Transform>();
        StartPos = Position.position;
    }

    [Client]
    void Update()
    {
        if(Mathf.Abs(Position.position.x - StartPos.x) > howFarStop && !ReverseDirection) {
            //check to see if the button has been pressed all the way down
            Position.position= new Vector3(StartPos.x, Position.position.y, Position.position.z);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        if (Position.position.x > StartPos.x && ReverseDirection) {
            Position.position= new Vector3(StartPos.x, Position.position.y, Position.position.z);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ReverseDirection = false;
        }
    }

    [Client]
    void OnCollisionExit(Collision collision)//check for when to unlock the button
    {
        if (collision.gameObject.tag == "Hand") {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            ReverseDirection = true;
            CmdResetWhiteboard();
        }
    }

    [Command(ignoreAuthority=true)]
    void CmdResetWhiteboard(){
        RpcResetWhiteboard();
    }

    [ClientRpc]
    void RpcResetWhiteboard(){
        whiteboard.ResetWhiteboard();
    }
}
