using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/*  
   ORIGINAL COLORS 
      0,   0,   0
    180, 180, 180
    216, 216, 216
    256, 256, 256
                    */
public class PaletteSwap : MonoBehaviour
{
    public Material material;
    public List<Color> colors;
    protected bool updateTexture;
    protected static readonly int PaletteTex = Shader.PropertyToID("_PaletteTex");
    protected void Awake()
    {
        UpdatePalette();
    }

    public void OnValidate()
    {
        if (Application.isPlaying)
        {
            updateTexture = true;
            return;
        }

        UpdatePalette();
    }

    protected void Update()
    {
        if (updateTexture)
        { 
            updateTexture = false;
            UpdatePalette(); 
        }
    }

    public virtual void UpdatePalette()
    {
         material.SetTexture(PaletteTex, GetTexture());
    }

    protected Texture2D GetTexture()
    {
        Texture2D newTexture = new Texture2D(colors.Count, 1, TextureFormat.RGBA32, false);

        for (int i = 0; i < colors.Count; i++)
        {
            newTexture.SetPixel(i, 0, colors[i]);
        }

        newTexture.filterMode = FilterMode.Point;
        newTexture.wrapMode = TextureWrapMode.Clamp;
        newTexture.Apply();
        return newTexture;
    }
}