using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBoxControl : MonoBehaviour
{
    // : private
    public Animator _animator;
    public Transform _spawnPoint;


    float spawnDelay = 1.5f; // 스폰 지연 시간


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

}