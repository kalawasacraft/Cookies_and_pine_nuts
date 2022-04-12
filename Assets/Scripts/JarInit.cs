using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarInit : MonoBehaviour
{
    [SerializeField] private bool _initGame = false;
    public GameObject _initAllCookies;

    private Collider2D _collider;
    private Animator _animator;
    private AudioSource _audio;

    private string _initAnimationName = "Init";
    private string _stopAnimationName = "Stop";

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _collider = GetComponent<Collider2D>();
    }

    private void Init()
    {
        _animator.Play(_initAnimationName);
        _audio.Play();

        if (_initGame) {
            GameManager.InitKalawasa();
        }
    }

    private void Stop()
    {
        _animator.Play(_stopAnimationName);
    }

    private void ThrowCookies()
    {
        _initAllCookies.GetComponent<AllCookies>().Init();
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        _collider.enabled = false;
        Init();
    }
}
