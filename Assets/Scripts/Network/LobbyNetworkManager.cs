using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _roomInput;
    [SerializeField] private RoomItemUI _roomItemUIPrefab;
    [SerializeField] private Transform _roomListParent;
    [SerializeField] private RoomItemUI _playerItemUIPrefab;
    [SerializeField] private Transform _playerListParent;
    [SerializeField] private Text _statusField;
    [SerializeField] private Button _leaveRoomButton;
    [SerializeField] private Text _currentLocationText;
    [SerializeField] private GameObject _roomListWindow;
    [SerializeField] private GameObject _playerListWindow;
    [SerializeField] private GameObject _createRoomListWindow;

    private List<RoomItemUI> _roomList = new List<RoomItemUI>();
    private List<RoomItemUI> _playerList = new List<RoomItemUI>();

    private void Start()
    {
        Initialize();
        Connect();
    }

    #region PhotonCallbacks

    public override void OnConnectedToMaster()
    {
        _statusField.text = "Connected to master server";
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (_statusField == null) { return; }
        _statusField.text = "Disconnected";
    }

    public override void OnJoinedLobby()
    {
        _currentLocationText.text = "Rooms";
    }

    public override void OnJoinedRoom()
    {
        _statusField.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + "room";
        _currentLocationText.text = PhotonNetwork.CurrentRoom.Name;
        _leaveRoomButton.interactable = true;

        ShowWindow(false);
        UpdatePlayerList();
    }

    public override void OnLeftRoom()
    {
        if (_statusField != null)
        {
            _statusField.text = "Lobby";
        }

        if (_currentLocationText != null)
        {
            _currentLocationText.text = "Rooms";
        }
        
        _leaveRoomButton.interactable = false;
        ShowWindow(true);
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    #endregion

    private void Initialize()
    {
        _leaveRoomButton.interactable = false;
    }

    private void Connect()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        //clear current list
        for (int i = 0; i < _roomList.Count; i++)
        {
            Destroy(_roomList[i].gameObject);
        }

        _roomList.Clear();
        //generate updated

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount == 0) { continue; }

            RoomItemUI newRoomItem = Instantiate(_roomItemUIPrefab);
            newRoomItem.LobbyNetworkParent = this;
            newRoomItem.SetName(roomList[i].Name);
            newRoomItem.transform.SetParent(_roomListParent);

            _roomList.Add(newRoomItem);
        }
    }

    private void UpdatePlayerList()
    {
        for (int i = 0; i < _playerList.Count; i++)
        {
            Destroy(_playerList[i].gameObject);
        }

        _playerList.Clear();

        if (PhotonNetwork.CurrentRoom == null) { return; }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            RoomItemUI newPlayerItem = Instantiate(_playerItemUIPrefab);

            newPlayerItem.transform.SetParent(_playerListParent);
            newPlayerItem.SetName(player.Value.NickName);

            _playerList.Add(newPlayerItem);
        }
    }

    private void ShowWindow(bool isRoomList)
    {
        _roomListWindow.SetActive(isRoomList);
        _playerListWindow.SetActive(!isRoomList);
        _createRoomListWindow.SetActive(isRoomList);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(_roomInput.text))
        {
            PhotonNetwork.CreateRoom(_roomInput.text, new RoomOptions() { MaxPlayers = 12 }, null);
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnBackMainMenuPressed()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenuScene");
    }
}
