using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalAnimationControl : NetworkBehaviour
{
    public Animator animator;
    public GameObject[] animationBodyParts;
    public Material invisible;

    [SyncVar (hook = "OnAnimationStateChange")]
    public string animState = "idle";

    void OnAnimationStateChange(string aString){
        if(isLocalPlayer) return;
        UpdateAnimation(aString);
    }

    void UpdateAnimation(string aString){
        if(animState == aString) return;
        animState = aString;

        if(animState == "idle"){
            animator.SetBool("Idle", true);
        }
        if(animState == "run"){
            animator.SetBool("Idle", false);
        }
    }

    [Command]
    public void CmdUpdateAnim(string aString){
        UpdateAnimation(aString);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Idle", true);

        if(isLocalPlayer){
            foreach(GameObject bodypart in animationBodyParts){
                SkinnedMeshRenderer[] mesh = bodypart.GetComponentInChildren<SkinnedMeshRenderer[]>();
                Renderer[] r = bodypart.GetComponentInChildren<Renderer[]>();
                foreach(SkinnedMeshRenderer meshRender in mesh){
                    meshRender.material = invisible;
                }
                foreach(Renderer rend in r){
                    rend.material = invisible;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
