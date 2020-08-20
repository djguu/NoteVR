using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRig : MonoBehaviour
{
    public Transform touchL;
    public Transform touchR;

    public bool inputA;
    public bool inputB;
    public bool inputX;
    public bool inputY;

    public Vector2 rightStick;
    public Vector2 leftStick;


    // Update is called once per frame
    void Update()
    {
        touchL.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        touchR.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

        touchL.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        touchR.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

        inputA = OVRInput.Get(OVRInput.RawButton.A);

        if(OVRInput.GetDown(OVRInput.RawButton.B)){
            inputB = !inputB;
        }

        rightStick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

        inputX = OVRInput.Get(OVRInput.RawButton.X);

        if(OVRInput.GetDown(OVRInput.RawButton.Y)){
            inputY = !inputY;
        }

        leftStick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
    }
}
