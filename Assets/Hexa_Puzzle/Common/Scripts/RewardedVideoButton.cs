using UnityEngine;

public class RewardedVideoButton : MonoBehaviour
{
    public void OnClick()
    {
#if UNITY_EDITOR
        OnUserEarnedReward();
#else
        if (IsAdAvailable())
        {
            AdmobController.onUserEarnedReward = OnUserEarnedReward;
            AdmobController.instance.ShowRewardedAd();
        }
        else
        {
            Toast.instance.ShowMessage("Ad is not available now, please wait..");
        }
#endif

        Sound.instance.PlayButton();
    }

    public void OnUserEarnedReward()
    {
        int amount = ConfigController.Config.rewardedVideoAmount;
        GameState.hint.ChangeValue(amount);

        string unit = amount == 1 ? " hint" : " hints";
        Toast.instance.ShowMessage("You've received " + amount + unit, 3);
    }

    private bool IsAdAvailable()
    {
        return AdmobController.instance.rewardedAd.IsLoaded();
    }
}
