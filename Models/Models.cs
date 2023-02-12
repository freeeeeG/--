using UnityEngine;
using QFramework;
using Cinemachine;

public interface IMultiplayerGameModel : IModel
{
    BindableProperty<bool> IsGameStarted { get; set; }
    BindableProperty<int> PlayerCount { get; set; }
    BindableProperty<int> FlagCount { get; set; }
    BindableProperty<int> LightCount { get; set; }
}

public class MultiplayerGameModel : AbstractModel, IMultiplayerGameModel
{
    public BindableProperty<bool> IsGameStarted { get; set; }
    public BindableProperty<int> PlayerCount { get; set; }
    public BindableProperty<int> FlagCount { get; set; }
    public BindableProperty<int> LightCount { get; set; }

    public MultiplayerGameModel()
    {
        IsGameStarted = new BindableProperty<bool>(false);
        PlayerCount = new BindableProperty<int>(0);
        FlagCount = new BindableProperty<int>(0);
        LightCount = new BindableProperty<int>(0);
    }

    protected override void OnInit()
    {
        throw new System.NotImplementedException();
    }
}

#region PlayerMdodel
public interface IPlayerMovementInputModel : IModel
{
    BindableProperty<float> Velocity { get; set; }
    BindableProperty<Animator> anim { get; set; }
    BindableProperty<Camera> cam { get; set; }
    BindableProperty<CharacterController> controller { get; set; }
    BindableProperty<bool> isGrounded { get; set; }
    BindableProperty<Vector3> desiredMoveDirection { get; set; }
    BindableProperty<float> InputX { get; set; }
    BindableProperty<float> InputZ { get; set; }
    BindableProperty<bool> blockRotationPlayer { get; set; }
    BindableProperty<float> desiredRotationSpeed { get; set; }

    BindableProperty<float> Speed { get; set; }

    BindableProperty<float> allowPlayerRotation { get; set; }

    BindableProperty<float> HorizontalAnimSmoothTime { get; set; }
    BindableProperty<float> VerticalAnimTime { get; set; }
    BindableProperty<float> StartAnimTime { get; set; }
    BindableProperty<float> StopAnimTime { get; set; }
    BindableProperty<float> verticalVel { get; set; }
    BindableProperty<Vector3> moveVector { get; set; }
}

public class PlayerMovementInputModel : AbstractModel, IPlayerMovementInputModel
{
    public BindableProperty<float> Velocity { get; set; }
    public BindableProperty<Animator> anim { get; set; }
    public BindableProperty<Camera> cam { get; set; }
    public BindableProperty<CharacterController> controller { get; set; }
    public BindableProperty<bool> isGrounded { get; set; }
    public BindableProperty<Vector3> desiredMoveDirection { get; set; }
    public BindableProperty<float> InputX { get; set; }
    public BindableProperty<float> InputZ { get; set; }
    public BindableProperty<bool> blockRotationPlayer { get; set; }
    public BindableProperty<float> desiredRotationSpeed { get; set; }
    public BindableProperty<float> Speed { get; set; }
    public BindableProperty<float> allowPlayerRotation { get; set; }
    public BindableProperty<float> HorizontalAnimSmoothTime { get; set; }
    public BindableProperty<float> VerticalAnimTime { get; set; }
    public BindableProperty<float> StartAnimTime { get; set; }
    public BindableProperty<float> StopAnimTime { get; set; }
    public BindableProperty<float> verticalVel { get; set; }
    public BindableProperty<Vector3> moveVector { get; set; }

    public PlayerMovementInputModel()
    {
        Velocity = new BindableProperty<float>(5);
        anim = new BindableProperty<Animator>(null);
        cam = new BindableProperty<Camera>(null);
        controller = new BindableProperty<CharacterController>(null);
        isGrounded = new BindableProperty<bool>(false);
        desiredMoveDirection = new BindableProperty<Vector3>(Vector3.zero);
        InputX = new BindableProperty<float>(0);
        InputZ = new BindableProperty<float>(0);
        blockRotationPlayer = new BindableProperty<bool>(false);
        desiredRotationSpeed = new BindableProperty<float>(0.3f);
        Speed = new BindableProperty<float>(0);
        allowPlayerRotation = new BindableProperty<float>(0.1f);
        HorizontalAnimSmoothTime = new BindableProperty<float>(0.2f);
        VerticalAnimTime = new BindableProperty<float>(0.2f);
        StartAnimTime = new BindableProperty<float>(0.3f);
        StopAnimTime = new BindableProperty<float>(0.15f);
        verticalVel = new BindableProperty<float>(-0.5f);
        moveVector = new BindableProperty<Vector3>(Vector3.zero);
    }

    protected override void OnInit() { }
}

public interface IPlayerShootingModel : IModel
{
    BindableProperty<ParticleSystem> inkParticle { get; set; }
    BindableProperty<Transform> parentController { get; set; }
    BindableProperty<Transform> splatGunNozzle { get; set; }
    BindableProperty<CinemachineFreeLook> freeLookCamera { get; set; }
    BindableProperty<CinemachineImpulseSource> impulseSource { get; set; }

}

public class PlayerShootingModel : AbstractModel, IPlayerShootingModel
{
    public BindableProperty<ParticleSystem> inkParticle { get; set; }
    public BindableProperty<Transform> parentController { get; set; }
    public BindableProperty<Transform> splatGunNozzle { get; set; }
    public BindableProperty<CinemachineFreeLook> freeLookCamera { get; set; }
    public BindableProperty<CinemachineImpulseSource> impulseSource { get; set; }


    public PlayerShootingModel()
    {
        inkParticle = new BindableProperty<ParticleSystem>(null);
        parentController = new BindableProperty<Transform>(null);
        splatGunNozzle = new BindableProperty<Transform>(null);
        freeLookCamera = new BindableProperty<CinemachineFreeLook>(null);
        impulseSource = new BindableProperty<CinemachineImpulseSource>(null);
    }

    protected override void OnInit() { }
}

public interface IPlayerBaseModel : IModel
{
    BindableProperty<string> PlayerName { get; set; }
    BindableProperty<int> Health { get; set; }
    BindableProperty<int> MaxHealth { get; set; }
    BindableProperty<Transform> PlayerTransform { get; set; }
}

public class PlayerBaseModel : AbstractModel, IPlayerBaseModel
{
    public BindableProperty<string> PlayerName { get; set; }
    public BindableProperty<int> Health { get; set; }
    public BindableProperty<int> MaxHealth { get; set; }
    public BindableProperty<Transform> PlayerTransform { get; set; }

    public PlayerBaseModel()
    {
        PlayerName = new BindableProperty<string>("Admin");
        Health = new BindableProperty<int>(100);
        MaxHealth = new BindableProperty<int>(100);
        PlayerTransform = new BindableProperty<Transform>(null);
    }

    protected override void OnInit() { }
}


#endregion
