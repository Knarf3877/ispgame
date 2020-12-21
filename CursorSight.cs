using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorSight : MonoBehaviour
{
    public Transform player;
    public float staticCursorDistance = 500f;
    public Image activeCursor, staticCursor;
    // Start is called before the first frame update
    void LateUpdate()
    {

        if (player != null)
        {
            Vector3 staticPos = (player.transform.forward * staticCursorDistance) + player.transform.position;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(staticPos);
            screenPos.z = -12f;

            staticCursor.transform.position = Vector3.Lerp(staticCursor.transform.position, screenPos, Time.deltaTime * 6f);
        }
        if (activeCursor != null && player != null)
        {
            activeCursor.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - 32f, Input.mousePosition.z);
        }

        if(UnifiedPlayerControl.warping == true)
        {
            staticCursor.gameObject.SetActive(false);
        }
        else
        {
            staticCursor.gameObject.SetActive(true);
        }
    }
}
