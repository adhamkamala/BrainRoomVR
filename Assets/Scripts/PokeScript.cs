using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeScript : MonoBehaviour
{
    private Renderer renderer;
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
        renderer = GetComponent<Renderer>();
        material = renderer.material;
    }
}
