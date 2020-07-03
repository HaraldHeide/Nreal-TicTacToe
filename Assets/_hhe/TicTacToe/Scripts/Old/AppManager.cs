using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;
using TMPro;

//public class AppManager : Singleton<AppManager>
public class AppManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Message;

    private const int MAXPLAYERS = 2;

    public int PlayerInPlay;

    public GameObject ex;
    public GameObject oh;
    private float count = 0;

    PhotonView view;

    [HideInInspector]
    public static GameObject GameFieldInPlay;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    void Start()
    {
        PlayerInPlay = 1;
    }

    public void Called_From_GameField() // CAN HAVE PARAMETERS
    {
        Message.text = "Kilroy1: " + GameFieldInPlay.name;

        if (PhotonNetwork.PlayerList.Length < 2)
        {
            return;
        }

        Message.text = "Kilroy2: " + GameFieldInPlay.name;

        if (PhotonNetwork.LocalPlayer.ActorNumber != PlayerInPlay)
        {
            return;
        }

        Message.text = "Kilroy3: " + GameFieldInPlay.name;
        GameFieldInPlay.GetComponent<PlacePiece>().Taken = true;
        Message.text = "Kilroy4: " + GameFieldInPlay.name;

        view.RPC("exoh_prefab", RpcTarget.All, GameFieldInPlay.transform.position, Quaternion.identity);
        Message.text = "Actornr: " + PhotonNetwork.LocalPlayer.ActorNumber.ToString() + " PlayerInPlay: " + PlayerInPlay.ToString();

    }

    [PunRPC]
    void exoh_prefab(Vector3 _position, Quaternion _rotation)
    {
        PlayerInPlay++;
        if (PlayerInPlay > MAXPLAYERS)
        {
            PlayerInPlay = 1;
        }

        if (count % 2 == 0)
        {
            GameObject Go = Instantiate(oh, _position, _rotation) as GameObject;
        }
        else if (count % 1 == 0)
        {
            GameObject Go = Instantiate(ex, _position, _rotation) as GameObject;
        }
        count++;
    }
}
