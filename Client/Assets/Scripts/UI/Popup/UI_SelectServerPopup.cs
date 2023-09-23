using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectServerPopup : UI_Popup
{
    public List<UI_SelectServerPopup_Item> Items { get; } = new List<UI_SelectServerPopup_Item>();
    UI_SelectServerPopup_Item _selectedServer = null;
    Animator _animator = null;

    enum Buttons
    {
        EnterBtn
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.EnterBtn).gameObject.BindEvent(OnClickButton);
        _animator = GetButton((int)Buttons.EnterBtn).gameObject.GetComponentInChildren<Animator>();
        _animator.gameObject.SetActive(false);
    }

    void OnClickButton(PointerEventData evt)
    {
        if(_selectedServer != null)
        {
            _animator.gameObject.SetActive(true);
            Managers.Network.ConnectToGame(_selectedServer.Info);
            Managers.Scene.LoadScene(Define.Scene.Game);
            _animator.gameObject.SetActive(false);
            Managers.UI.ClosePopupUI();
        }
    }

    public void SetSelectedServer(UI_SelectServerPopup_Item selectedServer)
    {
        if (_selectedServer != null)
        {
            _selectedServer.TurnOffCheckMark();
        }
        _selectedServer = selectedServer;
        _selectedServer.TurnOnCheckMark();
    }

    public void SetServers(List<ServerInfo> servers)
    {
        Items.Clear();

        GameObject grid = GetComponentInChildren<GridLayoutGroup>().gameObject;
        foreach (Transform child in grid.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < servers.Count; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Popup/UI_SelectServerPopup_Item", grid.transform);
            UI_SelectServerPopup_Item item = go.GetOrAddComponent<UI_SelectServerPopup_Item>();
            item.ParentUI = this;
            Items.Add(item);

            item.Info = servers[i];
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (Items.Count == 0)
            return;

        foreach(var item in Items)
        {
            item.RefreshUI();
        }
    }
}
