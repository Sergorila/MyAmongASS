using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public static UIControl Instance;
    public Button killBtn;
    public bool hasTarget;
    public Killable curPlayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        killBtn.interactable = hasTarget;
    }

    public void OnKillButtonPressed()
    {
        if (curPlayer == null)
        {
            return;
        }
        curPlayer.Kill();
    }
}
