using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeftControllerRay : MonoBehaviour
{
    public LayerMask layerCard;
    public LayerMask layerPosition;
    public LayerMask layerCardOptions;
    public float distance = 5f;
    private bool cardHit = false;
    private LayerMask layerSelected;
    private GameObject cardHitObj;
    public GameObject pokeMat;
    // Start is called before the first frame update
    void Start()
    {
        layerSelected = layerCard;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
            if (Physics.Raycast(transform.position, transform.forward * distance, out hit, distance, layerSelected))
            {
            cardHit= true;
            cardHitObj = hit.collider.gameObject;
           // Debug.Log(cardHitObj.name);
            pokeMat.GetComponent<PokeScript>().ChangeColorToGreen();
       
            } else
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
        } else { return null; }
    }
    public void ChangeLayerToCards()
    {
        layerSelected = layerCard;
    }

    public void ChangeLayerToPostion()
    {
        layerSelected = layerPosition;

    }

    public void ChangeLayerToCardOptions()
    {
        layerSelected = layerCardOptions;

    }

    public bool IsLayerCard()
    {
        return layerSelected == layerCard;
    }

    public bool IsLayerPosition()
    {
        return layerSelected == layerPosition;
    }

    public bool IsLayerCardOption()
    {
        return layerSelected == layerCardOptions;
    }
}
