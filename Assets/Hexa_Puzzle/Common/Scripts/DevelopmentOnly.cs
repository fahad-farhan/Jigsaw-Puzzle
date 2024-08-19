using UnityEngine;
using System.Collections;
using System.IO;

public class DevelopmentOnly : MonoBehaviour {
    public bool setRuby;
    public int ruby;
    public bool setUnlockLevel;
    public int unlockLevel;
    public int unlockWorld;

    public bool clearAllPrefs;

    private void Start()
    {
        if (setRuby)
            CurrencyController.SetBalance(ruby);
        if (setUnlockLevel)
            LevelController.SetUnlockLevel(unlockWorld, unlockLevel);

        if (clearAllPrefs)
        {
            CPlayerPrefs.DeleteAll();
            CPlayerPrefs.Save();
        }
    }
}
