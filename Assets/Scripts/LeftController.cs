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
    public GameObject mainSystem;
    public GameObject cardSystem;
    public GameObject uiSystem;


    private GameObject cardTmp;
    private GameObject cardTmpMindMap;
    private GameObject locationTmp;
    private bool grabBool = false;
    private bool restBool = false;
    private bool optionsPressed = false;
    private bool mindMapMovement = false;
    private Vector3 rayPosition;

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
        if (mindMapMovement == true)
        {
            cardTmpMindMap.transform.position = new Vector3(rayPosition.x, rayPosition.y, cardTmpMindMap.transform.position.z);
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
                        if (mainSystem.GetComponent<MainSystem>().WhatMode() == 0)
                        {
                            if (cardTmp != null)
                            {
                                Destroy(cardTmp.gameObject);
                                grabBool = false;
                                restBool = false;
                                optionsPressed = false;
                              
                            }

                        } else
                        {
                            if (cardTmp != null)
                            {
                              //  cardSystem.GetComponent<CardSystem>().DeleteCard(cardTmp);
                                Destroy(cardTmp.gameObject);
                            } else if (cardTmpMindMap != null)
                            {
                                cardSystem.GetComponent<CardSystem>().DeleteCard(cardTmpMindMap);
                            } else if (cardTmp == cardTmpMindMap)
                            {
                                Destroy(cardTmp.gameObject);
                            }


                        }
                        leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                        break;
                    case "GreenExpandButton":
                        if (mainSystem.GetComponent<MainSystem>().WhatMode() == 0)
                        {
                            _ = aiSystem.GetComponent<OpenAIController>().ModeWhiteBoardExtend(boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponent<BoardScript>().GetTopicTxt(), boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponent<BoardScript>().GetAnswerTxt());
                        }
                        break;
                    case "Replace":
                        recordSystem.GetComponent<RecordSystem>().EnableReplace();
                        recordSystem.GetComponent<RecordSystem>().SetGameObjectTmp(cardTmpMindMap); // obsulete since SetCardToReplace does same
                        recordSystem.GetComponent<RecordSystem>().StartRecording();
                        break;

                    case "ReplaceAI":
                        cardSystem.GetComponent<CardSystem>().SetCardToReplace(cardTmpMindMap);
                        Debug.Log(cardTmpMindMap);
                        Debug.Log(cardTmpMindMap.name);
                        Debug.Log(cardTmpMindMap.GetComponent<Node>().nodeName);
                        _ = aiSystem.GetComponent<OpenAIController>().ModeMindMapReplaceAI(cardTmpMindMap.gameObject.GetComponent<Node>().nodeName);
                        optionsPressed = false;
                        cardTmpMindMap.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
                        leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                        // gen 3 cards with title
                        break;
                    case "Relocate":
                        cardSystem.GetComponent<CardSystem>().RelocateCard(cardTmpMindMap) ;
                        leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                        break;
                    case "AutoSort":
                        aiSystem.GetComponent<OpenAIController>().ModeMindMapAutoSort(cardTmp.GetComponent<CardScript>().subTitleTxt.text);
                        leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                        Destroy(cardTmp);
                        break;
                }
            }
          

        }

        if (leftPoke.GetComponent<LeftControllerRay>().IsLayerCard())
        {
            Debug.Log(leftPoke.GetComponent<LeftControllerRay>().IsRayHit());
            tmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
            if(tmp!=null)
            {
            switch (tmp.gameObject.name) {
                case "DeleteBoardBtn":
                    spawnSystem.GetComponent<SpawnSystem>().RemoveFromListPoints(tmp.gameObject.transform.parent.gameObject.transform);
                    Destroy(tmp.gameObject.transform.parent.gameObject);
                    break;
                    case "ButtonMindMap":
                        uiSystem.GetComponent<UISystem>().ModeMindMap();
                    break;
                    case "ButtonWhiteBoard":
                        uiSystem.GetComponent<UISystem>().ModeWhiteBoard();
                        break;
                    case var str when str.Contains("MindMap(ReplaceAICard)"):
                        cardSystem.GetComponent<CardSystem>().ReplaceCard(tmp.gameObject.GetComponent<CardScript>().subTitleTxt.text);
                        cardSystem.GetComponent<CardSystem>().DestroyAICards();
                        break;
                    case var str when str.Contains("MindMap(NormalCard)"):
                        if (cardTmp != null) {
                            cardSystem.GetComponent<CardSystem>().UserAttachCardNode(tmp.gameObject.GetComponent<Node>().nodeName, cardTmp);
                            Destroy(cardTmp);
                            grabBool = false;
                            restBool = false;

                        }
                        break;
                    default:
                        break;
            }
            }
            
        }
    
    }
    void OnLeftSecondaryPressed(InputAction.CallbackContext context)
    {
        if (mainSystem.GetComponent<MainSystem>().WhatMode() == 0) {
            if (leftPoke.GetComponent<LeftControllerRay>().IsLayerCard())
            {
                cardTmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
            }
            else if (leftPoke.GetComponent<LeftControllerRay>().IsLayerPosition())
            {
                locationTmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
            }
            if (cardTmp != null && leftPoke.GetComponent<LeftControllerRay>().IsLayerCard())
            {
                grabBool = true;
                cardTmp.GetComponent<CardScript>().GetLocationHolding().GetComponent<LocationHighlighter>().changeAvailable(true);

            }
            else
            {
                grabBool = false;
            }

            if (locationTmp != null && leftPoke.GetComponent<LeftControllerRay>().IsLayerPosition())
            {

                restBool = false;
                cardTmp.transform.parent = null;
                cardTmp.transform.parent = locationTmp.transform;
                cardTmp.transform.localPosition = new Vector3(0.1f, 0f, 0f);
                cardTmp.transform.localRotation = Quaternion.identity;
                cardTmp.transform.localScale = new Vector3(1f, 1f, 1f);
                leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
            }
        } else
        {
            mindMapMovement = !mindMapMovement;

        }


    }
    void OnLeftXPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Left X button pressed!");
        if (cardTmp!=null)
        {
            if (grabBool == true)
            {
                Transform parentObject = cardTmp.transform.parent;
                cardTmp.transform.parent = null;
                cardTmp.transform.Rotate(Vector3.up, -90f);
                cardTmp.transform.parent = parentObject;
                if (mainSystem.GetComponent<MainSystem>().WhatMode() == 0)
                {
                    leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToPostion();

                }

                grabBool = false;
                restBool = true;
            }
            else
            {
                if (cardTmp.transform.rotation.y != 0f)
                {
                    Transform parentObject = cardTmp.transform.parent;
                    cardTmp.transform.parent = null;
                    cardTmp.transform.Rotate(Vector3.up, 90f);
                    cardTmp.transform.parent = parentObject;

                }

                leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                grabBool = true;
                restBool = false;
            }
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
        else if (cardTmpMindMap != null & !optionsPressed)
        {
            optionsPressed = true;
            cardTmpMindMap.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
            Transform upperBar = cardTmpMindMap.transform.Find("UpperBar");
            Transform autoSort = upperBar.Find("AutoSort");
            autoSort.gameObject.SetActive(!optionsPressed);
                leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCardOptions();
        }
        else if (cardTmpMindMap != null & optionsPressed)
        {
            optionsPressed = false;
            cardTmpMindMap.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
            leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
        }
    }
    void OnLeftMenuPressed(InputAction.CallbackContext context)
    {
        // show modes
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

    public void AttachCard(GameObject card)
    {
        cardTmp = card;
        grabBool = true;
        optionsPressed = false;

        Transform upperbar = card.transform.Find("UpperBar");
        if (upperbar != null)
        {
            foreach (Transform child in upperbar)
            {
                GameObject childObject = child.gameObject;
                if (childObject.name == "RedDeleteButton" || childObject.name == "AutoSort")
                {
                    childObject.SetActive(true);
                }
                else
                {
                    childObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("Upperbar not found among the children of the parent GameObject.");
        }
    }

    public void SetCardTmpMindMap(GameObject gmo)
    {
        cardTmpMindMap = gmo;
    }

    public void SetRayPosition(Vector3 pos)
    {
        rayPosition = pos;
    }
    public bool GetMovementBool()
    {
        return mindMapMovement;
    }
}
