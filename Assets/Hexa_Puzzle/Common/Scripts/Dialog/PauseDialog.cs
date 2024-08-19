using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseDialog : Dialog {

    protected override void Start()
    {
        base.Start();
    }

    public void OnReplayClick()
    {
        Close();
        Sound.instance.PlayButton();
        MainController.instance.Replay();
    }
}
