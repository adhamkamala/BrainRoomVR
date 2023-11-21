using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardsSystem : MonoBehaviour
{
    private GameObject selectedBoard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetSelectedBoard()
    {
        return selectedBoard;
    }

    public void SetSelectedBoard(GameObject board)
    {
        selectedBoard = board;
    }

}
