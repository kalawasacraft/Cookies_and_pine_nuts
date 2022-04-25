using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalawasaController : MonoBehaviour
{
    public static KalawasaController Instance;

    public Transform startPoint;
    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private float _smoothVelocity = 0.05f;
    [SerializeField] private float _stunTime = 0.3f;
    [SerializeField] private float _dashForce = 5f;
    [SerializeField] private float _waitDashTime = 5f;

    [SerializeField] private AudioClip _closeEyes;
    [SerializeField] private AudioClip _rolling;
    public GameObject stunObject;
    public GameObject beakObject;
    public BoxDialog talkDialog;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private AudioSource _audio;

    private Vector2 _movement;
    private Vector3 _velocity = Vector3.zero;
    private float _currentWaitTime = 0f;
    private bool _facingRight = false;
    private bool _isPickUp = false;
    private bool _isDash = false;
    private bool _isInit = false;
    private bool _hasCookie = false;
    
    private string _goOutAnimationTriggerName = "GoOut";
    private string _pickUpAnimationTriggerName = "PickUp";
    private string _dashAnimationTriggerName = "Dash";
    private string _isIdleAnimationBoolName = "IsIdle";
    private string _isRightAnimationBoolName = "IsRight";
    private string _idleRightAnimationName = "IdleRight";
    private string _idleLeftAnimationName = "IdleLeft";
    private string _initAnimationName = "Init";
    private string _tagPickUpName = "PickUp";
    private string _tagDashName = "Dash";
    private string _tagRollingName = "Rolling";
    private string _layerOffsideName = "Offside";
    private string _layerMainName = "Default";

    void Awake()
    {
        Instance = this;

        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(_layerOffsideName);
    }

    void Update()
    {
        if (_isInit) {
            
            float horizontalInput = SimpleInput.GetAxisRaw("Horizontal");
            float verticalInput = SimpleInput.GetAxisRaw("Vertical");
            
            if (!_isPickUp && !_isDash) {
                _movement = new Vector2(horizontalInput, 0f);
                _facingRight = (horizontalInput > 0 ? true : (horizontalInput < 0 ? false : _facingRight));
            }

            if (verticalInput == -1 && !_isPickUp && !_isDash) {
                _movement = Vector2.zero;
                _rigidbody.velocity = Vector2.zero;
                if (!_hasCookie) {
                    _animator.SetTrigger(_pickUpAnimationTriggerName);
                } else {
                    DropCookie();
                }
            }

            if (SimpleInput.GetButtonDown("Dash") && !_isPickUp && !_isDash && _currentWaitTime <= 0f) {
                _rigidbody.velocity = new Vector2(0f, _rigidbody.velocity.y);
                _rigidbody.AddForce(Vector2.right * (_facingRight ? 1f : -1f) * _dashForce, ForceMode2D.Impulse);
                
                _animator.SetTrigger(_dashAnimationTriggerName);
                StartCoroutine(InitWaitDashTime());
            }
        }
    }

    void FixedUpdate()
    {
        if (_isInit) {
            float horizontalVelocity = _movement.normalized.x * _speed;
            _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, new Vector2(horizontalVelocity, _rigidbody.velocity.y), ref _velocity, _smoothVelocity);
        }
    }

    void LateUpdate()
    {
        _animator.SetBool(_isIdleAnimationBoolName, _movement == Vector2.zero);
        _animator.SetBool(_isRightAnimationBoolName, _facingRight);

        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag(_tagRollingName)) {
            if (!_audio.loop) {
                _audio.loop = true;
                _audio.clip = _rolling;
                _audio.Play();
            }
        } else {
            if (_audio.loop) {
                _audio.Stop();
            }
            _audio.loop = false;
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag(_tagPickUpName)) {
            _isPickUp = true;
        } else {
            _isPickUp = false;
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag(_tagDashName)) {
            _isDash = true;
        } else {
            _isDash = false;
        }
    }

    public static void Init(float value)
    {
        Instance.Invoke("EnableTalk", 1f);
        Instance.talkDialog.SetTalk(":O");
        Instance.Invoke("DisableTalk", 4f);
        Instance.Invoke("StartGoOut", value);
    }

    private void EnableTalk() 
    {
        Instance.talkDialog.ActiveDialog(true);
    }

    private void DisableTalk()
    {
        Instance.talkDialog.ActiveDialog(false);
    }

    public void StartGoOut()
    {
        Instance._animator.SetTrigger(Instance._goOutAnimationTriggerName);
    }

    private void Ready()
    {
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        gameObject.layer = LayerMask.NameToLayer(_layerMainName);
        Active();
        ReadyFace();

        GameManager.BeginTimer();
    }

    private void Active()
    {
        _isInit = true;
        stunObject.SetActive(false);
    }

    private void ForceIdle()
    {
        if (_facingRight) {
            _animator.Play(_idleRightAnimationName);
        } else {
            _animator.Play(_idleLeftAnimationName);
        }
    }

    public void StunTime()
    {
        _isInit = false;
        _movement = Vector2.zero;
        ForceIdle();
        StunFace();
        DropCookie();
        stunObject.SetActive(true);
        Invoke("Active", _stunTime);
    }

    public void CloseEyesSound()
    {
        _audio.clip = _closeEyes;
        _audio.Play();
    }

    public void PickUpCookie()
    {
        _hasCookie = true;
    }

    private void DropCookie()
    {
        if (beakObject.transform.childCount > 0) {
            beakObject.transform.GetChild(0).gameObject.GetComponent<CookieInit>().Drop();
            Invoke("ChangeStateHasCookie", 0.3f);
        }
    }

    private void ChangeStateHasCookie()
    {
        _hasCookie = false;
    }

    public static void ReadyFace()
    {
        Instance.Invoke("EnableTalk", 0f);
        Instance.talkDialog.SetTalk("Go!");
        Instance.Invoke("DisableTalk", 1.5f);
    }

    public static void HappinessFace()
    {
        Instance.Invoke("EnableTalk", 0f);
        Instance.talkDialog.SetTalk(":D");
        Instance.Invoke("DisableTalk", 2f);
    }

    public void StunFace()
    {
        Invoke("EnableTalk", 0f);
        talkDialog.SetTalk(":S");
        Invoke("DisableTalk", 1f);
    }

    public static bool HasCookie()
    {
        return Instance._hasCookie;
    }

    public static void SetInitActive(bool value)
    {
        Instance._isInit = value;
    }

    public static bool GetIsInit()
    {
        return Instance._isInit;
    }

    public static void CompletedHappiness()
    {
        Instance.transform.position = Instance.startPoint.position;
        Instance._animator.Play(Instance._initAnimationName);
    }

    private IEnumerator InitWaitDashTime()
    {
        _currentWaitTime = _waitDashTime;        

        while (_currentWaitTime > 0) {
            GameManager.DrawDashSkillTime((_waitDashTime - _currentWaitTime) / _waitDashTime);
            _currentWaitTime -= Time.deltaTime;

            yield return null;
        }

        GameManager.DrawDashSkillTime(1f);
    }
}
