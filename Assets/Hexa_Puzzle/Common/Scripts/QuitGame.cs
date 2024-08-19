using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

    public static QuitGame instance;
    private void Awake()
    {
        instance = this;
    }

    public void ShowConfirmDialog()
    {
        DialogController.instance.ShowDialog(DialogType.QuitGame);
    }
}
