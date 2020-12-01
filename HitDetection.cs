using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public GameObject explodeFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Debug.Log("Laser Hit");
        Instantiate(explodeFX, this.transform.position, this.transform.rotation * Quaternion.Euler(0,-90,0));
    }
}
