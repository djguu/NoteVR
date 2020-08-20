using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using InputTracking = UnityEngine.XR.InputTracking;
using Node = UnityEngine.XR.XRNode;

public class LocalPlayerControl : NetworkBehaviour
{
    public GameObject ovrCameraRig;
    public Transform leftHand;
    public Transform rightHand;
    public Camera leftEye;
    public Camera rightEye;
    Vector3 pos;
    public float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {   //Resolve que a camera nao salte  para o novo jogador
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
            
            //posicao das maos
            leftHand.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            rightHand.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            leftHand.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            rightHand.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            // leftHand.localRotation = InputTracking.GetLocalRotation(Node.LeftHand);
            // rightHand.localRotation = InputTracking.GetLocalRotation(Node.RightHand);
            // leftHand.localPosition = InputTracking.GetLocalPosition(Node.LeftHand);
            // rightHand.localPosition = InputTracking.GetLocalPosition(Node.RightHand);

            //posiçao e rotaçao do jogador
            Vector2 primaryAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
            // print(primaryAxis.y);
            if(primaryAxis.y > 0f){
                pos += (primaryAxis.y * -transform.forward * Time.deltaTime * speed);
            }
            if(primaryAxis.y < 0f){
                pos += (Mathf.Abs(primaryAxis.y) * transform.forward * Time.deltaTime * speed);
            }
            if(primaryAxis.x > 0f){
                pos += (primaryAxis.x * -transform.right * Time.deltaTime * speed);
            }
            if(primaryAxis.x < 0f){
                pos += (Mathf.Abs(primaryAxis.x) * transform.right * Time.deltaTime * speed);
            }

            transform.position = pos;

            Vector3 euler = transform.rotation.eulerAngles;
            Vector2 secondaryAxys = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
            euler.y += secondaryAxys.x * speed;
            transform.rotation = Quaternion.Euler(euler);

            transform.localRotation = Quaternion.Euler(euler);
        }
    }
}
