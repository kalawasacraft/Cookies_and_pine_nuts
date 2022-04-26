using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieInit : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private AudioSource _audio;

    private Transform _mainParent;
    public BoxDialog talkDialog;

    [SerializeField] private Transform _decreaseChild;
    [SerializeField] private AudioClip _impactGround;    
    [SerializeField] private AudioClip _impactPickUp;
    
    [SerializeField] private int _valueHappiness = 32;
    [SerializeField] private float _fadeTime = 1f;
    private float _initalDistanceChild;
    private bool _isInit = true;
    private bool _onTheGround = false;
    private bool _isPickUp = false;

    private string _tagGroundName = "Ground";
    private string _tagPlayerName = "Player";
    private string _tagBagName = "Bag";
    private string _layerOffsideName = "Offside";

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        _mainParent = transform.parent;
        _valueHappiness = (int) Mathf.Pow(2, (int) Random.Range(1, 7));
        _initalDistanceChild = Mathf.Sqrt(Mathf.Pow(_decreaseChild.transform.localPosition.x, 2) + Mathf.Pow(_decreaseChild.transform.localPosition.y, 2));
    }

    void Update()
    {
        _decreaseChild.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        _decreaseChild.transform.localPosition = new Vector3(Mathf.Sin((gameObject.transform.rotation.eulerAngles.z * Mathf.PI)/180) * _initalDistanceChild, 
                                                            Mathf.Cos((gameObject.transform.rotation.eulerAngles.z * Mathf.PI)/180) * _initalDistanceChild, 0f);

        if (_isPickUp) {
            transform.position = transform.parent.position;
        }
        
        if (_onTheGround && Mathf.Abs(_rigidbody.velocity.y) <= 0.05f && Mathf.Abs(_rigidbody.velocity.x) <= 0.05f) {
            talkDialog.ActiveDialog(true);
        } else {
            talkDialog.ActiveDialog(false);
        }
    }

    public void DecreaseHappiness()
    {
        _valueHappiness /= 2;
        talkDialog.SetTalk(_valueHappiness.ToString());
        
        EnabledDecresedChild();
        Invoke("DisabledDecresedChild", 0.3f);

        if (_valueHappiness == 0) {
            gameObject.layer = LayerMask.NameToLayer(_layerOffsideName);
            StartCoroutine(FadeOff());
        }
    }

    private void EnabledDecresedChild()
    {
        _decreaseChild.gameObject.SetActive(true);
        _sprite.color = new Color(207f/255f, 87f/255f, 60f/255f);
    }

    private void DisabledDecresedChild()
    {
        _decreaseChild.gameObject.SetActive(false);
        _sprite.color = new Color(1f, 1f, 1f);
    }
    
    private Color ChangeAlpha(float value)
    {
        Color newColor = _sprite.color;
        newColor.a = value;
        return newColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == _tagGroundName && !_onTheGround) {
            _onTheGround = true;
            PlaySound(_impactGround);
            DecreaseHappiness();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag(_tagPlayerName) && _onTheGround && !KalawasaController.HasCookie()) {
            
            _onTheGround = false;
            _isInit = false;
            PickUp();
            PlaySound(_impactPickUp);
            transform.SetParent(collision.transform);

            collision.SendMessageUpwards("PickUpCookie");

        } else if (collision.CompareTag(_tagBagName) && !_onTheGround && !_isInit) {
            
            collision.SendMessageUpwards("WinCookie");
            GameManager.AddToScore(_valueHappiness);
            Destroy(gameObject);
        }
    }

    private void PickUp()
    {
        _isPickUp = true;
        _collider.enabled = false;
        _rigidbody.bodyType = RigidbodyType2D.Static;
        //_sprite.sortingOrder = 1;
        GameManager.PendingToAdd(_valueHappiness);
    }

    public void Drop()
    {
        _isPickUp = false;
        _collider.enabled = true;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        //_sprite.sortingOrder = 0;
        transform.SetParent(_mainParent);
        GameManager.PendingToAdd(0);
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

        Destroy(gameObject);
    }

    public void Throw(Vector2 direction, float speed)
    {
        _rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}
