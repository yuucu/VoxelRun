using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(transform.name + " triggered " + other.transform.name);
        if (other.tag == "Player")
        {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Get");
            Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
