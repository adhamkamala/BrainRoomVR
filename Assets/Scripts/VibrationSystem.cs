using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VibrationSystem : MonoBehaviour
{
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public float vibrationDuration = 0.2f;
    public float successAmplitude = 0.5f;
    public void HapticLeft()
    {
        try
        {
            leftController.SendHapticImpulse(successAmplitude, vibrationDuration);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in HapticLeft: {ex.Message}");
        }


    }
    public void HapticRight()
    {

        try
        {
            rightController.SendHapticImpulse(successAmplitude, vibrationDuration);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in HapticRight: {ex.Message}");
        }
    }

}
