using UnityEngine;
using Photon.Pun;
using QFramework;

interface INetWorkSystem : ISystem { }

public class NetWorkSystem : AbstractSystem, INetWorkSystem
{
    GameObject networkLaughter = GameObject.Find("NetworkLaughter");

    protected override void OnInit()
    {
        this.RegisterEvent<GameStartEvent>(
            e => networkLaughter.GetComponent<NetworkLaughter>().OnGameStart()
        );
        this.RegisterEvent<MultiplayerGameStartEvent>(e =>
        {
            if (!networkLaughter.GetComponent<NetworkLaughter>().JoinedRoom())
                this.SendEvent<JoinRoomFailedEvent>();
        });
    }
}

public class NetworkLaughter : MonoBehaviourPunCallbacks
{
    public void OnGameStart()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to server...");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master");
    }

    public bool JoinedRoom()
    {
        return PhotonNetwork.JoinOrCreateRoom(
            "Room",
            new Photon.Realtime.RoomOptions() { MaxPlayers = 20 },
            default
        );
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }
}
