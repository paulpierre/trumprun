using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UM_TBM_MatchesLoadResult : UM_Result {

	private List<UM_TBM_Match> _Matches =  new List<UM_TBM_Match>();
	private List<UM_TBM_Invite> _Invitations =  new List<UM_TBM_Invite>();
	
	
	public UM_TBM_MatchesLoadResult(ISN_Result result):base(result) {}
	public UM_TBM_MatchesLoadResult(GooglePlayResult result):base(result)  {}
	
	
	
	public void SetMatches(List<UM_TBM_Match> matches) {
		_Matches = matches;
	}

	public void SetInvitations(List<UM_TBM_Invite> invitations) {
		_Invitations = invitations;
	}
	
	public List<UM_TBM_Match> Matches {
		get {
			return _Matches;
		}
	}

	public List<UM_TBM_Invite> Invitations {
		get {
			return _Invitations;
		}
	}
}
