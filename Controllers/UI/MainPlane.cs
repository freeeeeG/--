using UnityEngine;
using QFramework;
using UnityEngine.UI;

public class MainPlane : MonoBehaviour, IController
{
    GameObject MultiplayerGameStart_Button;
    GameObject ReConnect_Button;
    GameObject Exit_Button;

    private void Start()
    {
        MultiplayerGameStart_Button = GameObject.Find("MultiplayerGameStart");
        ReConnect_Button = GameObject.Find("ReConnect");
        Exit_Button = GameObject.Find("Exit");
        MultiplayerGameStart_Button
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                Debug.Log("MultiplayerGameStart");
                this.SendCommand<MultiplayerGameStartCommand>();
            });
        Exit_Button
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                Application.Quit();
            });
        ReConnect_Button
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                ReConnect_Button.SetActive(false);
                this.SendCommand<ReConnectCommand>();
            });


        // 事件binding
        this.RegisterEvent<ReConnectEvent>(e =>
        {
            MultiplayerGameStart_Button.SetActive(true);
            Exit_Button.SetActive(true);
        });
        this.RegisterEvent<JoinRoomFailedEvent>(e =>
        {
            gameObject.SetActive(true);
            MultiplayerGameStart_Button.SetActive(false);
            Exit_Button.SetActive(false);
            ReConnect_Button.SetActive(true);
        });
        ReConnect_Button.SetActive(false);
    }

    public IArchitecture GetArchitecture()
    {
        return Blockade.Interface;
    }
}
