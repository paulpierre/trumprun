using UnityEngine;
using System.Collections;

public class UM_TBM_MatchResult : UM_Result {

	private UM_TBM_Match _Match;


	public UM_TBM_MatchResult(ISN_Result result):base(result) {}
	public UM_TBM_MatchResult(GooglePlayResult result):base(result)  {}


	
	public void SetMatch(UM_TBM_Match match) {
		_Match = match;
	}

	public UM_TBM_Match Match {
		get {
			return _Match;
		}
	}
}
