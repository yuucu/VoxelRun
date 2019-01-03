using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPlayer : MonoBehaviour
{

    Transform followTarget;

    Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        set();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(followTarget.position.x, transform.position.y, followTarget.position.z);
        transform.position = targetPos;
    }

    void set()
    {
        followTarget = GameObject.Find("Player").transform;
        offset = transform.position - followTarget.position;
    }
}
