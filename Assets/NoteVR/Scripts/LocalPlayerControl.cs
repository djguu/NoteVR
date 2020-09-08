using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System.IO;

public class LocalPlayerControl : NetworkBehaviour
{
    public GameObject ovrCameraRig;
    private Vector3 pos;
    public Transform leftHand;
    public Transform rightHand;
    
    public GameObject playerNameObject;

    [SyncVar]
    private string playerName;
    private TextMeshPro playerNameBox;

    [SerializeField]
    public float speed = 3;

    private Whiteboard whiteboard;

    private Texture2D texture;

    public override void OnStartServer(){
        playerName = "User " + (netIdentity.connectionToClient.connectionId + 1);
        whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard> ();
        texture = whiteboard.texture;
    }

    public override void OnStartLocalPlayer(){
        tag = "LocalPlayer";
    }

    public override void OnStopServer(){
        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null )
            File.WriteAllBytes(Application.persistentDataPath + "/" + "Whiteboard.png", pngData);
        else
            Debug.Log("Could not convert Whiteboard to png. Skipping saving texture");
    }

    
    public override void OnStartClient()
    {
        pos = transform.position;
        playerNameBox = playerNameObject.GetComponent<TextMeshPro>();
        playerNameBox.text = playerName;
    }

    
    void Update()
    {   
        //Allows only the main camera and the player camera to be on scene
        if(!isLocalPlayer){
            Destroy(ovrCameraRig);
        }
        else{
            //handles the hands position
            leftHand.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            rightHand.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            leftHand.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            rightHand.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

            Vector3 euler = transform.rotation.eulerAngles;
            
            Vector2 primaryAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

            Vector2 secondaryAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

            if(primaryAxis.y > 0f){
                pos += (primaryAxis.y * transform.forward * Time.deltaTime * speed);
            }
            if(primaryAxis.y < 0f){
                pos += (Mathf.Abs(primaryAxis.y) * -transform.forward * Time.deltaTime * speed);
            }
            if(primaryAxis.x > 0f){
                pos += (primaryAxis.x * transform.right * Time.deltaTime * speed);
            }
            if(primaryAxis.x < 0f){
                pos += (Mathf.Abs(primaryAxis.x) * -transform.right * Time.deltaTime * speed);
            }

            transform.position = pos;

            
            euler.y += secondaryAxis.x * speed;
            
            transform.rotation = Quaternion.Euler(euler);

            transform.localRotation = Quaternion.Euler(euler);
        }
    }

    [Command]
    public void CmdPassAuthority(GameObject marker){
        marker.GetComponent<NetworkIdentity>().RemoveClientAuthority();
        marker.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }
}
