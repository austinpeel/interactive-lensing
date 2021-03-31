using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyManager : MonoBehaviour
{
    public Sprite[] galaxies;

    public Transform modelGalaxy;
    public Transform targetGalaxy;

    private int index;
    private int numGalaxies;

    private SpriteRenderer modelGalaxySR;
    private SpriteRenderer targetGalaxySR;

    private void Start()
    {
        numGalaxies = galaxies.Length;
        modelGalaxySR = modelGalaxy.GetComponent<SpriteRenderer>();
        targetGalaxySR = targetGalaxy.GetComponent<SpriteRenderer>();

        if (galaxies.Length > 0)
        {
            modelGalaxySR.sprite = galaxies[index];
            targetGalaxySR.sprite = galaxies[index];
        }
    }

    private void SetGalaxyImages(int index, float rotationAngle)
    {
        this.index = index;
        modelGalaxySR.sprite = galaxies[this.index];
        targetGalaxySR.sprite = galaxies[this.index];
        modelGalaxy.localRotation = Quaternion.Euler(0, 0, rotationAngle);
        targetGalaxy.localRotation = Quaternion.Euler(0, 90, rotationAngle);
    }

    public void ShowNextGalaxy()
    {
        //print("Galaxy Manager > Heard match");
        if (galaxies.Length > 0)
        {
            index = (index + 1) % numGalaxies;
            print("galaxy index " + index);
            SetGalaxyImages(index, Random.Range(0, 4) * 90);
        }
    }

    public void Restart()
    {
        if (galaxies.Length > 0)
        {
            SetGalaxyImages(0, 0);
        }
    }
}
