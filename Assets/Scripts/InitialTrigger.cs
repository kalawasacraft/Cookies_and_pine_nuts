using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialTrigger : MonoBehaviour
{
    public static InitialTrigger Instance;
    
    public List<GameObject> initPineNuts;

    void Awake()
    {
        Instance = this;
    }

    public static void Init()
    {
        for (int i = 0; i < Instance.initPineNuts.Count; i++) {
            Instance.initPineNuts[i].SetActive(true);
        }
    }
    
}
