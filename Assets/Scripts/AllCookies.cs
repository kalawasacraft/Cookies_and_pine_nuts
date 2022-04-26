using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCookies : MonoBehaviour
{
    public GameObject prefab;

    [SerializeField] private int _amount = 10;
    [SerializeField] private float _minSpeedValue = 1f;
    [SerializeField] private float _maxSpeedValue = 2f;
    [SerializeField] private Vector2 _rangeDirectionX;
    [SerializeField] private Vector2 _rangeDirectionY;

    void Start()
    {
        InitializeCookies();
    }

    public void Init()
    {
        ThrowCookies();
    }

    private void InitializeCookies()
    {
        for (int i = 0; i < _amount; i++) {
            AddCookie();
        }
    }

    private void AddCookie()
    {
        GameObject cookie = Instantiate(prefab, this.transform.position, Quaternion.identity, this.transform);
        cookie.SetActive(false);
    }

    private void ThrowCookies()
    {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject cookie = transform.GetChild(i).gameObject;
            cookie.SetActive(true);
            Vector2 dir = new Vector2(RandomNumber(_rangeDirectionX.x, _rangeDirectionX.y),
                                        RandomNumber(_rangeDirectionY.x, _rangeDirectionY.y));
            cookie.GetComponent<CookieInit>().Throw(dir, RandomNumber(_minSpeedValue, _maxSpeedValue));
            //cookie.transform.position = new Vector3(RandomNumber(_minPositionX, _maxPositionX), this.transform.position.y, this.transform.position.z);
        }
    }

    private float RandomNumber(float min, float max)
    {
        return Random.Range(min, max);
    }
}
