using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using static Unity.Burst.Intrinsics.X86.Avx;

public class LeftController : MonoBehaviour
{
    public InputActionReference leftMainButton;
    public InputActionReference leftSecondaryButton;
    public InputActionReference leftXbutton;
    public InputActionReference leftYButton;
    public InputActionReference leftMenuButton;
    public InputActionReference rightMainButton;
    public InputActionReference rightSecondaryButton;
    public InputActionReference rightBButton;
    public InputActionReference rightAButton;
    public GameObject leftPoke;
    public GameObject rightPoke;
    public GameObject aiSystem;
    public GameObject holdPosition;
    public GameObject restPosition;
    public GameObject boardsSystem;
    public GameObject spawnSystem;
    public GameObject recordSystem;

    private GameObject cardTmp;
    private GameObject locationTmp;
    private bool grabBool = false;
    private bool restBool = false;
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
        if (leftPoke.GetComponent<LeftControllerRay>().IsLayerCardOption())
        {
           tmp =  leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
            if (tmp != null) {
                switch (tmp.gameObject.name)
                {
                    case "RedDeleteButton":
                        if (cardTmp != null)
                        {
                            Destroy(cardTmp.gameObject);
                            grabBool = false;
                            restBool = false;
                            optionsPressed = false;
                            leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                        }
                        break;
                    case "GreenExpandButton":
                        _ = aiSystem.GetComponent<OpenAIController>().SendMessageMore(boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponent<BoardScript>().GetTopicTxt(), boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponent<BoardScript>().GetAnswerTxt());
                        break;
                }
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

    }
    void OnRightMainPressed(InputAction.CallbackContext context)
    {

        var tmp = rightPoke.GetComponent<RightControllerRay>().IsRayHit();
        if (tmp) {
            switch (tmp.gameObject.name)
            {
                case "ConfirmBtn":
                    recordSystem.GetComponent<RecordSystem>().OnConfirmClick();
                    break;
                case "CancelBtn":
                    recordSystem.GetComponent<RecordSystem>().OnCancelClick();
                    break;
            }
        }
      
    }
    void OnRightSecondaryPressed(InputAction.CallbackContext context)
    {
      
    }
    void OnRightBPressed(InputAction.CallbackContext context)
    {
       recordSystem.GetComponent<RecordSystem>().EndRecording();
    }
    void OnRightAPressed(InputAction.CallbackContext context)
    {
        recordSystem.GetComponent<RecordSystem>().StartRecording();
    }

}
