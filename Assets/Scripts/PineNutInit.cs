using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineNutInit : MonoBehaviour
{
    //private Collider2D _collider;
    //private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;

    [SerializeField] private float _fadeTime = 2f;
    private bool _isFirstCollisionGround = false;
    private bool _isFirstCollisionPlayer = false;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        //_collider = GetComponent<Collider2D>();
        //_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == "Player" && !_isFirstCollisionPlayer) {
            Debug.Log("Collision Player");

            _isFirstCollisionPlayer = true;
            collision.gameObject.SendMessageUpwards("StunTime");

        } else if (collision.gameObject.tag == "Ground" && !_isFirstCollisionGround) {
            Debug.Log("Collision Ground!");

            _isFirstCollisionGround = true;
            gameObject.layer = LayerMask.NameToLayer("Offside");
            StartCoroutine(FadeOff());

            Destroy(this.gameObject, _fadeTime);
        }
    }

    private IEnumerator FadeOff()
    {
        float _currentFadeTime = _fadeTime;
        
        while (_currentFadeTime > 0) {
            _currentFadeTime -= Time.deltaTime;
            
            Color _newColor = _sprite.color;
            _newColor.a = _currentFadeTime / _fadeTime;
            _sprite.color = _newColor;

            yield return null;
        }
    }
}
