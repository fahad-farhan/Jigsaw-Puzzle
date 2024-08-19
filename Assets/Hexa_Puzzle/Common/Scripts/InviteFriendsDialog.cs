using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;

public class InviteFriendsDialog : Dialog
{
    public RowInviteFriends Row;
    public RectTransform contentTransform;
    public Action<string> onInviteProcessed;
    public Scrollbar scrollBar;
    private List<InvitableFriend> data;

    public void Build(List<InvitableFriend> data)
    {
        this.data = data;

        for (int i = 0; i < data.Count; i++)
        {
            string name = data[i].name;
            string avatarUrl = data[i].avatarUrl;
            RowInviteFriends row = (RowInviteFriends)Instantiate(Row);
            row.Build(name, avatarUrl);
            row.transform.SetParent(contentTransform);
            row.rectTransform.localScale = Vector3.one;
            row.rectTransform.anchoredPosition = new Vector3(0, (25 - i) * 110);
        }
        contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, 110 * data.Count);
    }

    protected override void Start()
    {
        base.Start();
        scrollBar.value = 1f;
    }

    public void Invite()
    {
        Sound.instance.PlayButton();
        if (CheckRequirement())
        {
            string recipients = GetRecipients();
            onInviteProcessed(recipients);
            Close();
        }
    }

    private bool CheckRequirement()
    {
        int numofChecked = 0;
        for (int i = 0; i < data.Count; i++)
        {
            RowInviteFriends row = contentTransform.GetChild(i).GetComponent<RowInviteFriends>();
            data[i].shouldInvite = row.toggleCheck.isOn;
            if (row.toggleCheck.isOn)
                numofChecked++;
        }

        if (numofChecked >= CommonConst.MIN_INVITE_FRIEND)
        {
            return true;
        }
        return false;
    }

    private string GetRecipients()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].shouldInvite == false) continue;
            string id = data[i].id;

            if (i != data.Count - 1)
                builder.Append(id).Append(",");
            else
                builder.Append(id);
        }

        return builder.ToString();
    }
}
