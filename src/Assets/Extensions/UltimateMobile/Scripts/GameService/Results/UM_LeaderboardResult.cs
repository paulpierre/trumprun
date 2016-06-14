using UnityEngine;
using System.Collections;

public class UM_LeaderboardResult : UM_Result {

	private UM_Leaderboard _Leaderboard;
	
	
	public UM_LeaderboardResult(UM_Leaderboard leaderboard, ISN_Result result):base(result) {
		Setinfo(leaderboard);
	}
	
	public UM_LeaderboardResult(UM_Leaderboard leaderboard, GooglePlayResult result):base(result) {
		Setinfo(leaderboard);
	}


	
	
	private void Setinfo(UM_Leaderboard leaderboard) {
		_Leaderboard = leaderboard;
	}
	
	
	
	public UM_Leaderboard Leaderboard {
		get {
			return _Leaderboard;
		}
	}
}
