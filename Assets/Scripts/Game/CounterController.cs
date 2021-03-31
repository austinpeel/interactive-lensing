using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterController : MonoBehaviour
{
    private int count;
    private TextMeshProUGUI counterTMP;

    private void Awake()
    {
        counterTMP = GetComponent<TextMeshProUGUI>();
        counterTMP.text = count.ToString();
    }

    public void IncrementCounter()
    {
        count += 1;
        counterTMP.text = count.ToString();
    }

    public int GetCount()
    {
        return count;
    }

    public void ResetCounter()
    {
        count = 0;
        counterTMP.text = count.ToString();
    }
}
