using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour
{
    public Transform spike;
    public string playerTag = "Player";
    public float activeDuration = 20f;
    public float inactiveDuration = 0.1f;
    public bool startActive = true;
    public bool randomize = true;
    private bool isActive;


    private AudioSource ja;
    private bool audio = false;

    void Start()
    {
        ja = GetComponent<AudioSource>();
        if (startActive)
        {
            SetActive();
        }
        else
        {
            SetInactive();
        }
    }

    void SetActive()
    {
        if (audio == true)
        {
            ja.Play();
            audio = false;
        }
        float duration = activeDuration;
        isActive = true;
        spike.gameObject.SetActive(true);
        if (randomize)
        {
            duration = Random.Range(activeDuration * .25f, activeDuration * 1.25f);
        }
        Invoke("SetInactive", duration);
    }

    void SetInactive()
    {
        float duration = activeDuration;
        isActive = false;
        spike.gameObject.SetActive(false);
        if (randomize)
        {
            duration = Random.Range(inactiveDuration * .25f, inactiveDuration * 1.25f);
        }
        Invoke("SetActive", duration);
        audio = true;
    }

    public bool GetStatus()
    {
        return isActive;
    }
}
