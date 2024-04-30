using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        Invoke("DestroyParticle", _particleSystem.main.duration);
    }

    void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
