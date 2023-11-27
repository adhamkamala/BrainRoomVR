using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RightControllerRay : MonoBehaviour
{
    public LayerMask layer;
    public float distance = 5f;
    public GameObject pokeMat;

    private bool cardHit = false;
    private GameObject cardHitObj;


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue);
            if (Physics.Raycast(transform.position, transform.forward * distance, out hit, distance, layer))
            {
            cardHit = true;
            cardHitObj = hit.collider.gameObject;
            pokeMat.GetComponent<PokeScript>().ChangeColorToGreen();

        }
        else
        {
            cardHit = false;
            pokeMat.GetComponent<PokeScript>().ChangeColorToWhite();
        }

    }

    public GameObject IsRayHit()
    {
        if (cardHit)
        {
            return cardHitObj;
        }
        else { return null; }
    }
}
