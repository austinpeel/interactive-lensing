using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceDistance : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 120);
    }

    public void SetDistance(float distance)
    {
        float zPosition = 122.5f - distance;
        transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
    }
}
