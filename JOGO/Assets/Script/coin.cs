using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    private AudioSource sound;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            sound.Play();
            Destroy(this.gameObject, 0.1f);
        }
    }
}
