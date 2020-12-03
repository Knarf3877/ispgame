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
        ContactPoint contact = collision.contacts[0];
        Vector3 contactPoint = contact.point;
        Destroy(gameObject);
        Debug.Log("Laser Hit");
        GameObject smallExplosion = Instantiate(explodeFX, contactPoint, this.transform.rotation * Quaternion.Euler(0,-90,0));
        Destroy(smallExplosion, 5f);
    }
}
