using UnityEngine;
using System.Collections;

public class PostEffect: MonoBehaviour
{
    // Thanks to Will Weissman's tutorial

    Camera attachedCamera;
    public Shader PostOutline;
    public Shader YellowOutline;
    public Shader DrawSimple;
    Camera tempCam1;
    Camera tempCam2;
    Material PostMat;
    Material PostMat2;

    private void Start()
    {
        attachedCamera = GetComponent<Camera>();
        tempCam1 = new GameObject().AddComponent<Camera>();
        tempCam1.enabled = false;
        tempCam2 = new GameObject().AddComponent<Camera>();
        tempCam2.enabled = false;
        PostMat = new Material(PostOutline);
        PostMat2 = new Material(YellowOutline);
    }
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
        // set up temp cams
        tempCam1.CopyFrom(attachedCamera);
        tempCam1.clearFlags = CameraClearFlags.Color;
        tempCam1.backgroundColor = Color.black;
        tempCam2.CopyFrom(tempCam1);

        // set up culling mask
        tempCam1.cullingMask = 1 << LayerMask.NameToLayer("Selected");
        tempCam2.cullingMask = 1 << LayerMask.NameToLayer("Targetable");

        // make tempRTs
        RenderTexture tempRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);
        RenderTexture tempRT2 = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);

        tempRT.Create();
        tempRT2.Create();

        tempCam1.targetTexture = tempRT;
        tempCam2.targetTexture = tempRT2;

        tempCam1.RenderWithShader(DrawSimple, "");
        tempCam2.RenderWithShader(DrawSimple, "");

        Graphics.Blit(source, destination);
        //PostMat.SetTexture("_SceneTex", source);
        Graphics.Blit(tempRT, destination, PostMat);
        Graphics.Blit(tempRT2, destination, PostMat2);

        tempRT.Release();
        tempRT2.Release();
    }
}