using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RightControllerRay : MonoBehaviour
{
    public LayerMask layer;
    public GameObject pokeMat;
    public float distance = 5f;

    private GameObject cardHitObj;
    private Renderer rendererObj;
    private bool cardHit = false;
    private bool isBlinking = false;
    private float originalMetallic;

    public GameObject IsRayHit()
    {
        if (cardHit)
        {
            return cardHitObj;
        }
        else { return null; }
    }
    public void StopBlinking()
    {
        if (cardHitObj != null)
        {
            isBlinking = false;
            StopCoroutine(Blink());
            if (rendererObj != null)
            {
                rendererObj.material.SetFloat("_Metallic", originalMetallic);
                rendererObj.material.SetFloat("_Smoothness", 0.5f);
            }

        }

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
    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue);
        if (Physics.Raycast(transform.position, transform.forward * distance, out hit, distance, layer))
        {
            cardHit = true;
            cardHitObj = hit.collider.gameObject;
            pokeMat.GetComponent<PokeScript>().ChangeColorToGreen();
            if (!isBlinking)
            {
                SetRenderer();
                isBlinking = true;
                StartCoroutine(Blink());
            }

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

    }
}
