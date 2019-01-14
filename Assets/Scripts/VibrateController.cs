using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class VibrateController : MonoBehaviour {
    static float seconds = 0;
    IEnumerator lastCoroutine;

    public static void vibrateControllerForXSeconds(float seconds, float left, float right)
    {
        VibrateController.seconds = seconds;
        GamePad.SetVibration(PlayerIndex.One, left, right);
    }

    void FixedUpdate()
    {
        if(seconds != 0)
        {
            if (lastCoroutine != null) StopCoroutine(lastCoroutine);
            lastCoroutine = waitUntilTurnOff(seconds);
            StartCoroutine(lastCoroutine);
            seconds = 0;
        }
    }

    IEnumerator waitUntilTurnOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }

    public static void stopVibrations()
    {
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }
}
