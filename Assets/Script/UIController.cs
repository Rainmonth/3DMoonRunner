﻿using UnityEngine;
using System.Collections;

/// <summary>
/// UI 界面控制器，主要管理游戏开始、游戏恢复、游戏重新开始、游戏暂停是 的UI显示状态。
/// </summary>
public class UIController : MonoBehaviour {

    public GameObject PlayUI;
    public GameObject ResumeUI;
    public GameObject RestartUI;
    public GameObject PauseUI;

    public static UIController instance;

    public Canvas canvas;


    public void PlayHandler()
    {
        HidePlayUI();
        ShowPauseUI();
        AudioManager.instance.PlayButtonAudio();
        GameController.instance.Play();
    }

    public void PauseHandler()
    {
        ShowResumeUI();
        HidePauseUI();
        AudioManager.instance.PlayButtonAudio();
        GameController.instance.Pause();
    }


    public void ResumeHandler()
    {
        HideResumeUI();
        ShowPauseUI();
        AudioManager.instance.PlayButtonAudio();
        GameController.instance.Resume();
    }

    public void RestartHandler()
    {
        HideRestartUI();
        ShowPauseUI();
        AudioManager.instance.PlayButtonAudio();
        GameController.instance.Restart();
    }

    public void ExitHandler()
    {
        AudioManager.instance.PlayButtonAudio();
        GameController.instance.Exit();
    }

    public void ShowRestartUI()
    {
        iTween.MoveTo(RestartUI, canvas.transform.position + Vector3.zero, 1.0f);
    }

    public void HideRestartUI()
    {
        iTween.MoveTo(RestartUI, canvas.transform.position + new Vector3(-Screen.width / 2 - 500, 0, 0), 1.0f);
    }


    public void HidePlayUI()
    {
        iTween.MoveTo(PlayUI, canvas.transform.position + new Vector3(-Screen.width / 2 - 500, 0, 0), 1.0f);
    }

    public void ShowPauseUI()
    {
        iTween.MoveTo(PauseUI, canvas.transform.position + new Vector3(-Screen.width / 2, -Screen.height / 2, 0), 1.0f);
    }

    public void ShowResumeUI()
    {
        iTween.MoveTo(ResumeUI, canvas.transform.position + Vector3.zero, 1.0f);
    }

    public void HidePauseUI()
    {
        // iTween.MoveTo(PauseUI, canvas.transform.position + new Vector3(-Screen.width / 2 - 500, -Screen.height / 2, 0), 1.0f);
        iTween.FadeTo(PauseUI, 0, 0.5f);
    }

    public void HideResumeUI()
    {
        iTween.MoveTo(ResumeUI, canvas.transform.position + new Vector3(-Screen.width / 2 - 500, 0, 0), 1.0f);
    }

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
