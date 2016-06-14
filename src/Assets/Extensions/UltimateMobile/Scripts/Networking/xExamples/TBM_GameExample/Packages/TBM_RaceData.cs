using UnityEngine;
using System.Collections;

public class TBM_RaceData : MNT_NetworkPackage {

	public int _RaceTime;

	public TBM_RaceData():base(1) {

	}

	public TBM_RaceData(byte[] data):base(data) {
		_RaceTime = ReadValue<int>();
	}


	public void SetRaceTime(int raceTime) {
		_RaceTime = raceTime;
	}


	public void Save() {
		WriteValue<int>(_RaceTime);
	}
}
