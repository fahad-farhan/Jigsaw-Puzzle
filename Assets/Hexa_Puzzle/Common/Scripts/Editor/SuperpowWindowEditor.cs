using UnityEngine;
using UnityEditor;

public class SuperpowWindowEditor
{
    [MenuItem("Superpow/Clear all playerprefs")]
    static void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    [MenuItem("Superpow/Unlock all levels")]
    static void UnlockAllLevel()
    {
        CPlayerPrefs.useRijndael(CommonConst.ENCRYPTION_PREFS);

        for (int i = 1; i <= Const.WORLD_NAME.Length; i++)
        {
            LevelController.SetUnlockLevel(i, Const.MAX_LEVEL + 1);
        }
    }

    [MenuItem("Superpow/Credit balance (ruby, hint..)")]
    static void AddRuby()
    {
        CPlayerPrefs.useRijndael(CommonConst.ENCRYPTION_PREFS);
        GameState.hint.ChangeValue(1000);
    }

    [MenuItem("Superpow/Set balance to 0")]
    static void SetBalanceZero()
    {
        CPlayerPrefs.useRijndael(CommonConst.ENCRYPTION_PREFS);
        CurrencyController.SetBalance(0);
    }
}