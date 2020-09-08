using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
<<<<<<< HEAD:Assets/NoteVR/Scripts/LocalPlayerControl.cs
using System.IO;
=======
>>>>>>> PlayerModifications:Assets/Scripts/LocalPlayerControl.cs

public class LocalPlayerControl : NetworkBehaviour
{
    public GameObject ovrCameraRig;
    private Vector3 pos;
    public Transform leftHand;
    public Transform rightHand;
<<<<<<< HEAD:Assets/NoteVR/Scripts/LocalPlayerControl.cs
    
    public GameObject playerNameObject;

    [SyncVar]
    private string playerName;
    private TextMeshPro playerNameBox;
=======

    // private FindPlayerNumber findNumber;

    // [SyncVar]
    // public int playerNumber;
    
    // public GameObject playerNameObject;

    // [SyncVar]
    // public string playerName;
    // private TextMeshPro playerNameBox;
>>>>>>> PlayerModifications:Assets/Scripts/LocalPlayerControl.cs

    [SerializeField]
    public float speed = 3;

<<<<<<< HEAD:Assets/NoteVR/Scripts/LocalPlayerControl.cs
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
=======
    
    // public override void OnStartClient()
    void Start()
    {
        pos = transform.position;
        // // playerNameBox = playerNameObject.GetComponent<TextMeshPro>();
        // // if(hasAuthority){
        // findNumber = GameObject.Find("NumberOfPlayers").GetComponent<FindPlayerNumber>();
        // findNumber.numberOfPlayers += 1;
        // playerNumber = findNumber.numberOfPlayers;
        // playerName = "User " + (playerNumber).ToString();

        // // }
        // SetPlayerName(playerName);
>>>>>>> PlayerModifications:Assets/Scripts/LocalPlayerControl.cs
    }

    
    void Update()
    {   
        //Allows only the main camera and the player camera to be on scene
        if(!isLocalPlayer){
            Destroy(ovrCameraRig);
        }
        else{
<<<<<<< HEAD:Assets/NoteVR/Scripts/LocalPlayerControl.cs
=======
            if(leftEye.tag != "MainCamera"){
                leftEye.tag = "MainCamera";
                leftEye.enabled = true;
            }
            if(rightEye.tag != "MainCamera"){
                rightEye.tag = "MainCamera";
                rightEye.enabled = true;
            }

            Vector3 euler = transform.rotation.eulerAngles;
            
            Vector2 primaryAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

            Vector2 secondaryAxys = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

>>>>>>> PlayerModifications:Assets/Scripts/LocalPlayerControl.cs
            //handles the hands position
            leftHand.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            rightHand.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            leftHand.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            rightHand.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

<<<<<<< HEAD:Assets/NoteVR/Scripts/LocalPlayerControl.cs
            Vector3 euler = transform.rotation.eulerAngles;
            
            Vector2 primaryAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

            Vector2 secondaryAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

=======

            //handles player position and rotation
>>>>>>> PlayerModifications:Assets/Scripts/LocalPlayerControl.cs
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

<<<<<<< HEAD:Assets/NoteVR/Scripts/LocalPlayerControl.cs
    [Command]
    public void CmdPassAuthority(GameObject marker){
        marker.GetComponent<NetworkIdentity>().RemoveClientAuthority();
        marker.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }
=======
    // [Client]
    // void SetPlayerName(string name){
    //     CmdSendNameToServer(playerName);
    // }

    // [Command]
    // void CmdSendNameToServer(string name){
    //     RpcSetPlayerName(name);
    // }

    // [ClientRpc]
    // void RpcSetPlayerName(string name){
    //     playerNameBox = transform.Find("name").gameObject.GetComponent<TextMeshPro>();
    //     playerNameBox.text = name;
    // }
>>>>>>> PlayerModifications:Assets/Scripts/LocalPlayerControl.cs
}
