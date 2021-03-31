using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class LensEffectSIE : MonoBehaviour
{
    // Assign in the inspector
    public Material material;

    [SerializeField, Range(0, 1)]
    private float thetaE = 0;
    [SerializeField, Range(-0.5f, 0.5f)]
    private float e1 = 0;
    [SerializeField, Range(-0.5f, 0.5f)]
    private float e2 = 0;

    [SerializeField, Range(-0.49f, 0.49f)]
    private float x0 = 0;
    [SerializeField, Range(-0.49f, 0.49f)]
    private float y0 = 0;

    // Assign in the inspector
    public LensRandomizerSIE target;
    public Image matchMeter;
    public Slider thetaESlider;
    public Slider e1Slider;
    public Slider e2Slider;

    // For broadcasting
    public GameEvent OnMatch;
    public GameEvent OnLoadLevel;

    private bool gameIsFrozen;

    private void Start()
    {
        SetShaderParams();
        ComputeError();
    }

    private void Update()
    {
        if (gameIsFrozen)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Cursor.visible = false;
            Vector2 screenPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float x0 = screenPosition.x - 0.5f;
            float y0 = screenPosition.y - 0.5f;

            //screenPosition = new Vector2(x, y);
            if (Mathf.Abs(x0) <= 0.49 && Mathf.Abs(y0) <= 0.49)
            {
                SetX0(x0);
                SetY0(y0);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.M) && target != null)
        {
            SetE1(target.lensParams.e1);
            SetE2(target.lensParams.e2);
            SetX0(target.lensParams.x0);
            SetY0(target.lensParams.y0);

        }
    }

    public void ComputeError()
    {
        if (target != null && matchMeter != null)
        {
            // theta_E
            float maxErrorThetaE = (target.lensParams.thetaE > 0.5f) ? target.lensParams.thetaE : 1 - target.lensParams.thetaE;
            float errorThetaE = Mathf.Abs(thetaE - target.lensParams.thetaE) / maxErrorThetaE;

            // e1
            float maxErrorE1 = 0.5f + Mathf.Abs(target.lensParams.e1);
            float errorE1 = Mathf.Abs(e1 - target.lensParams.e1) / maxErrorE1;

            // e2
            float maxErrorE2 = 0.5f + Mathf.Abs(target.lensParams.e2);
            float errorE2 = Mathf.Abs(e2 - target.lensParams.e2) / maxErrorE2;

            // x0
            float maxErrorX0 = 0.5f + Mathf.Abs(target.lensParams.x0);
            float errorX0 = Mathf.Abs(x0 - target.lensParams.x0) / maxErrorX0;

            // y0
            float maxErrorY0 = 0.5f + Mathf.Abs(target.lensParams.y0);
            float errorY0 = Mathf.Abs(y0 - target.lensParams.y0) / maxErrorY0;

            // Total
            float matchAmount = (5 - errorThetaE - errorE1 - errorE2 - errorX0 - errorY0) / 5;

            matchMeter.fillAmount = matchAmount;

            if (matchAmount >= 0.992f)
            {
                thetaE = target.lensParams.thetaE;
                e1 = target.lensParams.e1;
                e2 = target.lensParams.e2;
                x0 = target.lensParams.x0;
                y0 = target.lensParams.y0;
                SetShaderParams();
                matchMeter.fillAmount = 1;
                gameIsFrozen = true;
                Cursor.visible = true;
                if (OnMatch != null)
                {
                    //print("Lens Effect SIE > Raising OnMatch");
                    OnMatch.Raise();
                }
                //print("LensEffectSIE > Invoking LoadNewLevel");
                Invoke(nameof(LoadNewLevel), 2.5f);
            }
        }
    }

    private void LoadNewLevel()
    {
        if (OnLoadLevel != null)
        {
            //print("Lens Effect SIE > Raising OnLoadLevel");
            OnLoadLevel.Raise();
        }
        target.RandomizeLensParameters();
        gameIsFrozen = false;
        ResetParameters();
    }

    public void ResetParameters()
    {
        bool originalStatus = gameIsFrozen;
        ResumeGame();
        SetThetaE(0);
        SetE1(0);
        SetE2(0);
        SetX0(0);
        SetY0(0);
        ComputeError();
        gameIsFrozen = originalStatus;
    }

    public void FreezeGame()
    {
        gameIsFrozen = true;
    }

    public void ResumeGame()
    {
        gameIsFrozen = false;
    }

    // OnRenderImage() is called when the camera has finished rendering
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            Graphics.Blit(source, destination, material);
        }
    }

    private void OnValidate()
    {
        SetShaderParams();
    }

    private void SetShaderParams()
    {
        if (material != null)
        {
            material.SetFloat("_ThetaE", thetaE);
            material.SetFloat("_E1", e1);
            material.SetFloat("_E2", e2);
            material.SetFloat("_X0", x0);
            material.SetFloat("_Y0", y0);
        }
    }

    public void SetThetaE(float newThetaE)
    {
        if (gameIsFrozen)
        {
            return;
        }

        thetaE = newThetaE;
        if (material != null)
        {
            material.SetFloat("_ThetaE", thetaE);
            ComputeError();

            // Make sure the UI slider matches the true value
            if (thetaESlider != null)
            {
                thetaESlider.value = thetaE;
            }
        }
    }

    public void SetE1(float newE1)
    {
        if (gameIsFrozen)
        {
            return;
        }

        e1 = newE1;
        if (material != null)
        {
            material.SetFloat("_E1", e1);
            ComputeError();

            // Make sure the UI slider matches the true value
            if (e1Slider != null)
            {
                e1Slider.value = e1;
            }
        }
    }

    public void SetE2(float newE2)
    {
        if (gameIsFrozen)
        {
            return;
        }

        e2 = newE2;
        if (material != null)
        {
            material.SetFloat("_E2", e2);
            ComputeError();
        }

        // Make sure the UI slider matches the true value
        if (e2Slider != null)
        {
            e2Slider.value = e2;
        }
    }

    private void SetX0(float newX0)
    {
        if (gameIsFrozen)
        {
            return;
        }

        x0 = newX0;
        if (material != null)
        {
            material.SetFloat("_X0", x0);
            ComputeError();
        }
    }

    private void SetY0(float newY0)
    {
        if (gameIsFrozen)
        {
            return;
        }

        y0 = newY0;
        if (material != null)
        {
            material.SetFloat("_Y0", y0);
            ComputeError();
        }
    }
}
