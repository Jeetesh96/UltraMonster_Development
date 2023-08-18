using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class Vignette : ImageEffectShaderBase
{
    public Vector2 offset;
    public float exp;
    public Color colr;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        v_effectMat.SetFloat("_Xoffset", offset.x);
        v_effectMat.SetFloat("_Yoffset", offset.y);
        v_effectMat.SetFloat("_Exp", exp);
        v_effectMat.SetColor("_VignetteColor", colr);
        v_effectMat.SetFloat("_screenRatio" , (Screen.width * 1f) / (Screen.height * 1f));
        Graphics.Blit(source, destination, v_effectMat);
    }
}
