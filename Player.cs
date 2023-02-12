using QFramework;
using UnityEngine;
public class Player : Architecture<Player>
{
    protected override void Init()
    {
        Debug.Log("Player Init");
        this.RegisterSystem<PlayerMovementSystem>(new PlayerMovementSystem());
        this.RegisterModel<PlayerBaseModel>(new PlayerBaseModel());
        this.RegisterModel<PlayerMovementInputModel>(new PlayerMovementInputModel());
        this.RegisterSystem<PlayerShootingSystem>(new PlayerShootingSystem());
        this.RegisterModel<PlayerShootingModel>(new PlayerShootingModel());
    }
}
