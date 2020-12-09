using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public GameObject scoretext;
    private float avgFrameRate;
    public Image healthBar;
    public Image shieldBar;
    private void Update()
    {
        avgFrameRate = Mathf.Round(Time.frameCount / Time.time);
        scoretext.GetComponent<Text>().text = UnifiedPlayerControl.totalSpeed.ToString("F0") + " mph" +
            "\n" + avgFrameRate + " fps" +
            "\n" + LaserFire.currentWeaponAmmo.ToString("F0") + " : ammo left" +
            "\n" + UnifiedPlayerControl.throttle.ToString("F3") +
             "\n" + DamagePlayer.playerHealth.ToString("F0") +
             "\n" + DamagePlayer.playerShield.ToString("F0");
        healthBar.fillAmount = DamagePlayer.playerHealth / 100;
        shieldBar.fillAmount = DamagePlayer.playerShield / 100;
    }


}
