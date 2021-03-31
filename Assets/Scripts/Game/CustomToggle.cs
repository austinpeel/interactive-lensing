using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour, IPointerClickHandler
{
    public Image targetImage;

    public GameEvent OnToggle;
    public bool isOn;

    private void Awake()
    {
        if (targetImage != null)
        {
            targetImage.gameObject.SetActive(isOn);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isOn = !isOn;
        ToggleChecked();

        if (OnToggle != null)
        {
            //print("CustomToggle > Raising OnToggle");
            OnToggle.Raise();
        }
        // throw new System.NotImplementedException();
    }

    private void ToggleChecked()
    {
        if (targetImage != null)
        {
            targetImage.gameObject.SetActive(!targetImage.gameObject.activeInHierarchy);
        }
    }

    private void OnValidate()
    {
        targetImage.gameObject.SetActive(isOn);
    }
}
