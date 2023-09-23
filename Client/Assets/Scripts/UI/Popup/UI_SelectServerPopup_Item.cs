using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectServerPopup_Item : UI_Base
{
    public ServerInfo Info { get; set; }
    public UI_SelectServerPopup ParentUI { get; set; } = null;

    enum Buttons
    {
        SelectServerBtn
    }

    enum Texts
    {
        ServerName,
        SelectServerBusyScore
    }

    enum GameObjects
    {
        CheckMarkParent
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GetButton((int)Buttons.SelectServerBtn).gameObject.BindEvent(OnClickButton);

        TurnOffCheckMark();
    }

    public void RefreshUI()
    {
        if (Info == null)
            return;

        GetTextMeshProUGUI((int)Texts.ServerName).text = Info.Name;
        GetTextMeshProUGUI((int)Texts.SelectServerBusyScore).text = Info.BusyScore.ToString();
    }

    void OnClickButton(PointerEventData evt)
    {
        if (ParentUI == null)
            return;

        ParentUI.SetSelectedServer(this);
    }

    public void TurnOnCheckMark()
    {
        GetObject((int)GameObjects.CheckMarkParent).SetActive(true);
    }

    public void TurnOffCheckMark()
    {
        GetObject((int)GameObjects.CheckMarkParent).SetActive(false);
    }
}
