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

    // Start is called before the first frame update
    void Start()
    {
        isLocationFree = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame) // for testing purposes
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
                    GameObject gmo = Instantiate(boardPrefab, spawnPoint.position, spawnPoint.rotation);
                    gmo.name = "Whiteboard(NormalBoard)";
                    boardsSystem.GetComponent<BoardsSystem>().SetSelectedBoard(gmo);
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

    public void DestoryAll()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>()
            .Where(go => go.name == "Whiteboard(NormalBoard)").ToArray();
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        occupiedSpawnLocations = new List<Transform>();

    }
}
