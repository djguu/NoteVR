using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class LocalPlayerControl : NetworkBehaviour
{
    public GameObject ovrCameraRig;
    public Camera leftEye;
    public Camera rightEye;
    public Vector3 pos;
    public Transform leftHand;
    public Transform rightHand;

    // private FindPlayerNumber findNumber;

    // [SyncVar]
    // public int playerNumber;
    
    // public GameObject playerNameObject;

    // [SyncVar]
    // public string playerName;
    // private TextMeshPro playerNameBox;

    [SerializeField]
    public float speed = 3;

    
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
    }

    
    void Update()
    {   
        //Allows only the main camera and the player camera to be on scene
        if(!isLocalPlayer){
            Destroy(ovrCameraRig);
        }
        else{
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

            //handles the hands position
            leftHand.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            rightHand.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            leftHand.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            rightHand.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);


            //handles player position and rotation
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

            
            euler.y += secondaryAxys.x * speed;
            
            transform.rotation = Quaternion.Euler(euler);

            transform.localRotation = Quaternion.Euler(euler);
        }
    }

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
}
