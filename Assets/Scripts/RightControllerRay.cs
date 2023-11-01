using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RightControllerRay : MonoBehaviour
{
    public LayerMask layer;
    public Transform gameObj;
    public float distance = 5f;
    public Material leftHandMaterial;
    private string rightPadName = "RightHandIndicator";
    private string leftPadName = "LeftHandIndicator";
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue);
            if (Physics.Raycast(transform.position, transform.forward * distance, out hit, distance, layer))
            {
       
            }

    }
}
