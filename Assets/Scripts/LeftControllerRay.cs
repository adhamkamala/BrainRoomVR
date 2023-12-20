using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class LeftControllerRay : MonoBehaviour
{
    public LayerMask layerCard;
    public LayerMask layerPosition;
    public LayerMask layerCardOptions;
    public GameObject pokeMat;
    public GameObject leftController;
    public GameObject mainSystem;
    public float distance = 5f;

    private GameObject cardHitObj;
    private LayerMask layerSelected;
    private Renderer rendererObj;
    private bool cardHit = false;
    private bool isBlinking = false;
    private float originalMetallic;

    public void StopBlinking()
    {
        if (cardHitObj!=null && rendererObj != null) {
            isBlinking = false;
            StopCoroutine(Blink());
            rendererObj.material.SetFloat("_Metallic", originalMetallic);
            rendererObj.material.SetFloat("_Smoothness", 0.5f); 

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
    
    private IEnumerator Blink()
    {
        while (isBlinking)
        {
            try
            {
                if (rendererObj != null)
                {
                    float newMetallic = Mathf.PingPong(Time.time, 1f);
                    rendererObj.material.SetFloat("_Metallic", newMetallic);
                    float newSmoothness = Mathf.PingPong(Time.time, 0.5f) + 0.5f;
                    rendererObj.material.SetFloat("_Smoothness", newSmoothness);
                }
      
            }
            catch (System.Exception)
            {

                throw;
            }
            yield return null;

        }
    }
    private void SetRenderer()
    {

        rendererObj = cardHitObj.GetComponent<Renderer>();
        originalMetallic = rendererObj.material.GetFloat("_Metallic");
    }
    private void Start()
    {
        layerSelected = layerCard;
    }
    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, UnityEngine.Color.red);
        if (Physics.Raycast(transform.position, transform.forward * distance, out hit, distance, layerSelected))
        {
            cardHit = true;
            cardHitObj = hit.collider.gameObject;
            if (!isBlinking)
            {
                SetRenderer();
                isBlinking = true;
                StartCoroutine(Blink());
            }

            if (cardHitObj.name.Contains("MindMap") && !leftController.GetComponent<LeftController>().GetMovementBool() && mainSystem.GetComponent<MainSystem>().WhatMode() == 1)
            {
                leftController.GetComponent<LeftController>().SetCardTmpMindMap(cardHitObj);
            }
            pokeMat.GetComponent<PokeScript>().ChangeColorToGreen();

        }
        else
        {
            if (isBlinking)
            {
                StopBlinking();
            }

            cardHit = false;
            pokeMat.GetComponent<PokeScript>().ChangeColorToWhite();
        }

        if (Physics.Raycast(transform.position, transform.forward * distance, out hit, distance))
        {
            leftController.GetComponent<LeftController>().SetRayPosition(hit.point);
        }


    }
}
