using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerPointerView : MonoBehaviour
{
    public GameObject controller; // Reference to your VR controller
    public float distance = 10f;
    public RectTransform dotTransform;
    // Start is called before the first frame update
    void Start()
    {
       // dotTransform = GetComponentInChildren<RectTransform>();

        if (dotTransform == null)
        {
            Debug.LogError("No RectTransform found in children. Make sure the dot is a child of the canvas.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controller)
        {
            UpdatePointerPosition();
        }
    }
    void UpdatePointerPosition()
    {
        // Get the controller position and rotation
        Vector3 controllerPosition = controller.transform.position;
        Quaternion controllerRotation = controller.transform.rotation;

        // Calculate the position of the dot based on the controller's forward direction
        Vector3 dotPosition = controllerPosition + controllerRotation * Vector3.forward * distance;

        // Set the dot's position
        dotTransform.position = dotPosition;
    }
}
