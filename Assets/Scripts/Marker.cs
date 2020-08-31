using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Mirror;


public class Marker : NetworkBehaviour 
{
    private Whiteboard whiteboard;
    private bool lastTouch;
    private Quaternion lastAngle;
    private RaycastHit touch;
    public Color color;
    public GameObject penTip;
    public GameObject markerInteractable;
    

    // Start is called before the first frame update
    void Start()
    {
        whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard> ();
    }

    [Client]
    // Update is called once per frame
    void Update()
    {
        float tipHeight = penTip.transform.localScale.y;

        Vector3 tip = penTip.transform.position;

        Vector3 forward = penTip.transform.up;

        // Debug.DrawRay(tip, forward * .03f, Color.red); 

        if (Physics.Raycast(tip, forward, out touch, 0.03f)){
            
            if(!(touch.collider.tag == "Whiteboard"))
                return;

            whiteboard.SetObjectType("Marker");

            whiteboard.SetColor(color);
            whiteboard.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
            whiteboard.ToggleTouch(true);

            if(!lastTouch){
                lastTouch = true;
                lastAngle = markerInteractable.transform.rotation;
            }
        }
        else{
            whiteboard.ToggleTouch(false);
            lastTouch = false;
        }

        if(lastTouch){
            markerInteractable.transform.rotation = lastAngle;
        }
    }

    public void SetGravity(bool gravity){
        CmdSetGravity(gravity);
    }

    [Command(ignoreAuthority=true)]
    void CmdSetGravity(bool gravity){
        RpcSetGravity(gravity);
    }

    [ClientRpc]
    void RpcSetGravity(bool gravity){
        markerInteractable.GetComponent<Rigidbody>().useGravity = gravity;
    }
}