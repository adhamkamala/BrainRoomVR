using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardSystem : MonoBehaviour
{
    public GameObject cardsPrf;
    public GameObject boardObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.oKey.wasPressedThisFrame) {

            initCardObj();
        }
    }
    void initCardObj() {
        if (boardObj.GetComponent<BoardSystem>().CanCreate())
        {
            boardObj.GetComponent<BoardSystem>().CardLocator(Instantiate(cardsPrf, new Vector3(0f, 0f, 0f), Quaternion.identity));
        }
    }
}
