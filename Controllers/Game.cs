using UnityEngine;
using QFramework;

public class Game : MonoBehaviour,IController
{
    private void Awake() {
        this.RegisterEvent<GameStartEvent>(OnGameStart);
        this.SendCommand<GameStartCommand>();
    }
    private void OnGameStart(GameStartEvent e)
    {
        Debug.Log("Game Start");
        //TODO: 调用主界面
        
    }
    private void Update() {

    }
    public IArchitecture GetArchitecture()
    {
        return Blockade.Interface;
    }

}
