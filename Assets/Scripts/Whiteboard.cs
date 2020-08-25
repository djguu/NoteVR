using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;      //VERIFICAR O QUE É ISTO

public class Whiteboard : NetworkBehaviour
{

    private int textureSize = 2048;
    private int penSize = 1;
    private Texture2D texture;
    private Color[] color;

    private bool touching, touchingLast;
    private float posX, posY;
    private float lastX, lastY;

    private string objectType;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        // renderer.material.color = Color.white;
        this.texture = new Texture2D(textureSize, textureSize);

        // Color[] textColor = new Color[] {};
        // textColor =  this.texture.GetPixels();

        // for(var i = 0; i < textColor.Length; ++i)
        //  {
        //      textColor[i] = Color.white;
        //  }
        // this.texture.SetPixels(textColor);
        // this.texture.Apply();

        renderer.material.mainTexture = this.texture;
    }

    [Client]
    void Update()
    {
        // print(color);
        RpcDraw();
        CmdDraw();  //Just draws it locally
    }

    [Command]
    void CmdDraw(){
        RpcDraw();
    }

    [ClientRpc]
    void RpcDraw(){
        if(this.objectType == "eraser"){
                this.penSize = 3;
            }
            else{
                this.penSize = 1;
            }
        // Debug.Log(posX + " " + posY);
        int x = (int) (posX * textureSize - (penSize / 2));
        int y = (int) (posY * textureSize - (penSize / 2));

        if(touchingLast){
            this.texture.SetPixels(x, y, penSize, penSize, color);

            float xDistance = Mathf.Abs(lastX-(float)x);
            float yDistance = Mathf.Abs(lastY-(float)y);

            if(xDistance < (penSize * 5) && yDistance < (penSize * 5) ){
                for(float t = 0.01f; t < 1.00f; t += 0.01f){
                    int lerpX = (int) Mathf.Lerp(lastX, (float)x, t);
                    int lerpY = (int) Mathf.Lerp(lastY, (float)y, t);
                    this.texture.SetPixels(lerpX, lerpY, penSize, penSize, this.color);
                }
            }
            texture.Apply();
        }

        this.lastX = (float)x;
        this.lastY = (float)y;

        this.touchingLast = this.touching;
    }

    public void ToggleTouch(bool touching){
        this.touching = touching;
    }

    public void SetTouchPosition(float x, float y){
        this.posX = x;
        this.posY = y;
    }

    public void SetColor(Color color){
        this.color = Enumerable.Repeat<Color>(color, penSize * penSize).ToArray<Color>();
    }

    public void SetObjectType(string type){
        this.objectType = type;
    }
}
