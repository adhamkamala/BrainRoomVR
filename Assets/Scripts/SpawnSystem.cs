using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnSystem : MonoBehaviour
{
    public GameObject boardPrefab;
    public Transform[] spawnLocations;
    private List<Transform> occupiedSpawnLocations = new List<Transform>();
    private bool isLocationFree = false;
    public GameObject boardsSystem;
    // Start is called before the first frame update
    void Start()
    {
        isLocationFree = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SpawnBoard();
        }
    }

    public void SpawnBoard()
    {
        if (boardPrefab != null && spawnLocations.Length > 0)
        {
            foreach (Transform spawnPoint in spawnLocations)
            {
                if (!occupiedSpawnLocations.Contains(spawnPoint))
                {
                    boardsSystem.GetComponent<BoardsSystem>().SetSelectedBoard(Instantiate(boardPrefab, spawnPoint.position, spawnPoint.rotation));
                    occupiedSpawnLocations.Add(spawnPoint);
                    return; // Spawned an object, exit the function
                }
            }
            Debug.Log("All spawn points are assigned.");
            isLocationFree = false;
        }
    }

    public bool GetLocationBool()
    {
        return isLocationFree;
    }

    public void RemoveFromListPoints(Transform t)
    {

        for (int i = 0; i < occupiedSpawnLocations.Count; i++)
        {
            if (occupiedSpawnLocations[i].position == t.position)
            {
                occupiedSpawnLocations.RemoveAt(i);
                break;
            }
        }
    }
}