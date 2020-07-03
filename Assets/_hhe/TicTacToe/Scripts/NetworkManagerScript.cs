using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class NetworkManagerScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text Message;

    private void Awake()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        Message.text = "";
        PhotonNetwork.ConnectUsingSettings();
    }

     #region PUN2 CallBacks
    public override void OnConnected()  // Has reached internett
    {
        base.OnConnected();
        //Message.text = "Connected...";
    }

    public override void OnConnectedToMaster()
    {
        //Message.text = "Connected to Master";
        //PhotonNetwork.JoinLobby(TypedLobby.Default);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("TicTacToe", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        //Message.text = "Joined Lobby...";
    }

    public override void OnJoinedRoom()
    {
        //Message.text = "Joined Room...";
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            PhotonNetwork.LocalPlayer.NickName = "Black";
            //StartCoroutine(SpawnMyPlayer1());
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = "White";
            //StartCoroutine(SpawnMyPlayer2());
        }

        if(PhotonNetwork.CountOfPlayers == 1)
        {
            Message.text = "Waiting for other player...";
        }
        else
        {
            //Message.text = PhotonNetwork.LocalPlayer.NickName + " Joined Room " + PhotonNetwork.CurrentRoom.Name +
            //        " now containing " + PhotonNetwork.CountOfPlayers.ToString();
            Message.text = "Other players turn...";
        }

        //PhotonNetwork.LoadLevel("Test");  //Index of scene in building list Not in use here
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Message.text = "My turn...";
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Message.text = "Other player left...";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }
    #endregion

    //IEnumerator SpawnMyPlayer1()
    //{
    //    yield return new WaitForSeconds(1f);
    //}
    //IEnumerator SpawnMyPlayer2()
    //{
    //    yield return new WaitForSeconds(1f);
    //}
}