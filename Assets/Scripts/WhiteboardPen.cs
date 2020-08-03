using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class WhiteboardPen : MonoBehaviour 
{

    public Whiteboard whiteboard;
    private bool lastTouch;
    private Quaternion lastAngle;
    private RaycastHit touch;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        this.whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard> ();
    }

    // Update is called once per frame
    void Update()
    {
        float tipHeight = transform.Find("Tip").transform.localScale.y;

        Vector3 tip = transform.Find("Tip").transform.position;

        Vector3 forward = (transform.right * -1) * .05f;

        // if (Physics.Raycast(tip, transform.right * -1, out touch, tipHeight)){
        if (Physics.Raycast(tip, transform.right * -1, out touch, 0.05f)){
            Debug.DrawRay(tip, forward, Color.green); 
            
            if(!(touch.collider.tag == "Whiteboard"))
                return;

            // print(touch.distance);
            // Debug.Log(touch.collider.tag);

            this.whiteboard = touch.collider.GetComponent <Whiteboard> ();
            // Debug.Log ("touching!");

            this.whiteboard.SetColor(this.color);
            this.whiteboard.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
            this.whiteboard.ToggleTouch(true);

            if(!lastTouch){
                lastTouch = true;
                lastAngle = transform.rotation;
            }
        }
        else{
            this.whiteboard.ToggleTouch(false);
            lastTouch = false;
        }

        if(lastTouch){
            transform.rotation = lastAngle;
        }
    }
}