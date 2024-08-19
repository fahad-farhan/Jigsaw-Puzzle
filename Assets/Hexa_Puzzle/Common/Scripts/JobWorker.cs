//#define USE_GPS
using UnityEngine;
using System.Collections.Generic;
using System;

public class JobWorker : MonoBehaviour
{
    public Action<string> onEnterScene;
    public Action onLink2Store;
    public Action onDailyGiftReceived;
    public Action onShowBanner;
    public Action onCloseBanner;
    public Action onShowFixedBanner;
    public Action onShowInterstitial;

    public static JobWorker instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CurrencyController.onBallanceIncreased += OnBallanceIncreased;

#if USE_GPS && (UNITY_ANDROID) && !UNITY_EDITOR
		int autoSignin = CUtils.GetAutoSigninGPS();
		if (autoSignin < CommonConst.MAX_AUTO_SIGNIN && !GooglePlayService.Instance.Authenticated)
		{
			GooglePlayService.Instance.SignIn();
			CUtils.SetAutoSigninGPS(autoSignin + 1);
		}
#endif
    }

    private void OnBallanceIncreased(int value)
    {
        int currentScore = CUtils.GetLeaderboardScore();
        CUtils.SetLeaderboardScore(currentScore + value);
    }
}