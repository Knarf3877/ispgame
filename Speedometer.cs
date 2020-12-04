using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public GameObject scoretext;
    private float avgFrameRate;
    private void Update()
    {
        avgFrameRate = Mathf.Round(Time.frameCount / Time.time);
        scoretext.GetComponent<Text>().text = UnifiedPlayerControl.totalSpeed.ToString("F0") + " mph" +
            "\n" + avgFrameRate + " fps" +
            "\n" + LaserFire.currentWeaponAmmo.ToString("F0") + " : ammo left" +
            "\n" + UnifiedPlayerControl.throttle.ToString("F3");
    }

}
