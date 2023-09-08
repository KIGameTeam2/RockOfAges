using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TitleManager : MonoBehaviourPunCallbacks
{
    // ������ ������ �÷��̾��� �̸��� ������ ����Ʈ
    public static List<string> playerNames;
    // ���� ���� ����
    private TMP_Text serverStatus;

    private TMP_InputField playerNameSpace;
    private Button optionButton;
    private Button startButton;


    private void Awake()
    {
        playerNames = new List<string>();
        serverStatus = GameObject.Find("ServerStatus").GetComponentInChildren<TMP_Text>();

        playerNameSpace = GameObject.Find("NameSpace").GetComponent<TMP_InputField>();
        optionButton = GameObject.Find("Button_Option").GetComponent<Button>();
        startButton = GameObject.Find("Button_Start").GetComponent<Button>();
    }

    void Start()
    {
#if PhotonSymbol
        PhotonNetwork.ConnectUsingSettings();
        startButton.interactable = false;
        serverStatus.text = "Connecting to Master Server";
        serverStatus.color = Color.yellow;
#endif
    }

    void Update()
    {
        
    }


    #region ���� : �����ͼ��� ����
#if PhotonSymbol
    public override void OnConnectedToMaster()
    {
        startButton.interactable = true;
        serverStatus.text = "Online";
        serverStatus.color = Color.green;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        startButton.interactable = false;
        serverStatus.text = "Offline\nConnecting to Master Server...";
        serverStatus.color = Color.red;
        PhotonNetwork.ConnectUsingSettings();
    }
#endif
    #endregion


    #region ���� : �÷��̾� �̸� �� �� ����
    private void CheckPlayerName() 
    {

    }

    private void RecorPlayerName() 
    {
    
    }
    #endregion
}
