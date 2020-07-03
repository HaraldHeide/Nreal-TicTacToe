using NRKernal;
using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class TicTacToeManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Message;

    #region GamePieces Prefabs
    public GameObject ex;
    public GameObject oh;
    #endregion GamePieces Prefabs

    private PhotonView photonView;
    private float count = 0;
    private RaycastHit hit;

    #region PlayersTurnNr Logick
    private int PlayerInPlay = 1;
    #endregion

    private byte[,] Board = new byte[3,3];

    void Start ()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        #region PlayersTurnNr Logic
        //Player has to press trigger button to go on
        if (!NRInput.GetButtonDown(ControllerButton.TRIGGER))
        {
            return;
        }

        //Must have 2 players in room
        if (PhotonNetwork.PlayerList.Length < 2)
        {
            return;
        }

        //Must be players turn
        if (PhotonNetwork.LocalPlayer.ActorNumber != PlayerInPlay)
        {
            return;
        }
        #endregion PlayersTurnNr Logic

        #region Select GameField
        // Get controller laser origin.
        Transform laserAnchor = NRInput.AnchorsHelper.GetAnchor(NRInput.RaycastMode == RaycastModeEnum.Gaze ? ControllerAnchorEnum.GazePoseTrackerAnchor : ControllerAnchorEnum.RightLaserAnchor);
        //Ray ray;
        if (Physics.Raycast(new Ray(laserAnchor.transform.position, laserAnchor.transform.forward), out hit, 20))
        {
            photonView.RPC("exoh_prefab", RpcTarget.All, hit.transform.position, Quaternion.identity, hit.transform.name);
        }
        #endregion Select GameField
    }

    //This code is run both on your local instance and all other players instance of the software
    [PunRPC]
    void exoh_prefab(Vector3 _pos, Quaternion _rot, string _gameFieldInPlayName)
    {
        // Have to get new ref of GameObject to get the gmeobjects in both local and remote scene
        GameObject _gameFieldInPlay = GameObject.Find(_gameFieldInPlayName); //Gets destryed both locally and remote
        Destroy(_gameFieldInPlay);

        //Keep track of GameFields occupied
        Track_Move((byte)PlayerInPlay, _gameFieldInPlayName);

        if (count % 2 == 0)
        {
            GameObject Go = Instantiate(ex, _pos, _rot) as GameObject;

            if (Check_Win((byte)PlayerInPlay))
            {
                if (photonView.IsMine)
                {
                    Message.text = "You Win...";
                }
                else
                {
                    Message.text = "You Loose... ";
                }
                return;
            }

            if (photonView.IsMine)
            {
                Message.text = "Other players turn...";
            }
            else
            {
                Message.text = "My turn... ";
            }

            PlayerInPlay = 2;
        }
        else if (count % 1 == 0)
        {
            GameObject Go = Instantiate(oh, _pos, _rot) as GameObject;

            if (Check_Win((byte)PlayerInPlay))
            {
                if (photonView.IsMine)
                {
                    Message.text = "You Win...";
                }
                else
                {
                    Message.text = "You Loose... ";
                }
                return;
            }

            if (photonView.IsMine)
            {
                Message.text = "My turn... ";
            }
            else
            {
                Message.text = "Other players turn...";
            }

            PlayerInPlay = 1;
        }
        count++;
    }

    private void Track_Move(byte _playerInPlay, string _gameFieldInPlayName)
    {
        switch (_gameFieldInPlayName)
        {
            case "Top Left":
                Board[0, 0] = _playerInPlay;
                break;
            case "Top Center":
                Board[1, 0] = _playerInPlay;
                break;
            case "Top Right":
                Board[2, 0] = _playerInPlay;
                break;
            case "Left Center":
                Board[0, 1] = _playerInPlay;
                break;
            case "Middle Center":
                Board[1, 1] = _playerInPlay;
                break;
            case "Right Center":
                Board[2, 1] = _playerInPlay;
                break;
            case "Bottom Left":
                Board[0, 2] = _playerInPlay;
                break;
            case "Bottom Center":
                Board[1, 2] = _playerInPlay;
                break;
            case "Bottom Right":
                Board[2, 2] = _playerInPlay;
                break;
            default:
                break;
        }
    }

    private bool Check_Win(byte _playerinPlay)
    {
        if (Board[0, 0] == _playerinPlay && Board[0, 1] == _playerinPlay && Board[0, 2] == _playerinPlay) //column Left
            return true;
        if (Board[1, 0] == _playerinPlay && Board[1, 1] == _playerinPlay && Board[1, 2] == _playerinPlay) //column middle
            return true;
        if (Board[2, 0] == _playerinPlay && Board[2, 1] == _playerinPlay && Board[2, 2] == _playerinPlay) //column right
            return true;
        if (Board[0, 0] == _playerinPlay && Board[1, 0] == _playerinPlay && Board[2, 0] == _playerinPlay) //row top
            return true;
        if (Board[0, 1] == _playerinPlay && Board[1, 1] == _playerinPlay && Board[2, 1] == _playerinPlay) //row middle
            return true;
        if (Board[0, 2] == _playerinPlay && Board[1, 2] == _playerinPlay && Board[2, 2] == _playerinPlay) //row right
            return true;
        if (Board[0, 0] == _playerinPlay && Board[1, 1] == _playerinPlay && Board[2, 2] == _playerinPlay) //cross Top Left Bottom Right
            return true;
        if (Board[2, 0] == _playerinPlay && Board[1, 1] == _playerinPlay && Board[0, 2] == _playerinPlay) //cross Top Right Bottom Left
            return true;

        return false;
    }
}
