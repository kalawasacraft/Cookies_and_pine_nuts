using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMPro.TMP_Text highScore;
    public TMPro.TMP_Text score;
    public TMPro.TMP_Text toAdd;
    public TMPro.TMP_Text dialog;
    public GameObject _panelBoy;
    public AudioSource _audioBoy;
    public Image timer;

    [SerializeField] private Color _startColor;
    [SerializeField] private Color _endColor;
    [SerializeField] private float _minScaleBoy;
    [SerializeField] private float _maxScaleBoy;

    private float _maxWidth;
    private List<string> _shouting = new List<string> {
        "Mom, the cookies fell out!",
        "I'm hungry!, buy me another please",
        "Nooo, buy me first, then I pick them up",
        "Mom!, something that looks like a rat is picking them up",
        "Bring a broom, it can bite us!",
    };
    private Color _currentColor;

    void Awake()
    {
        UIManager.Instance = this;
    }
    
    void Start()
    {
        _maxWidth = timer.rectTransform.sizeDelta.x;
    }

    public static void SetHighScore(float value)
    {
        Instance.highScore.SetText(value.ToString("0.00"));
    }

    public static void SetScore(float value)
    {
        Instance.score.SetText(value.ToString("0.00"));
    }

    public static void SetToAdd(int value)
    {
        Instance.toAdd.SetText(value.ToString());
    }

    public static void SetShouting(int value)
    {
        Instance._audioBoy.Play();
        Instance.dialog.SetText(Instance._shouting[Mathf.Min(value, Instance._shouting.Count-1)]);
        Instance.SetColorDialog();
    }

    private void SetColorDialog()
    {
        _currentColor = Instance.dialog.color;
        Instance.dialog.color = new Color(165f/255, 48f/255, 48f/255, 1f);
        Invoke("RestoreColorDialog", 2f);
    }

    private void RestoreColorDialog()
    {
        Instance.dialog.color = _currentColor;
    }

    public static void DrawBar(float value, float maxValue)
    {
        float currentWidth = (value * Instance._maxWidth) / maxValue;
        Instance.timer.rectTransform.sizeDelta = new Vector2(currentWidth, Instance.timer.rectTransform.sizeDelta.y);
        Instance.timer.color = Color.Lerp(Instance._endColor, Instance._startColor, value / maxValue);

        float currentScale = (((maxValue - value) * Instance._maxScaleBoy) + value * Instance._minScaleBoy) / maxValue;
        Instance._panelBoy.transform.localScale = new Vector3(currentScale, currentScale, 0f);
    }

    public static void Finish(bool value, float score, float time)
    {
        Instance.GetComponent<FinishWindow>().Finish(value, score, time);
    }
}
