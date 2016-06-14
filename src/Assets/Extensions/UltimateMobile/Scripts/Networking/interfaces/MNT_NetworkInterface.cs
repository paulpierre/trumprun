using UnityEngine;
using System;
using System.Collections;

public interface MNT_NetworkInterface {


	void NI_UnregisterHost();
	void NI_StartRoomSearch(bool ConnectToFirstAvailable = true);
	void NI_StopRoomSearch();


	void NI_Disconnect();

	//Server Methods
	void NI_CreateRoom();
	Action<MNT_PlayerTemplate> NI_OnRoomCreated { get; set; }
	Action<MNT_PlayerTemplate> NI_OnPlayerConnected { get; set; }
	Action<string> NI_OnPlayerDisconnected { get; set; }

	//Client Methods

	Action<MNT_PlayerTemplate> NI_OnConnectedToServer { get; set; }
	Action NI_OnDisconnectedFromServer { get; set; }

	void NI_SendDataToAll(byte[] buffer, MNT_DataType dataType);
	void NI_SendDataToServer(byte[] buffer, MNT_DataType dataType);
	void NI_SendDataToPlayers(byte[] buffer, MNT_DataType dataType, params string[] playersIds);

	Action<byte[]>  NI_OnDataReceived { get; set; }



}
