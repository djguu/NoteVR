using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using InputTracking = UnityEngine.XR.InputTracking;
using Node = UnityEngine.XR.XRNode;

public class LocalPlayerControl : NetworkBehaviour
{
    public OVRCameraRig ovrCameraRig;
    public Camera leftEye;
    public Camera rightEye;
    public Vector3 pos;
    public Transform leftHand;
    public Transform rightHand;
    // public Animator anim;

    public float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        // anim = GetComponentInChildren<Animator>();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        //Resolve que a camera nao salte  para o novo jogador
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
            print(ovrCameraRig.centerEyeAnchor.position);

            Vector3 euler = transform.rotation.eulerAngles;
            
            Vector2 primaryAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

            Vector2 secondaryAxys = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

            // if (primaryAxis.x != 0 || primaryAxis.y != 0){
            //     anim.SetBool("Idle", false);
            //     GetComponent<LocalAnimationControl>().CmdUpdateAnim("run");
            // }
            // else{
            //     anim.SetBool("Idle", true);
            //     GetComponent<LocalAnimationControl>().CmdUpdateAnim("idle");
            // }
            //posicao das maos
            leftHand.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            rightHand.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            // leftHand.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            // rightHand.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            leftHand.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            rightHand.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            // leftHand.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            // rightHand.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            // leftHand.localRotation = InputTracking.GetLocalRotation(Node.LeftHand);
            // rightHand.localRotation = InputTracking.GetLocalRotation(Node.RightHand);
            // leftHand.localPosition = InputTracking.GetLocalPosition(Node.LeftHand);
            // rightHand.localPosition = InputTracking.GetLocalPosition(Node.RightHand);

            //posiçao e rotaçao do jogador
            
            // print(primaryAxis.y);
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
}
