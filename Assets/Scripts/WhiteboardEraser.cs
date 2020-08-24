using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Mirror;


public class WhiteboardEraser : NetworkBehaviour 
{

    public Whiteboard whiteboard;
    private bool lastTouch;
    private Quaternion lastAngle;
    private RaycastHit touch;
    public Color color;
    private Material whiteboardMaterial;
    public GameObject eraserInteractable;
    private Rigidbody eraserInteractableRigidbody;
    // public GameObject penTip;

    // Start is called before the first frame update
    void Start()
    {
        this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard> ();
        this.whiteboardMaterial = GameObject.Find("Whiteboard").GetComponent<Renderer>().material;
        this.color = whiteboardMaterial.color;
        this.eraserInteractableRigidbody = this.eraserInteractable.GetComponent<Rigidbody>();
        // print(this.color);
        // this.whiteboard.SetColor(this.color);
        this.whiteboard.SetObjectType("eraser");
    }

    // Update is called once per frame
    void Update()
    {
        float tipHeight = transform.Find("Bottom").transform.localScale.y;

        Vector3 tip = transform.Find("Bottom").transform.position;

        Vector3 down = (transform.up * -1) * .03f;
        Debug.DrawRay(tip, down, Color.red); 


        if (Physics.Raycast(tip, this.transform.up * -1, out this.touch, 0.03f)){
            // Debug.DrawRay(tip, transform.forward, Color.red); 
            
            if(!(this.touch.collider.tag == "Whiteboard"))
                return;

            // print(touch.distance);
            Debug.Log(this.touch.collider.tag);

            this.whiteboard = this.touch.collider.GetComponent <Whiteboard> ();

            this.whiteboard.SetColor(this.color);
            this.whiteboard.SetTouchPosition(this.touch.textureCoord.x, this.touch.textureCoord.y);
            this.whiteboard.ToggleTouch(true);

            if(!this.lastTouch){
                this.lastTouch = true;
                this.lastAngle = this.transform.rotation;
            }
        }
        else{
            // this.whiteboard.ToggleTouch(false);
            this.lastTouch = false;
        }

        if(lastTouch){
            // transform.rotation = lastAngle;
            this.eraserInteractableRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
        else{
            this.eraserInteractableRigidbody.constraints =  RigidbodyConstraints.None;
        }
    }
}