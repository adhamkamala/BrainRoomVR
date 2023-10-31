using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftController : MonoBehaviour
{
    public InputActionReference leftMainButton; // Main
    public InputActionReference leftSecondaryButton; // Secondary 
    public InputActionReference leftXbutton;
    public InputActionReference leftYButton;
    public InputActionReference leftMenuButton;
    public InputActionReference rightMainButton;
    public InputActionReference rightSecondaryButton;
    public InputActionReference rightBButton;
    public InputActionReference rightAButton;

    void Start()
    {
        leftMainButton.action.performed += OnLeftMainPressed;
        leftSecondaryButton.action.performed += OnLeftSecondaryPressed;
        leftXbutton.action.performed += OnLeftXPressed;
        leftYButton.action.performed += OnLeftYPressed;
        leftMenuButton.action.performed += OnLeftMenuPressed;
        rightMainButton.action.performed += OnRightMainPressed;
        rightSecondaryButton.action.performed += OnRightSecondaryPressed;
        rightBButton.action.performed += OnRightBPressed;
        rightAButton.action.performed += OnRightAPressed;
    }

  //  void OnMainButtonPressed(InputAction.CallbackContext context)
  //  {
   //     Debug.Log("Main button pressed!");
  //  }

    void OnLeftMainPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Left Main button pressed!");
    }
    void OnLeftSecondaryPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Left Secondary button pressed!");
    }
    void OnLeftXPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Left X button pressed!");
    }
    void OnLeftYPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Left Y button pressed!");
    }
    void OnLeftMenuPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Left Menu button pressed!");
    }
    void OnRightMainPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Right Main button pressed!");
    }
    void OnRightSecondaryPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Right Secondary button pressed!");
    }
    void OnRightBPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Right B button pressed!");
    }
    void OnRightAPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Right A button pressed!");
    }
}
