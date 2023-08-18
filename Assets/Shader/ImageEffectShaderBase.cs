using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEffectShaderBase : MonoBehaviour
{
    public Shader effectShader;
    internal Material v_effectMat;

    private void OnEnable()
    {
        if(effectShader == null)
        {
            return;
        }

        v_effectMat = new Material(effectShader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, v_effectMat);
    }
}
