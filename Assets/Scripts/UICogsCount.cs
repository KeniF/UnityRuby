using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICogsCount : MonoBehaviour
{
    public static UICogsCount instance { get; private set; }
    TextMeshProUGUI text;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetCogs(int value)
    {
        text.SetText(value.ToString());
    }
}