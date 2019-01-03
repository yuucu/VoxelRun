using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool magnet = false;
    private bool coinUp = false;

    Transform followTarget;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            magnet = false;
            iTween.Stop(this.gameObject);
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("Get");
            Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }

        if (other.tag == "CoinCatcher")
        {
            magnet = true;
            followTarget = GameObject.Find("Player").transform;
        }

        if (other.tag == "CoinUpper")
        {
            if (!coinUp)
            {
                GetComponent<AudioSource>().Play();
                coinUp = true;
                Vector3 pos = transform.position;
                pos.x -= 0.08f;
                transform.position = pos;
                GameObject coin2 = Instantiate(Resources.Load<GameObject>("Prefabs/coin"), new Vector3(transform.position.x + 0.18f, transform.position.y, transform.position.z - 0.05f), Quaternion.identity, transform.parent);
                coin2.GetComponent<Coin>().coinOn();
            }
        }
    }

    public void coinOn()
    {
        coinUp = true;
    }

    void Update()
    {
        if (magnet)
        {
            Hashtable hash = new Hashtable();
            hash.Add("time", 1f);
            Vector3 pos = followTarget.position;
            pos.z += 0.1f;
            hash.Add("position", pos);
            iTween.MoveUpdate(this.gameObject, hash);
        }
    }
}
