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
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

       /* activeCursor = GetComponent<Image>();
        staticCursor = GetComponent<Image>();*/

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = CursorLockMode.None;
        }
        if (player != null)
        {
            Vector3 staticPos = (player.transform.forward * staticCursorDistance) + player.transform.position;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(staticPos);
            screenPos.z = -3f;

            staticCursor.transform.position = Vector3.Lerp(staticCursor.transform.position, screenPos, Time.deltaTime * 2f);
        }
        if (activeCursor != null && player != null)
        {
            activeCursor.transform.position = Input.mousePosition;
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
