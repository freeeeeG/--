using QFramework;

public class MultiplayerGameStartCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        //TODO: 调用多人游戏UI
        //TODO: 连接服务器
        //TODO: 创建Player
        //TODO: 注册倒计时系统
        this.SendEvent<MultiplayerGameStartEvent>();
    }
}

public class GameStartCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<GameStartEvent>();
    }
}


public class ReConnectCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<ReConnectEvent>();
    }
}