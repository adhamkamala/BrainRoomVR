using UnityEngine;

public class BoardsSystem : MonoBehaviour
{
    private GameObject selectedBoard;
    public GameObject GetSelectedBoard()
    {
        return selectedBoard;
    }
    public void SetSelectedBoard(GameObject board)
    {
        selectedBoard = board;
    }
}
