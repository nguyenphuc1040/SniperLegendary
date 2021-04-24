using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher ins;
    public GameObject loading, menu, room_lobby, FindRoomMenu;
    [SerializeField] Text roomname,roomNameCurrent;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    public GameObject StartGameButton;
    public Button JoinRoomWithCodeBtn;
    void Start()
    {
        ins = this;
        
        _setActiveMenu(true,false,false,false);
        StartCoroutine(_timeLoading());
    }
    public bool isExitPanel;
    public GameObject ExitPanel;
    void Update()
    {
        if (string.IsNullOrEmpty(roomname.text))
            JoinRoomWithCodeBtn.interactable = false; else JoinRoomWithCodeBtn.interactable = true;
        if (Input.GetKeyDown(KeyCode.Escape)){
            isExitPanel=!isExitPanel;
            ExitPanel.SetActive(isExitPanel);
        }
    }
    IEnumerator _timeLoading(){
        yield return new WaitForSeconds(2f);
        PhotonNetwork.ConnectUsingSettings();
    }
    public void _back(){
        
        _setActiveMenu(false,true,false,false);
    }
    public void _backtoMenu(){
        StartCoroutine(_backingmenu());
    }
    IEnumerator _backingmenu(){
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
       
            yield return null;
        }
        SceneManager.LoadSceneAsync("Menu");
    }
    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene =  true;
      
    }
    public override void OnJoinedLobby(){
        _setActiveMenu(false,true,false,false);
        PhotonNetwork.NickName = ManagerCtrl.ins._GetUserName();
      
    }
    public void _setActiveMenu(bool x, bool y,bool z, bool a){
        loading.SetActive(x);
        menu.SetActive(y);
        FindRoomMenu.SetActive(z);
        room_lobby.SetActive(a);
        if (y)  if (NameCtrl.ins!=null) NameCtrl.ins._setNameFirstTime();
    }
    public void _CreateRoom(){
    //    if (string.IsNullOrEmpty(roomname.text)){
    //        return;
    //    }
        if (ManagerCtrl.ins!=null){ManagerCtrl.ins._BtnPress();}
        PhotonNetwork.CreateRoom(Random.Range(10000,99999).ToString());
        _setActiveMenu(true,false,false,false);
    }
    public override void OnJoinedRoom(){
        _StartGame();
        // _setActiveMenu(false,false,false,true);
        // roomNameCurrent.text = "#"+ PhotonNetwork.CurrentRoom.Name;
        // Player[] players = PhotonNetwork.PlayerList;
        // foreach(Transform child in PlayerListContent)
		// {
		// 	Destroy(child.gameObject);
		// }
        // for (int i=0; i<players.Count(); i++){
        //     Instantiate(PlayerListItemPrefab,PlayerListContent).GetComponent<NamePlayerItem>().SetUp(players[i]);
        // }
        // StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient){
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCoe, string message){
        _CreateRoom();
       // _back();
    }
    
    public GameObject Notifica;
    public Text Notifica_txt;
    public Animator notificaAnimator;
    public override void OnJoinRoomFailed(short returnCoe, string message){
        notificaAnimator.SetTrigger("ON");
        _back();
    }
    public void _NotificaPress(bool x){
        Notifica.SetActive(x);
    }
    public void _StartGame(){
        PhotonNetwork.LoadLevel(1);
    }
    public void _LeaveRoom(){
        _setActiveMenu(true,false,false,false);
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom(){
        _setActiveMenu(false,true,false,false);
    }
    public void _FindRoom(bool x){
        _setActiveMenu(false,false,true,false);
    }
    public void _JoinRoom(RoomInfo info){
        PhotonNetwork.JoinRoom(info.Name);
        _setActiveMenu(true,false,false,false);      
    }
    public void _JoinRoomWithCODE(){
        
        if (string.IsNullOrEmpty(roomname.text)) return;
        if (ManagerCtrl.ins!=null){ManagerCtrl.ins._BtnPress();}
        string st = roomname.text;
        st = st.Replace("#",string.Empty); 
        PhotonNetwork.JoinRoom(st);
        _setActiveMenu(true,false,false,false);
    }
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
        foreach(Transform trans in roomListContent)
		{
			Destroy(trans.gameObject);
		}
        foreach(RoomInfo room in roomList){
            if (room.RemovedFromList) continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(room);
        }
	}

    public override void OnPlayerEnteredRoom(Player newPlayer){
        Instantiate(PlayerListItemPrefab,PlayerListContent).GetComponent<NamePlayerItem>().SetUp(newPlayer);
    }
}
