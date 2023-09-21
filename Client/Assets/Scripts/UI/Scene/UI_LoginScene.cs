using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LoginScene : UI_Scene
{
    enum GameObjects
    {
        Account,
        Password
    }

    enum Buttons
    {
        CreateBtn,
        LoginBtn
    }

    Animator _animator;

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.CreateBtn).gameObject.BindEvent(OnClickCreateButton);
        GetButton((int)Buttons.LoginBtn).gameObject.BindEvent(OnClickLoginButton);

        _animator = Util.FindChild<Animator>(GetButton((int)Buttons.LoginBtn).gameObject, "LoginBtnLoading", true);
        _animator.gameObject.SetActive(false);
    }

    public void OnClickCreateButton(PointerEventData evt)
    {
        string account = Get<GameObject>((int)GameObjects.Account).GetComponent<InputField>().text;
        string password = Get<GameObject>((int)GameObjects.Password).GetComponent<InputField>().text;

        CreateAccountPacketReq packet = new CreateAccountPacketReq()
        {
            AccountName = account,
            Password = password
        };

        Managers.Web.SendPostRequest<CreateAccountPacketRes>("account/create", packet, (res) =>
        {
            Debug.Log(res.CreateOk);

            Get<GameObject>((int)GameObjects.Account).GetComponent<InputField>().text = "";
            Get<GameObject>((int)GameObjects.Password).GetComponent<InputField>().text = "";
        });
    }

    public void OnClickLoginButton(PointerEventData evt)
    {
        string account = Get<GameObject>((int)GameObjects.Account).GetComponent<InputField>().text;
        string password = Get<GameObject>((int)GameObjects.Password).GetComponent<InputField>().text;

        LoginAccountPacketReq packet = new LoginAccountPacketReq()
        {
            AccountName = account,
            Password = password
        };

        _animator.gameObject.SetActive(true);
        Managers.Web.SendPostRequest<LoginAccountPacketRes>("account/login", packet, (res) =>
        {
            Debug.Log(res.LoginOk);
            Get<GameObject>((int)GameObjects.Account).GetComponent<InputField>().text = "";
            Get<GameObject>((int)GameObjects.Password).GetComponent<InputField>().text = "";
            _animator.gameObject.SetActive(false);

            if (res.LoginOk)
            {
                Managers.Network.ConnectToGame();
                Managers.Scene.LoadScene(Define.Scene.Game);
            }
        });
    }
}
