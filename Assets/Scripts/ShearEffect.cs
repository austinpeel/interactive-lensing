using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShearEffect : MonoBehaviour
{
    // Assign in the inspector
    public Material material;

    [SerializeField, Range(-0.99f, 0.99f)]
    private float e1 = 0;
    [SerializeField, Range(-0.99f, 0.99f)]
    private float e2 = 0;

    //private void Awake()
    //{
    //    if (shader != null)
    //    {
    //        material = new Material(shader);
    //        print("Creating");
    //    }
    //}

    // OnRenderImage() is called when the camera has finished rendering
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void OnValidate()
    {
        if (material != null)
        {
            material.SetFloat("_E1", e1);
            material.SetFloat("_E2", e2);
        }
    }
}
