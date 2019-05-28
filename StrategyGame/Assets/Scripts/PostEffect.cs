using UnityEngine;
using System.Collections;

public class PostEffect: MonoBehaviour
{
    // Thanks to Will Weissman's tutorial

    Camera attachedCamera;
    public Shader PostOutline;
    public Shader DrawSimple;
    Camera tempCam;
    Material PostMat;
    public RenderTexture tempRT;

    private void Start()
    {
        attachedCamera = GetComponent<Camera>();
        tempCam = new GameObject().AddComponent<Camera>();
        tempCam.enabled = false;
        PostMat = new Material(PostOutline);
    }
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
        // set up temp cam
        tempCam.CopyFrom(attachedCamera);
        tempCam.clearFlags = CameraClearFlags.Color;
        tempCam.backgroundColor = Color.black;

        // set up culling mask
        tempCam.cullingMask = 1 << LayerMask.NameToLayer("Selected");

        // make tempRT
        RenderTexture tempRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);

        tempRT.Create();

        tempCam.targetTexture = tempRT;

        tempCam.RenderWithShader(DrawSimple, "");

        Graphics.Blit(source, destination);
        //PostMat.SetTexture("_SceneTex", source);
        Graphics.Blit(tempRT, destination, PostMat);

        tempRT.Release();
    }
}