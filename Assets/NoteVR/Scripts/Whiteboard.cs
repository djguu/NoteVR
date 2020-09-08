using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;

public class Whiteboard : NetworkBehaviour
{

    private int textureSize = 4096;
    private int penSize = 1;
    private int eraserSize = 5;
    public Texture2D texture;
    private Color32[] color;
    private Color32[] eraser;
    private Color32[] textColor = new Color32[] {};
    
    private bool touching, touchingLast;
    private float posX, posY;
    private float lastX, lastY;

    private string objectType;

    void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();

        textColor = new Color32[textureSize * textureSize];

        for(var i = 0; i < textColor.Length; ++i)
        {
            textColor[i] = Color.white;
        }

        eraser = new Color32[eraserSize * eraserSize];

        for(var i = 0; i < eraser.Length; ++i)
        {
            eraser[i] = Color.white;
        }

        texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false, true);

        if(File.Exists(Application.persistentDataPath + "/" + "Whiteboard.png")){
            texture.LoadImage(File.ReadAllBytes(Application.persistentDataPath + "/" + "Whiteboard.png"));   
        }
        else{
            ResetWhiteboard();
            
        }
        renderer.material.mainTexture = texture;
    }

    [Client]
    void Update()
    {
        int x = (int) (posX * textureSize - (penSize / 2));
        int y = (int) (posY * textureSize - (penSize / 2));

        if(touchingLast){
            if(objectType == "Eraser"){
                CmdDraw(x, y, eraserSize, eraser, lastX, lastY);
            }
            else{
                CmdDraw(x, y, penSize, color, lastX, lastY);
            }
        }

        lastX = (float)x;
        lastY = (float)y;

        touchingLast = touching;
       
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
        texture.SetPixels32(x, y, penSize, penSize, color);

        float xDistance = Mathf.Abs(lastX-(float)x);
        float yDistance = Mathf.Abs(lastY-(float)y);

        if(xDistance < (penSize * 5) && yDistance < (penSize * 5) ){
            for(float t = 0.01f; t < 1.00f; t += 0.01f){
                int lerpX = (int) Mathf.Lerp(lastX, (float)x, t);
                int lerpY = (int) Mathf.Lerp(lastY, (float)y, t);
                texture.SetPixels32(lerpX, lerpY, penSize, penSize, color);
            }
        }
        texture.Apply();
    }

    public void ToggleTouch(bool touching){
        this.touching = touching;
    }

    public void SetTouchPosition(float x, float y){
        posX = x;
        posY = y;
    }

    public void SetColor(Color color){
        this.color = new Color32[penSize * penSize];
        for(var i = 0; i < this.color.Length; ++i)
        {
            this.color[i] = color;
        }
    }

    public void SetObjectType(string type){
        objectType = type;
    }

    public void ResetWhiteboard(){
        texture.SetPixels32(textColor);
        texture.Apply();
    }
}
