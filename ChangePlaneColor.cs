using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlaneColor : MonoBehaviour
{
    public GameObject plane;
    private MeshRenderer myRenderer;

    private void Start()
    {
        Debug.Log("This button is working");
    }

    public void OnChangeColor ()
    {
        myRenderer = plane.GetComponent<MeshRenderer>();
        myRenderer.enabled = !myRenderer.enabled;
    }
}
