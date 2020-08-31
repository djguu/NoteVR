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
    // public GameObject penTip;

    // Start is called before the first frame update
    void Start()
    {
        this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard> ();
        this.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        float tipHeight = transform.Find("Bottom").transform.localScale.y;

        Vector3 tip = transform.Find("Bottom").transform.position;

        Vector3 down = -transform.up;

        // Debug.DrawRay(tip, dow  * .05f, Color.red); 

        if (Physics.Raycast(tip, down, out this.touch, 0.02f)){
            
            if(!(this.touch.collider.tag == "Whiteboard"))
                return;

            this.whiteboard.SetObjectType("Eraser");

            this.whiteboard.SetColor(this.color);
            this.whiteboard.SetTouchPosition(this.touch.textureCoord.x, this.touch.textureCoord.y);
            this.whiteboard.ToggleTouch(true);

            if(!this.lastTouch){
                this.lastTouch = true;
                this.lastAngle = this.eraserInteractable.transform.rotation;
            }
        }
        else{
            this.whiteboard.ToggleTouch(false);
            this.lastTouch = false;
        }

        if(lastTouch){
            this.eraserInteractable.transform.rotation = lastAngle;
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
        this.eraserInteractable.GetComponent<Rigidbody>().useGravity = gravity;
    }
}