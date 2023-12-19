using UnityEngine;

public class MainSystem : MonoBehaviour
{
    private int mode;
    public int WhatMode()
    {
        return mode;
    }
    public void ChangeMode(int modeNumber)  // 0 --> whiteboard; 1-->mindmap
    {
        mode = modeNumber;
    }
}
