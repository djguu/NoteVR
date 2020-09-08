using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class Drawer : NetworkBehaviour 
{
    private Whiteboard whiteboard;
    private bool lastTouch;
    private Quaternion lastAngle;
    private RaycastHit touch;
    public Color color;
    public GameObject interactable;
    public GameObject point;
    
    public enum Type 
    {
        Marker, 
        Eraser
    };
    public Type objectType = Type.Marker;

    void Start()
    {
        whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard> ();
    }

    void OnEnable(){
        GameObject[] localplayer = GameObject.FindGameObjectsWithTag("LocalPlayer");
        localplayer[0].GetComponent<LocalPlayerControl>().CmdPassAuthority(gameObject);
    }

    [Client]
    void Update()
    {  
        var length = 0f;

        Vector3 tip; 

        Vector3 way;

        if (objectType.ToString() == "Marker"){
            tip = point.transform.position;
            way = point.transform.up;
            length = 0.03f;
        }
        else{
            tip = point.transform.Find("Bottom").transform.position;
            way = -point.transform.up;
            length = 0.02f;
        } 

        if (Physics.Raycast(tip, way, out touch, length)){
            
            if(!(touch.collider.tag == "Whiteboard"))
                return;

            whiteboard.SetObjectType(objectType.ToString());
            if (objectType.ToString() == "Marker")
                whiteboard.SetColor(color);

            whiteboard.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
            whiteboard.ToggleTouch(true);

            if(!lastTouch){
                lastTouch = true;
                lastAngle = interactable.transform.rotation;
            }
        }
        else{
            whiteboard.ToggleTouch(false);
            lastTouch = false;
        }

        if(lastTouch){
            interactable.transform.rotation = lastAngle;
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
        interactable.GetComponent<Rigidbody>().useGravity = gravity;
    }
}