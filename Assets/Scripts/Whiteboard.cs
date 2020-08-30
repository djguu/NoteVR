using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;      //VERIFICAR O QUE É ISTO

public class Whiteboard : NetworkBehaviour
{

    private int textureSize = 4096;
    private int penSize = 1;

    public int lerpMultiplier = 6;
    private Texture2D texture;
    private Color32[] color;

    private bool touching, touchingLast;
    private float posX, posY;
    private float lastX, lastY;

    private string objectType;

    private Color32[] textColor = new Color32[] {};

    // Start is called before the first frame update
    void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();

        this.texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false, true);
        
        // textColor =  this.texture.GetPixels();
        this.textColor = new Color32[textureSize * textureSize];

        for(var i = 0; i < textColor.Length; ++i)
        {
            this.textColor[i] = Color.white;
        }

        ResetWhiteboard();

        renderer.material.mainTexture = this.texture;
    }

    [Client]
    void Update()
    {
        if(this.objectType == "Eraser"){
            this.penSize = 5;
        }
        else{
            this.penSize = 1;
        }
        // Debug.Log(posX + " " + posY);
        int x = (int) (posX * textureSize - (penSize / 2));
        int y = (int) (posY * textureSize - (penSize / 2));

        if(this.touchingLast){
            // if(isServer){
            //     RpcDraw(x, y, penSize, color, lastX, lastY);
            // }
            // else{
            //     CmdDraw(x, y, this.penSize, this.color, this.lastX, this.lastY);
            // }
            CmdDraw(x, y, this.penSize, this.color, this.lastX, this.lastY);
        }

        this.lastX = (float)x;
        this.lastY = (float)y;

        this.touchingLast = this.touching;
       
    }

    [Command(ignoreAuthority=true)]
    void CmdDraw(int x, int y, int penSize, Color32[] color, float lastX, float lastY){
        RpcDraw(x, y, penSize, color, lastX, lastY);
    }

    [ClientRpc]
    void RpcDraw(int x, int y, int penSize, Color32[] color, float lastX, float lastY){
        Draw(x, y, penSize, color, lastX, lastY);
        
    }

    void Draw(int x, int y, int penSize, Color32[] color, float lastX, float lastY){
        this.texture.SetPixels32(x, y, penSize, penSize, color);

        float xDistance = Mathf.Abs(lastX-(float)x);
        float yDistance = Mathf.Abs(lastY-(float)y);

        if(xDistance < (penSize * 20) && yDistance < (penSize * 20) ){
            for(float t = 0.01f; t < 1.00f; t += 0.01f){
                int lerpX = (int) Mathf.Lerp(lastX, (float)x, t);
                int lerpY = (int) Mathf.Lerp(lastY, (float)y, t);
                this.texture.SetPixels32(lerpX, lerpY, penSize, penSize, color);
            }
        }
        this.texture.Apply();
    }

    public void ToggleTouch(bool touching){
        this.touching = touching;
    }

    public void SetTouchPosition(float x, float y){
        this.posX = x;
        this.posY = y;
    }

    public void SetColor(Color color){
        this.color = Enumerable.Repeat<Color32>(color, penSize * penSize).ToArray<Color32>();
    }

    public void SetObjectType(string type){
        this.objectType = type;
    }

    public void ResetWhiteboard(){
        this.texture.SetPixels32(this.textColor);
        this.texture.Apply();
    }
}
