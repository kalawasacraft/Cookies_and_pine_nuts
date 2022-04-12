using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineNutInit : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private AudioSource _audio;

    [SerializeField] private float _fadeTime = 2f;
    private bool _isFirstCollisionGround = false;
    private bool _isFirstCollisionPlayer = false;

    [SerializeField] private AudioClip _impactPlayer;
    [SerializeField] private AudioClip _impactGround;

    private string _tagPlayerName = "Player";
    private string _tagGroundName = "Ground";
    private string _layerOffsideName = "Offside";
    private string _layerMainName = "Default";
    private string _functionStunTimeName = "StunTime";

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _audio = GetComponent<AudioSource>();
    }

    public void Init()
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        _sprite.color = ChangeAlpha(1f);
        _isFirstCollisionGround = false;
        _isFirstCollisionPlayer = false;
        gameObject.layer = LayerMask.NameToLayer(_layerMainName);
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == _tagPlayerName && !_isFirstCollisionPlayer) {

            PlaySound(_impactPlayer);
            _isFirstCollisionPlayer = true;
            collision.gameObject.SendMessageUpwards(_functionStunTimeName);

        } else if (collision.gameObject.tag == _tagGroundName && !_isFirstCollisionGround) {

            PlaySound(_impactGround);
            _isFirstCollisionPlayer = true;
            _isFirstCollisionGround = true;
            gameObject.layer = LayerMask.NameToLayer(_layerOffsideName);
            StartCoroutine(FadeOff());
        }
    }

    private Color ChangeAlpha(float value)
    {
        Color newColor = _sprite.color;
        newColor.a = value;
        return newColor;
    }

    private void PlaySound(AudioClip clip)
    {
        _audio.clip = clip;
        _audio.Play();
    }

    private IEnumerator FadeOff()
    {
        float _currentFadeTime = _fadeTime;
        
        while (_currentFadeTime > 0) {
            _currentFadeTime -= Time.deltaTime;

            _sprite.color = ChangeAlpha(_currentFadeTime / _fadeTime);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
