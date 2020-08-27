using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;      //VERIFICAR O QUE É ISTO

[RequireComponent(typeof(MeshRenderer))]
public class Whiteboard : NetworkBehaviour
{

    private int textureSize = 2048;
    private int penSize = 1;

    public int lerpMultiplier = 6;
    private Texture2D texture;
    private Texture2D newTexture;
    private int textureWidth;
    private int textureHeight;
    private Color[] color;

    private bool touching, touchingLast;
    private float posX, posY;
    private float lastX, lastY;

    private string objectType;

    // private Color[] textColor = new Color[] {};
    private Color32[] textColor = new Color32[] {};

    // Start is called before the first frame update
    void Awake()
    {
        // Renderer renderer = GetComponent<MeshRenderer>();

        // this.texture = new Texture2D(textureSize, textureSize);
        
        // textColor =  this.texture.GetPixels();

        // for(var i = 0; i < textColor.Length; ++i)
        //  {
        //      textColor[i] = Color.white;
        //  }

        // ResetWhiteboard();

        // renderer.material.mainTexture = this.texture;

        this.texture = GetComponent<MeshRenderer>().material.mainTexture as Texture2D;

        this.textureWidth = this.texture.width;
        this.textureHeight = this.texture.height;

        this.textColor = new Color32[textureWidth * textureHeight];
        
        this.newTexture = new Texture2D(texture.width, texture.height);
        // newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false, false);
        // newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, true, true);
        // newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false, true);


        for(var i = 0; i < textColor.Length; ++i)
        {
            textColor[i] = Color.white;
        }

        ResetWhiteboard();
        
        GetComponent<MeshRenderer>().material.mainTexture = this.newTexture;
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
        if(this.objectType == "Eraser"){
                this.penSize = 5;
        }
        else{
            this.penSize = 1;
        }
        // Debug.Log(posX + " " + posY);
        int x = (int) (posX * textureSize - (penSize / 2));
        int y = (int) (posY * textureSize - (penSize / 2));

        if(touchingLast){
            this.newTexture.SetPixels(x, y, penSize, penSize, color);

            float xDistance = Mathf.Abs(lastX-(float)x);
            float yDistance = Mathf.Abs(lastY-(float)y);

            if(xDistance < (penSize * lerpMultiplier) && yDistance < (penSize * lerpMultiplier) ){
                print("lerp");
                for(float t = 0.01f; t < 1.00f; t += 0.01f){
                    int lerpX = (int) Mathf.Lerp(lastX, (float)x, t);
                    int lerpY = (int) Mathf.Lerp(lastY, (float)y, t);
                    this.newTexture.SetPixels(lerpX, lerpY, penSize, penSize, this.color);
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

    public void ResetWhiteboard(){
        // this.texture.SetPixels(textColor);
        this.newTexture.SetPixels32(textColor);
        this.newTexture.Apply();
    }
}
