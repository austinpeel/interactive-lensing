using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LensRandomizerSIE : MonoBehaviour
{
    // Assign in the inspector
    public Material material;

    // For broadcasting
    public GameEvent OnRandomizeLensParams;

    public LensParams lensParams;

    private void Start()
    {
        // Make sure that we don't match before the game has even started
        lensParams.thetaE = 0;
        lensParams.x0 = 10;
        if (material != null)
        {
            material.SetFloat("_ThetaE", lensParams.thetaE);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RandomizeLensParameters();
        }        
    }

    public void RandomizeLensParameters()
    {
        lensParams.thetaE = Random.Range(0.3f, 1);
        lensParams.e1 = Random.Range(-0.49f, 0.49f);
        lensParams.e2 = Random.Range(-0.49f, 0.49f);
        lensParams.x0 = Random.Range(-0.3f, 0.3f);
        lensParams.y0 = Random.Range(-0.4f, 0.4f);

        if (material != null)
        {
            material.SetFloat("_ThetaE", lensParams.thetaE);
            material.SetFloat("_E1", lensParams.e1);
            material.SetFloat("_E2", lensParams.e2);
            material.SetFloat("_X0", lensParams.x0);
            material.SetFloat("_Y0", lensParams.y0);
        }

        if (OnRandomizeLensParams != null)
        {
            //print("LensRandomizerSIE > Raising OnRandomizeLensParams");
            OnRandomizeLensParams.Raise();
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            Graphics.Blit(source, destination, material);
        }
    }

    public struct LensParams
    {
        public float thetaE;
        public float e1;
        public float e2;
        public float x0;
        public float y0;
    }
}
