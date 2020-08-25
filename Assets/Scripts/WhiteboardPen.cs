using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Mirror;


public class WhiteboardPen : NetworkBehaviour 
{

    public Whiteboard whiteboard;
    private bool lastTouch;
    private Quaternion lastAngle;
    private RaycastHit touch;
    public Color color;
    // public GameObject penTip;
    public GameObject markerInteractable;

    // Start is called before the first frame update
    void Start()
    {
        this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard> ();
        this.whiteboard.SetObjectType("pen");
    }

    // Update is called once per frame
    void Update()
    {
        float tipHeight = transform.Find("Tip").transform.localScale.y;

        Vector3 tip = transform.Find("Tip").transform.position;

        // Vector3 forward = (transform.right * -1) * .05f;
        Vector3 forward = (transform.forward) * .03f;
        // Vector3 backward = (-transform.forward) * .05f;
        // Vector3 right = (transform.right) * .05f;
        // Vector3 left = (-transform.right) * .05f;
        Debug.DrawRay(tip, forward, Color.red); 
        // Debug.DrawRay(tip, backward, Color.blue);
        // Debug.DrawRay(tip, right, Color.green);
        // Debug.DrawRay(tip, left, Color.yellow); 


        if (Physics.Raycast(tip, this.transform.forward, out this.touch, 0.05f)){
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
                this.lastAngle = this.markerInteractable.transform.rotation;
            }
        }
        else{
            this.whiteboard.ToggleTouch(false);
            this.lastTouch = false;
        }

        if(lastTouch){
            this.markerInteractable.transform.rotation = lastAngle;
        }
    }
}