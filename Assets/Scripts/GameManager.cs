using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float _timeInitPlayer = 2f;
    [SerializeField] private float _timeInitPineNutsPool = 3f;
    [SerializeField] private float _timeStartShouting = 2f;
    [SerializeField] private float _gameTime = 60f;
    private float _myScore = 0f;
    private int _potentialToAdd = 0;
    private float _timePlaying;
    private bool _timerGoing = false;
    private int _indexShouting = 0;

    void Awake()
    {
        GameManager.Instance = this;
    }

    void Start()
    {
        GetTopPlayer();
        UIManager.SetScore(_myScore);
        UIManager.SetToAdd(_potentialToAdd);
    }

    private void GetTopPlayer()
    {
        DatabaseHandler.GetTopPlayers(1, players => {
            
            if (players.Count == 1) {
                var e = players.GetEnumerator();
                e.MoveNext();
                UIManager.SetHighScore(e.Current.Value.score);
            } else {
                UIManager.SetHighScore(0f);
            }        
        });
    }

    public static void InitKalawasa()
    {
        Instance.Invoke("FirstShouting", Instance._timeStartShouting);
        KalawasaController.Init(Instance._timeInitPlayer);
        PineNutsPooling.Init(Instance._timeInitPineNutsPool);
    }

    private void FirstShouting()
    {
        UIManager.SetShouting(_indexShouting++);
    }

    public static void PendingToAdd(int value)
    {
        Instance._potentialToAdd = value;
        UIManager.SetToAdd(Instance._potentialToAdd);
    }

    public static void AddToScore(float value)
    {
        Instance._myScore += value;
        UIManager.SetScore(Instance._myScore);
    }

    public static void BeginTimer()
    {
        Instance._timerGoing = true;
        Instance.StartCoroutine(Instance.UpdateTimer());
    }

    public static void Completed()
    {
        UIManager.Finish(true, Instance._myScore + Instance._potentialToAdd, Instance._timePlaying);
    }

    private IEnumerator UpdateTimer()
    {
        _timePlaying = _gameTime;
        int initValue = (int) _gameTime;
        
        while (_timerGoing && _timePlaying > 0) {
            _timePlaying -= Time.deltaTime;

            UIManager.DrawBar(_timePlaying, _gameTime);

            int tempValue = (int) _timePlaying;
            if (initValue != tempValue) {
                if (tempValue == 55 || tempValue == 40 || tempValue == 25 || tempValue == 10) {
                    UIManager.SetShouting(_indexShouting++);
                }
                initValue = tempValue;
            }

            yield return null;
        }

        UIManager.Finish(false, _myScore, 0f);
    }
}
