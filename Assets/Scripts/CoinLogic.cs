using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLogic : MonoBehaviour
{
    [SerializeField] float RotationSpeed = 4f;


       [SerializeField] AudioClip CoinClip;
       AudioSource audio;
       MeshRenderer mesh;
       new Collider collider;

     void Start()
    {
        audio = GetComponent<AudioSource>();
        mesh = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, RotationSpeed);
      
   }

     void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "PLayer")
        {
            if (collider)
            {
                collider.enabled = false;
            }

            if (mesh)
            {
                mesh.enabled = false;
            }

            audio.PlayOneShot(CoinClip);
        }
        
    }

   
}
