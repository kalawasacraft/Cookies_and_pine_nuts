using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxDialog : MonoBehaviour
{
    public Image boxDialog;
    public TMPro.TMP_Text talk;
    public Vector3 offset;

    void Update()
    {
        boxDialog.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    public void SetTalk(string message)
    {
        talk.SetText(message);
    }

    public void ActiveDialog(bool value)
    {
        boxDialog.gameObject.SetActive(value);
    }
}
