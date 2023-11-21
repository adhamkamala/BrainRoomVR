using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unity.VisualScripting.Dependencies.NCalc;
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
    public GameObject leftPoke;
    public GameObject aiSystem;
    public GameObject holdPosition;
    public GameObject restPosition;
    public GameObject board;
    public GameObject spawnSystem;

    private bool grabBool = false;
    private bool restBool = false;
    private GameObject cardTmp;
    private GameObject locationTmp;
    private bool optionsPressed = false;

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

    private void Update()
    {
        if (grabBool == true & !optionsPressed)
        {
            cardTmp.transform.position = holdPosition.transform.position;
        }
        if (restBool == true)
        {
            cardTmp.transform.position = restPosition.transform.position;
        }
    }

    void OnLeftMainPressed(InputAction.CallbackContext context)
    {
        GameObject tmp;
        Debug.Log("Left Main button pressed!");
        if (leftPoke.GetComponent<LeftControllerRay>().IsLayerCardOption())
        {
           tmp =  leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
            Debug.Log(tmp.gameObject.name);
            switch (tmp.gameObject.name)
            {
                case "RedDeleteButton":
                    Debug.Log("Delete");
                    if (cardTmp != null) {
                        Destroy(cardTmp.gameObject);
                        grabBool = false;
                        restBool = false;
                        optionsPressed = false;
                        leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                    }
                    break;
                case "GreenExpandButton":
                    Debug.Log("Expand");
                    // take the question and the answers and ask chat gpt for more answers.. will mehr wissen...
                    aiSystem.GetComponent<OpenAIController>().SendMessageMore(board.GetComponent<BoardScript>().GetTopicTxt(), board.GetComponent<BoardScript>().GetAnswerTxt()); //pp
                    break;
                default:
                    Debug.Log("nun");
                    break;
            }

        }

        if (leftPoke.GetComponent<LeftControllerRay>().IsLayerCard())
        {
            tmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
            switch (tmp.gameObject.name) {
                case "DeleteBoardBtn":
                    spawnSystem.GetComponent<SpawnSystem>().RemoveFromListPoints(tmp.gameObject.transform.parent.gameObject.transform);
                    Destroy(tmp.gameObject.transform.parent.gameObject);
                    break;
            }
        }
    }
    void OnLeftSecondaryPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Left Secondary button pressed!");
    
        if (leftPoke.GetComponent<LeftControllerRay>().IsLayerCard())
        {
            cardTmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
        }
        else if (leftPoke.GetComponent<LeftControllerRay>().IsLayerPosition()) {
            locationTmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
        }
        if (cardTmp != null && leftPoke.GetComponent<LeftControllerRay>().IsLayerCard())
        {   
            grabBool = true;
            cardTmp.GetComponent<CardScript>().GetLocationHolding().GetComponent<LocationHighlighter>().changeAvailable(true);
            
        } else
        {
            grabBool= false;
        }
   
        if (locationTmp != null && leftPoke.GetComponent<LeftControllerRay>().IsLayerPosition())
        {
          
            restBool = false;
            cardTmp.transform.Rotate(Vector3.up, 90f);
            cardTmp.transform.position = new Vector3(locationTmp.transform.position.x + 0.005f, locationTmp.transform.position.y, locationTmp.transform.position.z);
            leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
        }

    }
    void OnLeftXPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Left X button pressed!");
        if (grabBool == true)
        {
            
            cardTmp.transform.Rotate(Vector3.up, -90f);
            leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToPostion();
            grabBool = false;
            restBool = true;
        } else
        {
            if (cardTmp.transform.rotation.y != 0f)
            {
                cardTmp.transform.Rotate(Vector3.up, 90f);
            }
            leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
            grabBool = true;
            restBool = false;
        }
    }
    void OnLeftYPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Left Y button pressed!");
        if (cardTmp!= null & optionsPressed)
        {
            optionsPressed = false;
            cardTmp.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
            leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
        } else if (cardTmp != null & !optionsPressed)
        {
            optionsPressed = true;
            cardTmp.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
            cardTmp.transform.position = holdPosition.transform.position;
            leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCardOptions();
        }
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
        aiSystem.GetComponent<OpenAIController>().testTrans();
    }
    void OnRightBPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Right B button pressed!");
        aiSystem.GetComponent<OpenAIController>().EndRecording();
    }
    void OnRightAPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Right A button pressed!");
        Debug.Log("Start Recording...");
        aiSystem.GetComponent<OpenAIController>().StartRecording();
    }
}
