using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RefreshButton : NetworkBehaviour
{
    public Whiteboard whiteboard;
    public float howFarStop;
    public bool Pressed;

    private Transform Position;
    private Vector3 StartPos;

    private bool ReverseDirection = false;
    // Start is called before the first frame update
    void Start()
    {
        this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard> ();
        Position = GetComponent<Transform>();
        StartPos = Position.position;
    }

    // Update is called once per frame
    void Update()
    {
        // print("posX: " + Position.position.x + " StartPosX: " + StartPos.x + " Distance: " + Mathf.Abs(Position.position.x - StartPos.x));
        if(Mathf.Abs(Position.position.x - StartPos.x) > howFarStop && !ReverseDirection) {
            print("1st");
            // print(Mathf.Abs(Position.position.x - StartPos.x));
            //check to see if the button has been pressed all the way down«
            Position.position= new Vector3(StartPos.x, Position.position.y, Position.position.z);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            // ReverseDirection = true;
            Pressed = true;//update pressed
        }

        if (Position.position.x > StartPos.x && ReverseDirection) {
            print("2nd");
            // print("Here pos: " + Position.position.x + " start:" + StartPos.x);
            Position.position= new Vector3(StartPos.x, Position.position.y, Position.position.z);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ReverseDirection = false;
        }
    }

    void OnCollisionExit(Collision collision)//check for when to unlock the button
    {
        // Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Hand") {
            print("3rd");
            // GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionX; //Remove Y movement constraint.
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            ReverseDirection = true;
            // Pressed = true;//update pressed
            this.whiteboard.ResetWhiteboard();
        }
    }
}
