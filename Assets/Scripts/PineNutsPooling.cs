using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineNutsPooling : MonoBehaviour
{
    public static PineNutsPooling Instance;

    public GameObject prefab;

    [SerializeField] private int _amount = 8;
    [SerializeField] private float _minInstantiateGap = 1;
    [SerializeField] private float _maxInstantiateGap = 3;
    [SerializeField] private float _minPositionX;
    [SerializeField] private float _maxPositionX;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializePool();
    }

    public static void Init(float delayTime)
    {
        Instance.Invoke("GetPineNutFromPool", delayTime);
    }

    private void InitializePool()
    {
        for (int i = 0; i < _amount; i++) {
            AddPineNutToPool();
        }
    }

    private void AddPineNutToPool()
    {
        GameObject pineNut = Instantiate(prefab, this.transform.position, Quaternion.identity, this.transform);
        pineNut.SetActive(false);
    }

    private void GetPineNutFromPool()
    {
        GameObject pineNut = null;

        for (int i = 0; i < transform.childCount; i++) {
            if (!transform.GetChild(i).gameObject.activeSelf) {
                pineNut = transform.GetChild(i).gameObject;
                break;
            }
        }

        if (pineNut == null) {
            AddPineNutToPool();
            pineNut = transform.GetChild(transform.childCount - 1).gameObject;
        }

        pineNut.transform.position = new Vector3(RandomNumber(_minPositionX, _maxPositionX), this.transform.position.y, this.transform.position.z);
        pineNut.GetComponent<PineNutInit>().Init();

        Invoke("GetPineNutFromPool", RandomNumber(_minInstantiateGap, _maxInstantiateGap));
    }

    private float RandomNumber(float min, float max)
    {
        return Random.Range(min, max);
    }
}
