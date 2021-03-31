using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string sceneToLoad;
    public Image targetImage;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.white;
    public Color selectedColor = Color.white;

    public void OnPointerClick(PointerEventData eventData)
    {
        try
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        catch
        {
            print("Could not load scene : " + sceneToLoad);
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

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    try
    //    {
    //        SceneManager.LoadScene(sceneToLoad);
    //    }
    //    catch (System.Exception ex)
    //    {
    //        print("Could not load scene : " + sceneToLoad);
    //    }
    //}
}
