using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.white;
    public Color selectedColor = Color.white;
    public GameEvent gameEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameEvent != null)
        {
            gameEvent.Raise();
            targetImage.color = normalColor;
        }
        else
        {
            Debug.Log("No gameEvent attached");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.color = selectedColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.color = normalColor;
        }
    }

    private void OnValidate()
    {
        if (targetImage != null)
        {
            targetImage.color = normalColor;
        }
    }
}
