using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    private int mode;
    // Start is called before the first frame update
    void Start()
    {
        mode = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int WhatMode()
    {
        return mode;
    }
    public void ChangeMode(int modeNumber)  // 0 --> whiteboard; 1-->mindmap
    {
        mode = modeNumber;
    }
}
