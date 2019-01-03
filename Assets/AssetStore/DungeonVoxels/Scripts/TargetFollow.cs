using UnityEngine;
using System.Collections;

public class TargetFollow : MonoBehaviour
{
    [SerializeField]
    Transform followTarget;
    [SerializeField]
    float smoothing = 1f;

    Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - followTarget.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(followTarget.position.x / 2  + offset.x, transform.position.y, followTarget.position.z + offset.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothing);
    }

    public void upgrade()
    {
        offset.z += 0.05f;
    }
}
