﻿using UnityEngine;
using System.Collections;

public class LevelController {
    public static int GetCurrentLevel()
    {
        return CPlayerPrefs.GetInt(PrefKeys.CURRENT_LEVEL, 1);
    }

    public static void SetCurrentLevel(int level)
    {
        CPlayerPrefs.SetInt(PrefKeys.CURRENT_LEVEL, level);
    }

    public static int GetUnlockLevel(int world)
    {
        return CPlayerPrefs.GetInt("unlocked_level_world_" + world, 1);
    }

    public static void SetUnlockLevel(int world, int level)
    {
        CPlayerPrefs.SetInt("unlocked_level_world_" + world, level);
    }

    public static int GetMovedLevel()
    {
        return CPlayerPrefs.GetInt("moved_level", 1);
    }

    public static void SetMovedLevel(int value)
    {
        CPlayerPrefs.SetInt("moved_level", value);
    }

    public static int GetNumStar(int level, int defaultStar = 0)
    {
        return CPlayerPrefs.GetInt("num_star_level_" + level, defaultStar);
    }

    public static void SetNumStar(int level, int value)
    {
        int currStar = GetNumStar(level);
        CPlayerPrefs.SetInt("num_star_level_" + level, Mathf.Max(currStar, value));
    }
}
