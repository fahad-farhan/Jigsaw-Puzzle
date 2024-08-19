﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;

public class BaseController : MonoBehaviour {
    public GameMaster gameMaster;
    public string sceneName;
    public Music.Type music = Music.Type.None;
    protected int numofEnterScene;

    protected virtual void Awake()
    {
        if (GameMaster.instance == null && gameMaster != null)
            Instantiate(gameMaster);
        
        iTween.dimensionMode = CommonConst.ITWEEN_MODE;
        CPlayerPrefs.useRijndael(CommonConst.ENCRYPTION_PREFS);

        numofEnterScene = CUtils.IncreaseNumofEnterScene(sceneName);
    }

    protected virtual void Start()
    {
        CPlayerPrefs.Save();
        if (JobWorker.instance.onEnterScene != null)
        {
            JobWorker.instance.onEnterScene(sceneName);
        }

#if UNITY_WSA && !UNITY_EDITOR
        StartCoroutine(SavePrefs());
#endif
        Music.instance.Play(music);
    }

    public virtual void OnApplicationPause(bool pause)
    {
        if (!pause) CUtils.ShowInterstitialAd();

        Debug.Log("On Application Pause");
        CPlayerPrefs.Save();
    }

    private IEnumerator SavePrefs()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            CPlayerPrefs.Save();
        }
    }
}
