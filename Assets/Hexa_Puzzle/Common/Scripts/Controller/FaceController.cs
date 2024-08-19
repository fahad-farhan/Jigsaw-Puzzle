using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FaceController
{
	public const string INVITABLE_OFFSET = "invitable_offset";

    public static int GetInvitableOffset()
    {
        return CPlayerPrefs.GetInt(INVITABLE_OFFSET, 0);
    }

    public static void SetInvitableOffet(int value)
    {
        CPlayerPrefs.SetInt(INVITABLE_OFFSET, value);
    }

    public static void IncreaseInvitableOffset()
    {
        int current = GetInvitableOffset();
        SetInvitableOffet(current + 1);
    }

    public static void SetPermissions(string permissions)
    {
        CPlayerPrefs.SetString("facebook_permissions", permissions);
    }

    public static string GetPermissions()
    {
        return CPlayerPrefs.GetString("facebook_permissions", "");
    }

    public static bool HasPublishActions()
    {
        string permissions = GetPermissions();
        return permissions.Contains("publish_actions");
    }

    public static void SetFriendAvatarUrls(string avatarUrls)
    {
        CPlayerPrefs.SetString("facebook_friends_avatar_urls", avatarUrls);
    }

    public static string GetFriendAvatarUrls()
    {
        return CPlayerPrefs.GetString("facebook_friends_avatar_urls", "");
    }

    public static void SetMyAvatarUrl(string url)
    {
        CPlayerPrefs.SetString("facebook_my_avatar_url", url);
    }

    public static string GetMyAvatarUrl()
    {
        return CPlayerPrefs.GetString("facebook_my_avatar_url", "");
    }

    public static string GetFriendAvatarUrl(int index)
    {
        string strAvatarUrls = FaceController.GetFriendAvatarUrls();
        List<string> avatarUrls = CUtils.BuildListFromString<string>(strAvatarUrls);
        if (index < avatarUrls.Count)
        {
            return avatarUrls[index];
        }
        return "";
    }

    public static void SetFriendLevels(string friendLevels)
    {
        CPlayerPrefs.SetString("facebook_friends_levels", friendLevels);
    }

    public static string GetFriendLevels()
    {
        return CPlayerPrefs.GetString("facebook_friends_levels", "");
    }

    public static void SetFriendScores(int index, string scores)
    {
        CPlayerPrefs.SetString("facebook_friends_scores_" + index, scores);
    }

    public static string GetFriendScores(int index)
    {
        return CPlayerPrefs.GetString("facebook_friends_scores_" + index, "");
    }

    public static void SetMyScores(string scores)
    {
        CPlayerPrefs.SetString("facebook_my_scores", scores);
    }

    public static string GetMyScores()
    {
        return CPlayerPrefs.GetString("facebook_my_scores", "");
    }

    public static int GetMyScore(int level)
    {
        string strScores = GetMyScores();
        List<int> scores = CUtils.BuildListFromString<int>(strScores);
        if (level <= scores.Count)
        {
            return scores[level - 1];
        }
        return 0;
    }

    public static void SetMyScore(int level, int score)
    {
        string strScores = FaceController.GetMyScores();
        List<int> scores = CUtils.BuildListFromString<int>(strScores);
        if (level <= scores.Count)
        {
            scores[level - 1] = score;
        }
        else if (level == scores.Count + 1)
        {
            scores.Add(score);
        }
        string newStrScores = CUtils.BuildStringFromCollection(scores);
        FaceController.SetMyScores(newStrScores);
    }

    public static int GetFriendScore(int index, int level)
    {
        string strScores = GetFriendScores(index);
        List<int> scores = CUtils.BuildListFromString<int>(strScores);
        if (level <= scores.Count)
        {
            return scores[level - 1];
        }
        return 0;
    }

    public static List<int> GetFriendIndexesPassed(int level)
    {
        List<int> indexes = new List<int>();
        string strFriendLevels = GetFriendLevels();
        List<int> friendLevels = CUtils.BuildListFromString<int>(strFriendLevels);
        for (int i = 0; i < friendLevels.Count; i++)
        {
            if (friendLevels[i] > level)
            {
                indexes.Add(i);
            }
        }
        return indexes;
    }

    public static void ChangeFriendLevels()
    {
        string friendLevels = GetFriendLevels();
        if (friendLevels == "") return;

        int dayChange = CPlayerPrefs.GetInt("facebook_daychange_level", -1);
        if (DateTime.Now.DayOfYear == dayChange) return;

        CPlayerPrefs.SetInt("facebook_daychange_level", DateTime.Now.DayOfYear);

        List<int> friendLevelList = CUtils.BuildListFromString<int>(friendLevels);
        for (int i = 0; i < friendLevelList.Count; i++)
        {
            int delta = UnityEngine.Random.Range(0, 3);
            friendLevelList[i] += delta;
            if (friendLevelList[i] >= CommonConst.TOTAL_LEVELS)
            {
                friendLevelList[i] = CommonConst.TOTAL_LEVELS;
            }
        }
        friendLevels = CUtils.BuildStringFromCollection(friendLevelList);
        SetFriendLevels(friendLevels);
    }
}