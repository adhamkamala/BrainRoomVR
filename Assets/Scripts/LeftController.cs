using UnityEngine;
using UnityEngine.InputSystem;
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
    public GameObject audioSystem;
    public GameObject vibrationSystem;

    private GameObject cardTmp;
    private GameObject cardTmpMindMap;
    private GameObject locationTmp;
    private Vector3 rayPosition;
    private bool grabBool = false;
    private bool restBool = false;
    private bool optionsPressed = false;
    private bool mindMapMovement = false;

    public void AttachCard(GameObject card)
    {
        vibrationSystem.GetComponent<VibrationSystem>().HapticLeft();
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

    private void Start()
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
        if (grabBool == true & !optionsPressed & cardTmp)
        {
            //  cardTmp.transform.position = holdPosition.transform.position;
            cardTmp.transform.SetParent(holdPosition.transform);
            cardTmp.transform.localPosition = Vector3.zero;
            cardTmp.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
        if (restBool == true)
        {
            //cardTmp.transform.position = restPosition.transform.position;
            cardTmp.transform.SetParent(restPosition.transform);
            cardTmp.transform.localPosition = Vector3.zero;
            cardTmp.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
        }
        if (mindMapMovement == true)
        {
            cardTmpMindMap.transform.position = new Vector3(rayPosition.x, rayPosition.y, cardTmpMindMap.transform.position.z);
        }
    }
    private void OnLeftMainPressed(InputAction.CallbackContext context)
    {
        audioSystem.GetComponent<AudioSystem>().PlayPrimaryClickSound();
        GameObject tmp;
        if (leftPoke.GetComponent<LeftControllerRay>().IsLayerCardOption())
        {
            tmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
            if (tmp != null)
            {
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

                        }
                        else
                        {
                            if (cardTmp != null)
                            {
                                //  cardSystem.GetComponent<CardSystem>().DeleteCard(cardTmp);
                                Destroy(cardTmp.gameObject);
                            }
                            else if (cardTmpMindMap != null)
                            {
                                cardSystem.GetComponent<CardSystem>().DeleteCard(cardTmpMindMap);
                            }
                            else if (cardTmp == cardTmpMindMap)
                            {
                                Destroy(cardTmp.gameObject);
                            }
                            optionsPressed = false;
                            cardTmpMindMap.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);


                        }
                        leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                        break;
                    case "GreenExpandButton":
                        if (mainSystem.GetComponent<MainSystem>().WhatMode() == 0)
                        {
                           aiSystem.GetComponent<OpenAIController>().ModeWhiteBoardExtend(boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponent<BoardScript>().GetTopicTxt(), boardsSystem.GetComponent<BoardsSystem>().GetSelectedBoard().GetComponent<BoardScript>().GetAnswerTxt());
                        }
                        else
                        {
                            cardSystem.GetComponent<CardSystem>().NodeStorageToJson();
                            aiSystem.GetComponent<OpenAIController>().ModeMindMapExtend(cardTmpMindMap.GetComponent<Node>().nodeName);
                            leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                            optionsPressed = false;
                            cardTmpMindMap.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
                        }
                        break;
                    case "Replace":
                        recordSystem.GetComponent<RecordSystem>().EnableReplace();
                        recordSystem.GetComponent<RecordSystem>().SetGameObjectTmp(cardTmpMindMap); // obsulete since SetCardToReplace does same
                        recordSystem.GetComponent<RecordSystem>().StartRecording();
                        optionsPressed = false;
                        cardTmpMindMap.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
                        break;

                    case "ReplaceAI":
                        cardSystem.GetComponent<CardSystem>().SetCardToReplace(cardTmpMindMap);
                        aiSystem.GetComponent<OpenAIController>().ModeMindMapReplaceAI(cardTmpMindMap.gameObject.GetComponent<Node>().nodeName);
                        optionsPressed = false;
                        cardTmpMindMap.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
                        leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                        break;
                    case "Relocate":
                        leftPoke.GetComponent<LeftControllerRay>().StopBlinking();
                        cardSystem.GetComponent<CardSystem>().RelocateCard(cardTmpMindMap);
                        leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                        optionsPressed = false;
                        cardTmpMindMap.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
                        break;
                    case "AutoSort":
                        cardSystem.GetComponent<CardSystem>().NodeStorageToJson();
                        aiSystem.GetComponent<OpenAIController>().ModeMindMapAutoSort(cardTmp.GetComponent<CardScript>().subTitleTxt.text);
                        leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                        optionsPressed = false;
                        cardTmpMindMap.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
                        Destroy(cardTmp);
                        break;
                }
            }


        }

        if (leftPoke.GetComponent<LeftControllerRay>().IsLayerCard())
        {
            tmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
            if (tmp != null)
            {
                switch (tmp.gameObject.name)
                {
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
                        if (cardTmp != null)
                        {
                            leftPoke.GetComponent<LeftControllerRay>().StopBlinking();
                            cardSystem.GetComponent<CardSystem>().UserAttachCardNode(tmp.gameObject.GetComponent<Node>().nodeName, cardTmp);
                            Destroy(cardTmp);
                            grabBool = false;
                            restBool = false;

                        }
                        else
                        {
                            mindMapMovement = !mindMapMovement;
                        }
                        break;
                    case var str when str.Contains("Whiteboard(NormalCard)"):
                        if (mainSystem.GetComponent<MainSystem>().WhatMode() == 0)
                        {
                            cardTmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
                            if (cardTmp != null)
                            {
                                grabBool = true;
                                cardTmp.GetComponent<CardScript>().GetLocationHolding().GetComponent<LocationHighlighter>().changeAvailable(true);

                            }
                            else
                            {
                                grabBool = false;
                            }
                            mindMapMovement = false;
                        }
                        break;

                    default:
                        break;
                }
            }

        }

        if (leftPoke.GetComponent<LeftControllerRay>().IsLayerPosition())
        {

            tmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
            if (tmp != null)
            {
                switch (tmp.gameObject.name)
                {
                    case var str when str.Contains("Whiteboard(NormalCard)"):
                        if (mainSystem.GetComponent<MainSystem>().WhatMode() == 0)
                        {

                            locationTmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
                            if (cardTmp != null)
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
                        }
                        break;
                    case var str when str.Contains("OnBoardLocations"):

                        if (mainSystem.GetComponent<MainSystem>().WhatMode() == 0)
                        {

                            locationTmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
                            if (locationTmp != null && leftPoke.GetComponent<LeftControllerRay>().IsLayerPosition())
                            {

                                restBool = false;
                                cardTmp.transform.parent = null;
                                cardTmp.transform.parent = locationTmp.transform;
                                cardTmp.transform.localPosition = new Vector3(0.1f, 0f, 0f);
                                cardTmp.transform.localRotation = Quaternion.identity;
                                cardTmp.transform.localScale = new Vector3(1f, 1f, 1f);
                                leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
                                cardTmp = null;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }
    private void OnLeftSecondaryPressed(InputAction.CallbackContext context)
    {
        //audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickAudio();
        //if (mainSystem.GetComponent<MainSystem>().WhatMode() == 0) {
        //    if (leftPoke.GetComponent<LeftControllerRay>().IsLayerCard())
        //    {
        //        cardTmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
        //    }
        //    else if (leftPoke.GetComponent<LeftControllerRay>().IsLayerPosition())
        //    {
        //        locationTmp = leftPoke.GetComponent<LeftControllerRay>().IsRayHit();
        //    }
        //    if (cardTmp != null && leftPoke.GetComponent<LeftControllerRay>().IsLayerCard())
        //    {
        //        grabBool = true;
        //        cardTmp.GetComponent<CardScript>().GetLocationHolding().GetComponent<LocationHighlighter>().changeAvailable(true);

        //    }
        //    else
        //    {
        //        grabBool = false;
        //    }

        //    if (locationTmp != null && leftPoke.GetComponent<LeftControllerRay>().IsLayerPosition())
        //    {

        //        restBool = false;
        //        cardTmp.transform.parent = null;
        //        cardTmp.transform.parent = locationTmp.transform;
        //        cardTmp.transform.localPosition = new Vector3(0.1f, 0f, 0f);
        //        cardTmp.transform.localRotation = Quaternion.identity;
        //        cardTmp.transform.localScale = new Vector3(1f, 1f, 1f);
        //        leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
        //    }
        //} else
        //{
        //    mindMapMovement = !mindMapMovement;

        //}


    }
    private void OnLeftXPressed(InputAction.CallbackContext context)
    {
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickSound();
        if (cardTmp != null)
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
                else
                {
                    leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
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
    private void OnLeftYPressed(InputAction.CallbackContext context)
    {
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickSound();
        if (cardTmp != null & optionsPressed)
        {
            optionsPressed = false;
            cardTmp.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
            leftPoke.GetComponent<LeftControllerRay>().ChangeLayerToCards();
        }
        else if (cardTmp != null & !optionsPressed)
        {
            optionsPressed = true;
            cardTmp.transform.Find("UpperBar").gameObject.SetActive(optionsPressed);
            cardTmp.transform.SetParent(null);
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
    private void OnLeftMenuPressed(InputAction.CallbackContext context)
    {
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickSound();
        uiSystem.GetComponent<UISystem>().ToggleUI();

    }
    private void OnRightMainPressed(InputAction.CallbackContext context)
    {
        audioSystem.GetComponent<AudioSystem>().PlayPrimaryClickSound();
        var tmpGameObjectHit = rightPoke.GetComponent<RightControllerRay>().IsRayHit();
        if (tmpGameObjectHit)
        {
            switch (tmpGameObjectHit.gameObject.name)
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
    private void OnRightSecondaryPressed(InputAction.CallbackContext context)
    {

    }
    private void OnRightBPressed(InputAction.CallbackContext context)
    {
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickSound();
        recordSystem.GetComponent<RecordSystem>().EndRecording();
    }
    private void OnRightAPressed(InputAction.CallbackContext context)
    {
        audioSystem.GetComponent<AudioSystem>().PlaySecondaryClickSound();
        recordSystem.GetComponent<RecordSystem>().StartRecording();
    }
}
