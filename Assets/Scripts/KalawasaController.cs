using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalawasaController : MonoBehaviour
{
    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private float _smoothVelocity = 0.05f;
    [SerializeField] private float _stunTime = 0.3f;
    public GameObject stunObject;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private AudioSource _audio;

    private Vector2 _movement;
    private Vector3 _velocity = Vector3.zero;
    private bool _facingRight = false;
    private bool _isPickUp = false;
    private bool _isInit = false;
    

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        //_audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Offside");
        Invoke("Init", 4f);
    }

    void Update()
    {
        if (_isInit) {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            
            _movement = new Vector2(horizontalInput, 0f);
            _facingRight = (horizontalInput > 0 ? true : (horizontalInput < 0 ? false : _facingRight));

            if (verticalInput == -1 && !_isPickUp) {
                _movement = Vector2.zero;
                _rigidbody.velocity = Vector2.zero;
                _animator.SetTrigger("PickUp");
            }
        }
    }

    void FixedUpdate()
    {
        float horizontalVelocity = _movement.normalized.x * _speed;
        _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, new Vector2(horizontalVelocity, _rigidbody.velocity.y), ref _velocity, _smoothVelocity);
    }

    void LateUpdate()
    {
        _animator.SetBool("IsIdle", _movement == Vector2.zero);
        _animator.SetBool("IsRight", _facingRight);

        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("PickUp")) {
            _isPickUp = true;
        } else {
            _isPickUp = false;
        }
    }

    private void Init()
    {
        _animator.SetTrigger("GoOut");
    }

    private void Ready()
    {
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        gameObject.layer = LayerMask.NameToLayer("Default");
        Active();
    }

    private void Active()
    {
        _isInit = true;
        stunObject.SetActive(false);
    }

    public void StunTime()
    {
        _isInit = false;
        stunObject.SetActive(true);
        Invoke("Active", _stunTime);
    }
}
