using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSpriteVisibility : MonoBehaviour
{
    public GameObject sprite;

    public void ToggleVisibility()
    {
        if (sprite.activeInHierarchy)
        {
            sprite.SetActive(false);
        }
        else
        {
            sprite.SetActive(true);
        }
    }
}
