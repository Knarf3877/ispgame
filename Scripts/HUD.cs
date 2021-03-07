using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUD : MonoBehaviour
{
    public GameObject scoretext;
    public GameObject weaponText;
    private float avgFrameRate;
    public Image healthBar;
    public Image shieldBar;
    private void Update()
    {
        avgFrameRate = Mathf.Round(Time.frameCount / Time.time);
        scoretext.GetComponent<Text>().text = UnifiedPlayerControl.totalSpeed.ToString("F0") + " mph" +
            "\n" + avgFrameRate + " fps" +
            "\n" + UnifiedPlayerControl.warpFuel.ToString("F0") + " : fuel left" +
            "\n" + UnifiedPlayerControl.throttle.ToString("F3") +
            "\n" + EnemyStats.totalDeaths.ToString("F0") + " : enemies killed";
        weaponText.GetComponent<TextMeshProUGUI>().text = WeaponName(LaserFire.currentWeapon) +
           "\n" + LaserFire.currentWeaponAmmo.ToString("F0");
        healthBar.fillAmount = DamagePlayer.playerHealth / 100;
        shieldBar.fillAmount = DamagePlayer.playerShield / 100;

    }

    string WeaponName(int weaponNum) {
        switch (weaponNum) {
            case 1:
                return "Type 1";

            case 2:
                return "Type 2";

            case 3:
                return "Type 3";

            case 4:
                return "Type 4";

            default:
                return "weapon not found";

        }
    }

}
