using UnityEngine;
using QFramework;
using DG.Tweening;
#region BlockadeSystem
interface IAchievementSystem : ISystem { }

public class AchievementSystem : AbstractSystem, IAchievementSystem
{
    protected override void OnInit() { }
}

interface IMultiplayerSystem : ISystem
{
    void Update();
}

public class MultiplayerSystem : AbstractSystem, IMultiplayerSystem
{
    protected override void OnInit() { }

    public void Update() { }
}

interface IUISystem : ISystem
{
    void Update();
}

public class UISystem : AbstractSystem, IUISystem
{
    GameObject mainPlane = GameObject.Find("MainPlane");
    GameObject multiplayerGamePlane = GameObject.Find("MultiplayerGamePlane");

    protected override void OnInit() { }

    public void Update() { }
}

#endregion


#region PlayerSystem

interface IPlayerShootingSystem : ISystem
{
    void Update();
}

public class PlayerShootingSystem : AbstractSystem, IPlayerShootingSystem
{
    float lastShootTime = 0;
    float shootInterval = 0.2f;

    protected override void OnInit()
    {
        lastShootTime = Time.time;
    }

    public void Update()
    {
        bool pressing = Input.GetMouseButton(0);
        Vector3 angle =
            this.GetModel<PlayerShootingModel>().parentController.Value.transform.localEulerAngles;
        this.GetModel<PlayerMovementInputModel>().blockRotationPlayer = new BindableProperty<bool>(
            pressing
        );
        if (pressing)
        {
            VisualPolish();
            this.GetSystem<PlayerMovementSystem>()
                .RotateToCamera(this.GetModel<PlayerBaseModel>().PlayerTransform);
        }

        if (pressing && Time.time - lastShootTime > shootInterval)
        {
            lastShootTime = Time.time;
            var bullet = PublicComponents.Interface.GetSystem<FactorySystem>().UseGoods<Bullet>();
            bullet.SetActive(true);
            Debug.Log(
                this.GetModel<PlayerShootingModel>().splatGunNozzle.Value.position.x
                    + " "
                    + this.GetModel<PlayerShootingModel>().splatGunNozzle.Value.position.y
                    + " "
                    + this.GetModel<PlayerShootingModel>().splatGunNozzle.Value.position.z
            );
            Debug.Log(
                "方向"
                    + this.GetModel<PlayerShootingModel>().splatGunNozzle.Value.forward.x
                    + " "
                    + this.GetModel<PlayerShootingModel>().splatGunNozzle.Value.forward.y
                    + " "
                    + this.GetModel<PlayerShootingModel>().splatGunNozzle.Value.forward.z
            );
            bullet.transform.position =
                this.GetModel<PlayerShootingModel>().splatGunNozzle.Value.position
                // + -this.GetModel<PlayerShootingModel>().splatGunNozzle.Value.forward * 1.5f;
                + this.GetModel<PlayerShootingModel>().parentController.Value.transform.forward * 2f;     
            bullet.GetComponent<Rigidbody>().velocity =this.GetModel<PlayerShootingModel>().parentController.Value.transform.forward * 10f;
                
        }
        this.GetModel<PlayerShootingModel>().parentController.Value.transform.localEulerAngles =
            new Vector3(
                Mathf.LerpAngle(
                    this.GetModel<PlayerShootingModel>().parentController.Value.transform.localEulerAngles.x,
                    pressing
                        ? RemapCamera(
                            this.GetModel<PlayerShootingModel>().freeLookCamera.Value.m_YAxis.Value,
                            0,
                            1,
                            -25,
                            25
                        )
                        : 0,
                    .3f
                ),
                angle.y,
                angle.z
            );
    }

