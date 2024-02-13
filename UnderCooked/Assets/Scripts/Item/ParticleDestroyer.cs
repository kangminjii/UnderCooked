using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        Invoke("DestroyParticleSystem", particleSystem.main.duration);
    }

    void DestroyParticleSystem()
    {
        Destroy(gameObject);
    }
}
