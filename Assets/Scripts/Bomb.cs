using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bomb")
        {
            Debug.Log(transform.name + " collision with " + collision.transform.name);
            Vector3 pos = transform.position;
            Instantiate(Resources.Load<GameObject>("Prefabs/bombFire"), pos, Quaternion.Euler(-90, 0, 0));
            Destroy(this.gameObject);
        }
    }
    
    void Update()
    {
        if (transform.position.y < -1)
            Destroy(this.gameObject);
    }
    
}
