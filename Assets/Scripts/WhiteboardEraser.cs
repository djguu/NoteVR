using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Mirror;


public class WhiteboardEraser : NetworkBehaviour 
{

    private Whiteboard whiteboard;
    private bool lastTouch;
    private Quaternion lastAngle;
    private RaycastHit touch;
    private Color color;
    private Material whiteboardMaterial;
    public GameObject eraserInteractable;
    private Rigidbody eraserInteractableRigidbody;

    void Start()
    {
        whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard> ();
        color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        float tipHeight = transform.Find("Bottom").transform.localScale.y;

        Vector3 tip = transform.Find("Bottom").transform.position;

        Vector3 down = -transform.up;

        // Debug.DrawRay(tip, dow  * .05f, Color.red); 

        if (Physics.Raycast(tip, down, out touch, 0.02f)){
            
            if(!(touch.collider.tag == "Whiteboard"))
                return;

            whiteboard.SetObjectType("Eraser");

            whiteboard.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
            whiteboard.ToggleTouch(true);

            if(!lastTouch){
                lastTouch = true;
                lastAngle = eraserInteractable.transform.rotation;
            }
        }
        else{
            whiteboard.ToggleTouch(false);
            lastTouch = false;
        }

        if(lastTouch){
            eraserInteractable.transform.rotation = lastAngle;
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
        eraserInteractable.GetComponent<Rigidbody>().useGravity = gravity;
    }
}