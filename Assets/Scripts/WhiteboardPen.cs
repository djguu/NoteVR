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
    // public GameObject penTip;

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
        Debug.DrawRay(tip, forward, Color.red); 


        if (Physics.Raycast(tip, this.transform.right * -1, out this.touch, 0.05f)){
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

        // if(lastTouch){
        //     transform.rotation = lastAngle;
        // }
    }
}