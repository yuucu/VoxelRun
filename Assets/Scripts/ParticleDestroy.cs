using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{

    ParticleSystem particle;

    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        if (!particle.IsAlive())
            Destroy(this.gameObject);
    }
}