    void VisualPolish()
    {
        if (!DOTween.IsTweening(this.GetModel<PlayerShootingModel>().parentController.Value))
        {
            this.GetModel<PlayerShootingModel>().parentController.Value.DOComplete();
            Vector3 forward =
                -this.GetModel<PlayerShootingModel>().parentController.Value.transform.forward;
            Vector3 localPos =
                this.GetModel<PlayerShootingModel>().parentController.Value.transform.localPosition;
            this.GetModel<PlayerShootingModel>()
                .parentController.Value.DOLocalMove(localPos - new Vector3(0, 0, .2f), .03f)
                .OnComplete(
                    () =>
                        this.GetModel<PlayerShootingModel>()
                            .parentController.Value.DOLocalMove(localPos, .1f)
                            .SetEase(Ease.OutSine)
                );

            this.GetModel<PlayerShootingModel>().impulseSource.Value.GenerateImpulse();
        }

        if (!DOTween.IsTweening(this.GetModel<PlayerShootingModel>().splatGunNozzle.Value))
        {
            this.GetModel<PlayerShootingModel>().splatGunNozzle.Value.DOComplete();
            this.GetModel<PlayerShootingModel>()
                .splatGunNozzle.Value.DOPunchScale(new Vector3(0, 1, 1) / 1.5f, .15f, 10, 1);
        }
    }

