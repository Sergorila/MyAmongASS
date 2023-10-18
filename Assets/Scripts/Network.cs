using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Network : MonoBehaviourPunCallbacks
{

    public Text statusText;

    public CameraFollow playerCamera;

    void Start()
    {
        statusText.text = "Connecting";
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        statusText.text = "Connected to Master / join";
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions() { MaxPlayers = 12 }, null);
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "Connected";
        playerCamera.target = PhotonNetwork.Instantiate("Player",
            new Vector3(
                Random.Range(-2, 2),
                Random.Range(-2, 2),
                0), Quaternion.identity).transform;
    }
}
