using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeScript : MonoBehaviour
{
    private Renderer renderer;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        material = renderer.material;
    }
    public void ChangeColorToGreen()
    {
        material.color= Color.green;
    }
    public void ChangeColorToWhite()
    {
        material.color = Color.white;
    }
}
