using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishWindow : MonoBehaviour
{
    private AudioSource _audio;

    public GameObject finishPanel;
    public TMPro.TMP_Text scoreText;
    public TMPro.TMP_Text timeScoreText;
    public TMPro.TMP_Text totalText;
    public Animator _animator;
    public AudioClip _completedSound;
    public AudioClip _CaughtSound;

    private string _happyAnimationName = "Happy";
    private string _sadAnimationName = "Sad";

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void Finish(bool couldWin, float score, float time)
    {
        KalawasaController.SetInitActive(false);
        
        finishPanel.SetActive(true);
        Time.timeScale = 0f;

        float timeScore = time / 10;
        float total = score + timeScore;
        scoreText.SetText(score.ToString("0.00"));
        timeScoreText.SetText(timeScore.ToString("0.00") + "   (" + time.ToString("0.00") + "s)");
        
        if (couldWin) {
            _audio.clip = _completedSound;
            _audio.Play();
            SaveScorePlayer(total);

            totalText.SetText(total.ToString("0.00"));
            _animator.Play(_happyAnimationName);
        } else {
            _audio.clip = _CaughtSound;
            _audio.Play();

            totalText.SetText(score.ToString("--   (they caught you)"));
            _animator.Play(_sadAnimationName);
        }
    }

    private void SaveScorePlayer(float total)
    {
        var newPlayer = new Player(total);
        string randomPlayer = "anonimo_" + Random.Range(0, 16).ToString();

        DatabaseHandler.GetPlayer(newPlayer, randomPlayer, player => {
            
            if (player.score < newPlayer.score) {
                DatabaseHandler.PostPlayer(newPlayer, randomPlayer, () => { });    
            }                
        });
    }
}
