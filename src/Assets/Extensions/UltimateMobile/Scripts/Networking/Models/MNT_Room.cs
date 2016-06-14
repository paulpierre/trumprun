using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MNT_Room  {

	private int _size = 0;
	private byte[] _info;

	private MNT_RoomState _State = MNT_RoomState.CREATED;
	private List<MNT_PlayerTemplate> _Players;


	//Avtions
	public Action<MNT_RoomState> MNT_RoomStateChangedAction = delegate {};


	public MNT_Room(int roomSize = 1, byte[] roomInfo = null) {
		_size = roomSize;
		_info = roomInfo;
		_State = MNT_RoomState.EMPTY;
		_Players =  new List<MNT_PlayerTemplate>();
	} 


	public void AddPlayer(MNT_PlayerTemplate player) {
		_Players.Add(player);

	}


	public void RemovePlayer(MNT_PlayerTemplate player) {
		RemovePlayerById(player.id);
	}

	public void RemovePlayerById(string id) {
		foreach(MNT_PlayerTemplate p in _Players) {
			if(p.id.Equals(id)) {
				_Players.Remove(p);
				return;
			}
		}
	}



	public MNT_PlayerTemplate GetPlayerById(string id) {
		foreach(MNT_PlayerTemplate p in _Players) {
			if(p.id.Equals(id)) {
				return p;
			}
		} 

		Debug.LogError("Player with id: " + id + " not found");
		return null;
	}




	public MNT_RoomState State {
		get {
			return _State;
		}

		set {
			if(_State != value) {
				_State = value;
				MNT_RoomStateChangedAction(_State);
			}
		}
	}

	public List<MNT_PlayerTemplate> Players {
		get {
			return _Players;
		}
	}

	public int size {
		get {
			return _size;
		}
	}

	public byte[] info {
		get {
			return _info;
		}
	}


	// --------------------------------------
	//  Service
	// --------------------------------------


	//method can be only called if there is new user in the room
	public void UpdateRoom(int size, byte[] info, List<MNT_PlayerTemplate> updatedPlayers) {
		_size = size;
		_info = info;
		_Players =  updatedPlayers;

		UpdateState();
	}


	private void UpdateState() {
		if(_Players.Count == size) {
			State = MNT_RoomState.GAME_STARTED;
		} else {
			if(State == MNT_RoomState.CREATED || State == MNT_RoomState.EMPTY) {
				if(_Players.Count >=2) {
					State = MNT_RoomState.CONNECTED;
				}
			}
		}
	}
	
}
