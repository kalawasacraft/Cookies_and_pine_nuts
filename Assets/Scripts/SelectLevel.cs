using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : MonoBehaviour
{
    public GameObject selectPanel;

    public void InitEasy()
    {
        InitGame(0);
    }

    public void InitMedium()
    {
        InitGame(1);
    }

    public void InitHard()
    {
        InitGame(2);
    }

    private void InitGame(int level)
    {
        GameManager.SetGameLevel(level);
        selectPanel.GetComponent<AudioSource>().Play();
        Invoke("LoadInitGame", 0.3f);
    }

    private void LoadInitGame()
    {
        selectPanel.SetActive(false);
        GameManager.InitGame();
    }
}
