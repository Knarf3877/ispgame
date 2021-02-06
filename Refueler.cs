using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refueler : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AutoRefuel());
    }


    public IEnumerator AutoRefuel()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (UnifiedPlayerControl.warpFuel < 10 && UnifiedPlayerControl.warping == false)
            {
                yield return new WaitForSeconds(.8f);
                UnifiedPlayerControl.warpFuel++;
            }
            else if (UnifiedPlayerControl.warpFuel < 30 && UnifiedPlayerControl.warping == false)
            {
                yield return new WaitForSeconds(.45f);
                UnifiedPlayerControl.warpFuel++;
            }
            else
            {
                yield return null;
            }
        }
    }

}