    float RemapCamera(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

interface IPlayerMovementSystem : ISystem
{
    void Update();
}

public class PlayerMovementSystem : AbstractSystem, IPlayerMovementSystem
{
    protected override void OnInit() { }

    public void Update()
    {
        InputMagnitude();
        this.GetModel<PlayerMovementInputModel>().isGrounded = new BindableProperty<bool>(
            this.GetModel<PlayerMovementInputModel>().controller.Value.isGrounded
        );
        if (this.GetModel<PlayerMovementInputModel>().isGrounded.Value)
        {
            this.GetModel<PlayerMovementInputModel>().verticalVel.Value -= 0;
        }
        else
        {
            this.GetModel<PlayerMovementInputModel>().verticalVel.Value -= 1f;
        }
        this.GetModel<PlayerMovementInputModel>().moveVector.Value = new Vector3(
            0,
            this.GetModel<PlayerMovementInputModel>().verticalVel.Value * 0.2f * Time.deltaTime,
            0
        );
        this.GetModel<PlayerMovementInputModel>()
            .controller.Value.Move(this.GetModel<PlayerMovementInputModel>().moveVector);
    }

    void PlayerMoveAndRotation()
    {
        this.GetModel<PlayerMovementInputModel>().InputX = new BindableProperty<float>(
            Input.GetAxis("Horizontal")
        );
        this.GetModel<PlayerMovementInputModel>().InputZ = new BindableProperty<float>(
            Input.GetAxis("Vertical")
        );
        var camera = Camera.main;
        var forward = this.GetModel<PlayerMovementInputModel>().cam.Value.transform.forward;
        var right = this.GetModel<PlayerMovementInputModel>().cam.Value.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        this.GetModel<PlayerMovementInputModel>().desiredMoveDirection =
            new BindableProperty<Vector3>(
                forward * this.GetModel<PlayerMovementInputModel>().InputZ
                    + right * this.GetModel<PlayerMovementInputModel>().InputX
            );

        if (this.GetModel<PlayerMovementInputModel>().blockRotationPlayer == false)
        {
            //Camera
            this.GetModel<PlayerBaseModel>().PlayerTransform.Value.rotation = Quaternion.Slerp(
                this.GetModel<PlayerBaseModel>().PlayerTransform.Value.rotation,
                Quaternion.LookRotation(
                    this.GetModel<PlayerMovementInputModel>().desiredMoveDirection
                ),
                this.GetModel<PlayerMovementInputModel>().desiredRotationSpeed
            );
            this.GetModel<PlayerMovementInputModel>()
                .controller.Value.Move(
                    this.GetModel<PlayerMovementInputModel>().desiredMoveDirection.Value
                        * Time.deltaTime
                        * this.GetModel<PlayerMovementInputModel>().Velocity
                );
        }
        else
        {
            //Strafe
            this.GetModel<PlayerMovementInputModel>()
                .controller.Value.Move(
                    (
                        this.GetModel<PlayerBaseModel>().PlayerTransform.Value.forward
                            * this.GetModel<PlayerMovementInputModel>().InputZ
                        + this.GetModel<PlayerBaseModel>().PlayerTransform.Value.right
                            * this.GetModel<PlayerMovementInputModel>().InputX
                    )
                        * Time.deltaTime
                        * this.GetModel<PlayerMovementInputModel>().Velocity
                );
        }
    }

    public void RotateToCamera(Transform t)
    {
        var forward = this.GetModel<PlayerMovementInputModel>().cam.Value.transform.forward;

        this.GetModel<PlayerMovementInputModel>().desiredMoveDirection =
            new BindableProperty<Vector3>(forward);
        Quaternion lookAtRotation = Quaternion.LookRotation(
            this.GetModel<PlayerMovementInputModel>().desiredMoveDirection
        );
        Quaternion lookAtRotationOnly_Y = Quaternion.Euler(
            this.GetModel<PlayerBaseModel>().PlayerTransform.Value.rotation.eulerAngles.x,
            lookAtRotation.eulerAngles.y,
            this.GetModel<PlayerBaseModel>().PlayerTransform.Value.rotation.eulerAngles.z
        );

        t.rotation = Quaternion.Slerp(
            this.GetModel<PlayerBaseModel>().PlayerTransform.Value.rotation,
            lookAtRotationOnly_Y,
            this.GetModel<PlayerMovementInputModel>().desiredRotationSpeed
        );
    }

    public void LookAt(Vector3 pos)
    {
        this.GetModel<PlayerBaseModel>().PlayerTransform.Value.rotation = Quaternion.Slerp(
            this.GetModel<PlayerBaseModel>().PlayerTransform.Value.rotation,
            Quaternion.LookRotation(pos),
            this.GetModel<PlayerMovementInputModel>().desiredRotationSpeed
        );
    }

    void InputMagnitude()
    {
        this.GetModel<PlayerMovementInputModel>().InputX = new BindableProperty<float>(
            Input.GetAxis("Horizontal")
        );
        this.GetModel<PlayerMovementInputModel>().InputZ = new BindableProperty<float>(
            Input.GetAxis("Vertical")
        );
        this.GetModel<PlayerMovementInputModel>().Speed = new BindableProperty<float>(
            new Vector2(
                this.GetModel<PlayerMovementInputModel>().InputX,
                this.GetModel<PlayerMovementInputModel>().InputZ
            ).sqrMagnitude
        );
        this.GetModel<PlayerMovementInputModel>()
            .anim.Value.SetBool(
                "shooting",
                this.GetModel<PlayerMovementInputModel>().blockRotationPlayer
            );

        if (
            this.GetModel<PlayerMovementInputModel>().Speed
            > this.GetModel<PlayerMovementInputModel>().allowPlayerRotation
        )
        {
            this.GetModel<PlayerMovementInputModel>()
                .anim.Value.SetFloat(
                    "Blend",
                    this.GetModel<PlayerMovementInputModel>().Speed,
                    this.GetModel<PlayerMovementInputModel>().StartAnimTime,
                    Time.deltaTime
                );
            this.GetModel<PlayerMovementInputModel>()
                .anim.Value.SetFloat(
                    "X",
                    this.GetModel<PlayerMovementInputModel>().InputX,
                    this.GetModel<PlayerMovementInputModel>().StartAnimTime / 3,
                    Time.deltaTime
                );

            this.GetModel<PlayerMovementInputModel>()
                .anim.Value.SetFloat(
                    "Y",
                    this.GetModel<PlayerMovementInputModel>().InputZ,
                    this.GetModel<PlayerMovementInputModel>().StartAnimTime / 3,
                    Time.deltaTime
                );
            PlayerMoveAndRotation();
        }
        else if (
            this.GetModel<PlayerMovementInputModel>().Speed
            < this.GetModel<PlayerMovementInputModel>().allowPlayerRotation
        )
        {
            this.GetModel<PlayerMovementInputModel>()
                .anim.Value.SetFloat(
                    "Blend",
                    this.GetModel<PlayerMovementInputModel>().Speed,
                    this.GetModel<PlayerMovementInputModel>().StartAnimTime,
                    Time.deltaTime
                );
            this.GetModel<PlayerMovementInputModel>()
                .anim.Value.SetFloat(
                    "X",
                    0,
                    this.GetModel<PlayerMovementInputModel>().StartAnimTime / 3,
                    Time.deltaTime
                );
            this.GetModel<PlayerMovementInputModel>()
                .anim.Value.SetFloat(
                    "Y",
                    0,
                    this.GetModel<PlayerMovementInputModel>().StartAnimTime / 3,
                    Time.deltaTime
                );
        }
    }
}


#endregion
