using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VibrationSystem : MonoBehaviour
{
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public float vibrationDuration = 0.2f;
    public float successAmplitude = 0.5f;
    public float failureAmplitude = 1f;

    public void HapticLeft()
    {
        leftController.SendHapticImpulse(successAmplitude, vibrationDuration);

    }
    public void HapticRight()
    {
        rightController.SendHapticImpulse(successAmplitude, vibrationDuration);

    }

    //private IEnumerator Vibrate(float amplitude, float duration, ActionBasedController controller)
    //{

    //    controller.SendHapticImpulse(amplitude, duration);
    //    yield return new WaitForSeconds(duration + 0.1f);
    //    controller.SendHapticImpulse(amplitude, duration);
    //}
}
