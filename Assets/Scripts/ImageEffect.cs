using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour {
    public Material mat;
    public float strength;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetFloat("_Strength", strength + 0.15f);
        mat.SetFloat("_UTime", Time.unscaledTime);
        Graphics.Blit(source, destination, mat);
    }
}
