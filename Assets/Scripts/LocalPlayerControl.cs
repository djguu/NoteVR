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
            leftHand.localRotation = InputTracking.GetLocalRotation(Node.LeftHand);
            rightHand.localRotation = InputTracking.GetLocalRotation(Node.RightHand);
            leftHand.localPosition = InputTracking.GetLocalPosition(Node.LeftHand);
            rightHand.localPosition = InputTracking.GetLocalPosition(Node.RightHand);

            //posiçao e rotaçao do jogador
            Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            if(primaryAxis.y > 0f){
                pos += (primaryAxis.y * transform.forward * Time.deltaTime);
            }
            if(primaryAxis.y < 0f){
                pos += (Mathf.Abs(primaryAxis.y) * -transform.right * Time.deltaTime);
            }
            if(primaryAxis.x > 0f){
                pos += (primaryAxis.y * transform.forward * Time.deltaTime);
            }
            if(primaryAxis.x < 0f){
                pos += (Mathf.Abs(primaryAxis.y) * -transform.right * Time.deltaTime);
            }

            transform.position = pos;

            Vector3 euler = transform.rotation.eulerAngles;
            Vector2 secondaryAxys = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            euler.y += secondaryAxys.y;
            transform.rotation = Quaternion.Euler(euler);

            transform.localRotation = Quaternion.Euler(euler);
        }
    }
}
