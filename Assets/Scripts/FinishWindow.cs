using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishWindow : MonoBehaviour
{
    private AudioSource _audio;

    public GameObject finishPanel;
    public TMPro.TMP_Text scoreText;
    public TMPro.TMP_Text timeScoreText;
    public TMPro.TMP_Text totalText;
    public GameObject newRecordText;
    public GameObject imputPanel;
    public TMPro.TMP_InputField inputNickname;
    public Button saveButton;
    public GameObject messagePanel;
    public Animator _animator;
    public AudioClip _completedSound;
    public AudioClip _CaughtSound;

    private string tempRandomPlayer;
    private float tempTotalPlayer;
    private string _happyAnimationName = "Happy";
    private string _sadAnimationName = "Sad";

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        inputNickname.onValueChanged.AddListener (delegate { SetNickname(); });
    }

    public void Finish(bool couldWin, float score, float time)
    {
        KalawasaController.SetInitActive(false);
        
        finishPanel.SetActive(true);
        Time.timeScale = 0f;

        float timeScore = time / 10;
        tempTotalPlayer = score + timeScore;
        scoreText.SetText(score.ToString("0.00"));
        timeScoreText.SetText(timeScore.ToString("0.00") + "   (" + time.ToString("0.00") + "s)");
        
        if (couldWin) {
            ShowMessage("checking...");

            _audio.clip = _completedSound;
            _audio.Play();
            SaveScorePlayer();

            totalText.SetText(tempTotalPlayer.ToString("0.00"));
            _animator.Play(_happyAnimationName);
        } else {
            _audio.clip = _CaughtSound;
            _audio.Play();

            totalText.SetText(score.ToString("--   (they caught you)"));
            _animator.Play(_sadAnimationName);
        }
    }

    private void SaveScorePlayer()
    {
        var newPlayer = new Player(tempTotalPlayer);
        tempRandomPlayer = "anonimo_" + Random.Range(0, 16).ToString();

        DatabaseHandler.GetTopPlayers(1, GameManager.GetGameLevel(), players => {
            
            if (players.Count == 1) {
                var e = players.GetEnumerator();
                e.MoveNext();

                if (e.Current.Value.score < tempTotalPlayer) {
                    ShowNewRecord();
                } else {
                    ShowMessage("you did not improve the highest happiness");
                }
            } else {
                ShowNewRecord();
            }  
        });

        DatabaseHandler.GetPlayer(newPlayer, tempRandomPlayer, GameManager.GetGameLevel(), player => {
            
            if (player.score < newPlayer.score) {

                DatabaseHandler.PostPlayer(newPlayer, tempRandomPlayer, GameManager.GetGameLevel(), () => { });
            }
        });
    }

    private void ShowNewRecord()
    {
        HideMessage();
        imputPanel.SetActive(true);
        newRecordText.SetActive(true);
    }

    private void ShowMessage(string message)
    {
        messagePanel.SetActive(true);
        messagePanel.GetComponentInChildren<TMPro.TMP_Text>().SetText(message);
    }

    private void HideMessage()
    {
        messagePanel.SetActive(false);
    }

    public void Restart()
    {
        PlayConfirmSound();
        Time.timeScale = 1f;
        Invoke("LoadRestart", 0.3f);
    }

    private void LoadRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetNickname()
    {
        if (!Regex.IsMatch(inputNickname.text, "^[a-zA-Z][0-9a-zA-Z][0-9a-zA-Z]+$")) {
            saveButton.interactable = false;
        } else {
            saveButton.interactable = true;
        }
    }

    public void Save()
    {
        PlayConfirmSound();
        saveButton.interactable = false;
        imputPanel.SetActive(false);

        DatabaseHandler.PostPlayer(new Player(tempTotalPlayer, inputNickname.text), tempRandomPlayer, GameManager.GetGameLevel(), () => { });
        
        ShowMessage(inputNickname.text + " was saved");
    }

    private void PlayConfirmSound()
    {
        finishPanel.GetComponent<AudioSource>().Play();
    }
}
