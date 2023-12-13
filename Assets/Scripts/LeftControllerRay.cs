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
    public float distance = 5f;
    public GameObject pokeMat;
    public GameObject leftController;

    private GameObject cardHitObj;
    private bool cardHit = false;
    private LayerMask layerSelected;
    private Renderer renderer;
    private bool isBlinking = false;
    private float originalMetallic;
    private float originalSmoothness;


    void Start()
    {
        layerSelected = layerCard;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, UnityEngine.Color.red);
            if (Physics.Raycast(transform.position, transform.forward * distance, out hit, distance, layerSelected))
            {
            cardHit= true;
            cardHitObj = hit.collider.gameObject;
            //renderer = cardHitObj.GetComponent<Renderer>();
            // renderer.material.color = UnityEngine.Color.green;
            if (!isBlinking)
            {
                SetRenderer();
                isBlinking = true;
                StartCoroutine(Blink());
            }

            if (cardHitObj.name.Contains("MindMap")&& !leftController.GetComponent<LeftController>().GetMovementBool()) {
                leftController.GetComponent<LeftController>().SetCardTmpMindMap(cardHitObj);
            }
            pokeMat.GetComponent<PokeScript>().ChangeColorToGreen();
       
            } else
        {
            StopBlinking();
            cardHit = false;
            pokeMat.GetComponent<PokeScript>().ChangeColorToWhite();
            //  leftController.GetComponent<LeftController>().SetRayPosition(hit.point);
        }

        if (Physics.Raycast(transform.position, transform.forward * distance, out hit, distance))
        {
            leftController.GetComponent<LeftController>().SetRayPosition(hit.point);
        }


    }
    private void StopBlinking()
    {
        if (cardHitObj!=null) {
            isBlinking = false;
            StopCoroutine(Blink());
            renderer.material.SetFloat("_Metallic", originalMetallic); // Reset the metallic value when blinking stops
            renderer.material.SetFloat("_Smoothness", 0.5f); // Set the smoothness value to 0.5 when blinking stops

        }

    }

    private IEnumerator Blink()
    {
        while (isBlinking)
        {
            float newMetallic = Mathf.PingPong(Time.time, 1f);
            renderer.material.SetFloat("_Metallic", newMetallic);
            float newSmoothness = Mathf.PingPong(Time.time, 0.5f) + 0.5f;
            renderer.material.SetFloat("_Smoothness", newSmoothness);
            yield return null;
        }
    }

    private void SetRenderer()
    {

        renderer = cardHitObj.GetComponent<Renderer>();
        originalMetallic = renderer.material.GetFloat("_Metallic");
        originalSmoothness = renderer.material.GetFloat("_Smoothness");

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
