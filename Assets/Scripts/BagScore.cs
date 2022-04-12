using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScore : MonoBehaviour
{
    public GameObject _enterToPanel;
    public GameObject _addAnim;

    [SerializeField] private float _timeAddAnim = 1f;
    [SerializeField] private Vector3 _offsetToPanel;
    private AudioSource _audio;

    private bool _isAllowedToEnter = false;
    private string _tagPlayerName = "Player";

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_isAllowedToEnter && KalawasaController.GetIsInit()) {

            _enterToPanel.transform.position = Camera.main.WorldToScreenPoint(transform.position + _offsetToPanel);
            _enterToPanel.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Return)) {
                KalawasaController.SetInitActive(false);
                KalawasaController.CompletedHappiness();
                GameManager.Completed();      
            }
        } else {
            _enterToPanel.SetActive(false);
        }
    }

    public void WinCookie()
    {
        _audio.Play();
        _addAnim.SetActive(true);
        Invoke("FinishAddAnim", _timeAddAnim);
        KalawasaController.HappinessFace();
    }

    public void FinishAddAnim()
    {
        _addAnim.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag(_tagPlayerName)) {
            _isAllowedToEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_tagPlayerName)) {
            _isAllowedToEnter = false;
        }
    }
}
