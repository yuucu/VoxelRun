using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    Animator m_Animator;

    [SerializeField]
    private float moveSpeed = 1f;
    
    private bool isAliving = true;

    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAliving)
            m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, -moveSpeed / 2f);

        if (transform.position.y < -1)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(transform.name + " collision with " + collision.transform.name);
            stop();
        }

        if (collision.gameObject.tag == "Bomb")
        {
            Debug.Log(transform.name + " collision with " + collision.transform.name);
            m_Rigidbody.AddForce(new Vector3(0, 200, 0));
            enemyDeath();
        }

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Water")
        {
            Vector3 newPos = other.transform.position;
            newPos.y += 0.4f;
            transform.position = newPos;
            enemyDeath();
        }
    }

    public void stop()
    {
        m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
        m_Animator.SetTrigger("HitPlayer");
        moveSpeed = 0;
    }

    void enemyDeath()
    {

        m_Animator.SetTrigger("Death");
        isAliving = false;

        Invoke("enemyDelete", 1.3f);
    }
    void enemyDelete()
    {
        /*
        GameObject coin = Resources.Load<GameObject>("Prefabs/coin");
        Vector3 pos = transform.position;
        Instantiate(coin, new Vector3(pos.x, 0.8f, pos.z - 0.9f), Quaternion.identity, stageParent.transform);
        Instantiate(coin, new Vector3(pos.x, 0.8f, pos.z), Quaternion.identity, stageParent.transform);
        Instantiate(coin, new Vector3(pos.x, 0.8f, pos.z + 0.9f), Quaternion.identity, stageParent.transform);
        */
        GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject, 1f);
    }

    public bool getIsAliving()
    {
        return isAliving;
    }

}
