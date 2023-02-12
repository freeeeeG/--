using QFramework;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerController : MonoBehaviourPun, IController
{
    [SerializeField]
    Transform parentController;

    [SerializeField]
    Transform splatGunNozzle;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        this.RegisterEvent<GameStartEvent>(GameStart);
    }

    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        Init();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        this.GetModel<PlayerShootingModel>().splatGunNozzle = new BindableProperty<Transform>(
            splatGunNozzle
        );
        this.GetModel<PlayerShootingModel>().parentController = new BindableProperty<Transform>(
            parentController
        );
        this.GetModel<PlayerBaseModel>().PlayerTransform = new BindableProperty<Transform>(
            transform
        );
        this.GetSystem<PlayerMovementSystem>().Update();
        this.GetSystem<PlayerShootingSystem>().Update();
    }

    void GameStart(GameStartEvent e)
    {
        Debug.Log("Player Start");
    }

    void Init()
    {
        PlayerMovementInputModelInit();
        PlayerShootingModelInit();
    }

    void PlayerMovementInputModelInit()
    {
        this.GetModel<PlayerMovementInputModel>().cam = new BindableProperty<Camera>(Camera.main);
        this.GetModel<PlayerMovementInputModel>().anim = new BindableProperty<Animator>(
            this.GetComponent<Animator>()
        );
        this.GetModel<PlayerMovementInputModel>().controller =
            new BindableProperty<CharacterController>(this.GetComponent<CharacterController>());
    }

    void PlayerShootingModelInit()
    {
        this.GetModel<PlayerShootingModel>().inkParticle = new BindableProperty<ParticleSystem>(
            GameObject.Find("MainVisualParticle").GetComponent<ParticleSystem>()
        );
        this.GetModel<PlayerShootingModel>().freeLookCamera =
            new BindableProperty<CinemachineFreeLook>(
                GameObject.Find("ThirdPersonCamera").GetComponent<CinemachineFreeLook>()
            );
        this.GetModel<PlayerShootingModel>().freeLookCamera.Value.LookAt = this.transform;
        this.GetModel<PlayerShootingModel>().freeLookCamera.Value.Follow = this.transform;
        this.GetModel<PlayerShootingModel>().impulseSource =
            new BindableProperty<CinemachineImpulseSource>(
                this.GetModel<PlayerShootingModel>()
                    .freeLookCamera.Value.GetComponent<CinemachineImpulseSource>()
            );
    }

    public IArchitecture GetArchitecture()
    {
        return Player.Interface;
    }
}
