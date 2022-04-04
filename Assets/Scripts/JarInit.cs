using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarInit : MonoBehaviour
{
    private Collider2D _collider;
    private Animator _animator;
    private AudioSource _audio;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _collider = GetComponent<Collider2D>();
    }

    private void Init()
    {
        _animator.Play("Init");
        _audio.Play();
    }

    private void Stop()
    {
        _animator.Play("Stop");
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        _collider.enabled = false;
        Init();
    }
}
