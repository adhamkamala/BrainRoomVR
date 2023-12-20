using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnSystem : MonoBehaviour
{
    public GameObject boardPrefab;
    public Transform[] spawnLocations;
    public GameObject boardsSystem;

    private List<Transform> occupiedSpawnLocations = new List<Transform>();
    private bool isLocationFree = false;

    public void SpawnBoard()
    {
        if (boardPrefab != null && spawnLocations.Length > 0)
        {
            foreach (Transform spawnPoint in spawnLocations)
            {
                if (!occupiedSpawnLocations.Contains(spawnPoint))
                {
                    GameObject gameObject = Instantiate(boardPrefab, spawnPoint.position, spawnPoint.rotation);
                    gameObject.name = "Whiteboard(NormalBoard)";
                    boardsSystem.GetComponent<BoardsSystem>().SetSelectedBoard(gameObject);
                    occupiedSpawnLocations.Add(spawnPoint);
                    return;
                }
            }
            isLocationFree = false;
        }
    }
    public bool GetLocationBool()
    {
        return isLocationFree;
    }
    public void RemoveFromListPoints(Transform transform)
    {

        for (int i = 0; i < occupiedSpawnLocations.Count; i++)
        {
            if (occupiedSpawnLocations[i].position == transform.position)
            {
                occupiedSpawnLocations.RemoveAt(i);
                break;
            }
        }
    }
    public void DestoryAll()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>()
            .Where(gameObject => gameObject.name == "Whiteboard(NormalBoard)").ToArray();
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        occupiedSpawnLocations = new List<Transform>();

    }

    private void Start()
    {
        isLocationFree = true;
    }
}
