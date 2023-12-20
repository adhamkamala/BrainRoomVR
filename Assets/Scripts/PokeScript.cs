using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeScript : MonoBehaviour
{
    private Renderer rendererObj;
    private Material material;

    public void ChangeColorToGreen()
    {
        material.color= Color.green;
    }
    public void ChangeColorToWhite()
    {
        material.color = Color.white;
    }
    private void Start()
    {
        rendererObj = GetComponent<Renderer>();
        material = rendererObj.material;
    }
}
