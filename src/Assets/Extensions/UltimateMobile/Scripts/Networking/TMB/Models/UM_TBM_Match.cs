using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UM_TBM_Match  {
	
	private string _Id;
	private byte[] _Data;
	private bool _IsCurrentPlayerTurn = false;
	private UM_TBM_MatchStatus _Status =  UM_TBM_MatchStatus.Unknown;

	private UM_TBM_Participant _CurrentParticipant = null;
	private List<UM_TBM_Participant> _Participants =  new List<UM_TBM_Participant>();
	

	public UM_TBM_Match(GK_TBM_Match match) {


		_Id = match.Id;
		_Data = match.Data;


		foreach(GK_TBM_Participant participant in match.Participants) {
			UM_TBM_Participant p = new UM_TBM_Participant(participant);
			_Participants.Add(p);
		}

		if(match.CurrentParticipant != null) {
			_CurrentParticipant = new UM_TBM_Participant(match.CurrentParticipant);
			if(_CurrentParticipant.Playerid.Equals(GameCenterManager.Player.Id)) {
				_IsCurrentPlayerTurn = true;
			}
		}


		switch(match.Status) {
		case GK_TurnBasedMatchStatus.Unknown:
			_Status = UM_TBM_MatchStatus.Unknown;
			break;

		case GK_TurnBasedMatchStatus.Open:
			_Status = UM_TBM_MatchStatus.Active;
			break;

		case GK_TurnBasedMatchStatus.Matching:
			_Status = UM_TBM_MatchStatus.Matching;
			break;

		case GK_TurnBasedMatchStatus.Ended:
			_Status = UM_TBM_MatchStatus.Ended;
			break;

		}

	}


	public UM_TBM_Match(GP_TBM_Match match) {
		_Id = match.Id;
		_Data = match.Data;

		foreach(GP_Participant participant in match.Participants) {
			UM_TBM_Participant p = new UM_TBM_Participant(participant);
			_Participants.Add(p);
			if(match.PendingParticipantId.Equals(participant.id)) {
				_CurrentParticipant = p;
			}
		}


		if(match.TurnStatus == GP_TBM_MatchTurnStatus.MATCH_TURN_STATUS_MY_TURN) {
			_IsCurrentPlayerTurn = true;
		}


		switch(match.Status) {
		case GP_TBM_MatchStatus.MATCH_STATUS_ACTIVE:
			_Status = UM_TBM_MatchStatus.Active;
			break;

		case GP_TBM_MatchStatus.MATCH_STATUS_AUTO_MATCHING:
			_Status = UM_TBM_MatchStatus.Matching;
			break;

		case GP_TBM_MatchStatus.MATCH_STATUS_CANCELED:
		case GP_TBM_MatchStatus.MATCH_STATUS_COMPLETE:
		case GP_TBM_MatchStatus.MATCH_STATUS_EXPIRED:
			_Status = UM_TBM_MatchStatus.Ended;
			break;
			
		
			
		}
	}


	//--------------------------------------
	// Public Methods
	//--------------------------------------


	public void TakeTrun(byte[] matchData, UM_TBM_Participant nextParticipant = null) {
		if(nextParticipant == null) {
			nextParticipant = NextParticipant;
		}
		TBM.Matchmaker.TakeTurn(Id, matchData, nextParticipant);
	}

	public void QuitInTurn(UM_TBM_Participant nextParticipant) {
		TBM.Matchmaker.QuitInTurn(Id, nextParticipant);
	}

	public void QuitOutOfTurn() {
		TBM.Matchmaker.QuitOutOfTurn(Id);
	}

	public void Finish(byte[] matchData,  params UM_TMB_ParticipantResult[] results) {
		TBM.Matchmaker.FinishMatch(Id, matchData, results);
	}

	public void Rematch() {
		TBM.Matchmaker.Rematch(Id);
	}


	public void Win(byte[] matchData) {

		List<UM_TMB_ParticipantResult> results = new List<UM_TMB_ParticipantResult>();

		foreach(UM_TBM_Participant p in Participants) {

			UM_TMB_ParticipantResult r;
			if(p == LocalParticipant) {
				r = new UM_TMB_ParticipantResult(p.Id, UM_TBM_Outcome.Won);
			} else {
				r = new UM_TMB_ParticipantResult(p.Id, UM_TBM_Outcome.Lost);
			}

			results.Add(r);
		}

		Finish(matchData, results.ToArray());
	}
	
	public void Lose(byte[] matchData) {
		List<UM_TMB_ParticipantResult> results = new List<UM_TMB_ParticipantResult>();
		
		foreach(UM_TBM_Participant p in Participants) {
			
			UM_TMB_ParticipantResult r;
			if(p == LocalParticipant) {
				r = new UM_TMB_ParticipantResult(p.Id, UM_TBM_Outcome.Lost);
			} else {
				r = new UM_TMB_ParticipantResult(p.Id, UM_TBM_Outcome.Won);
			}
			
			results.Add(r);
		}
		
		Finish(matchData, results.ToArray());
	}
	
	public void Tie(byte[] matchData) {
		List<UM_TMB_ParticipantResult> results = new List<UM_TMB_ParticipantResult>();
		
		foreach(UM_TBM_Participant p in Participants) {
			UM_TMB_ParticipantResult r = new UM_TMB_ParticipantResult(p.Id, UM_TBM_Outcome.Tied);
			results.Add(r);
		}
		
		Finish(matchData, results.ToArray());
	}



	public void Remove() {
		TBM.Matchmaker.RemoveMatch(Id);
	}



	//--------------------------------------
	// Get / Set
	//--------------------------------------


	public string Id {
		get {
			return _Id;
		}
	}

	public byte[] Data {
		get {
			return _Data;
		}
	}

	public bool IsLocalPlayerTurn {
		get {
			return _IsCurrentPlayerTurn;
		}
	}

	public bool IsEnded {
		get {
			return Status == UM_TBM_MatchStatus.Ended;
		}
	}

	public UM_TBM_MatchStatus Status {
		get {
			return _Status;
		}
	}

	public UM_TBM_Participant CurrentParticipant {
		get {
			return _CurrentParticipant;
		}
	}

	public List<UM_TBM_Participant> Participants {
		get {
			return _Participants;
		}
	}


	/// <summary>
	/// Can be used only for matches where total participants count 2
	/// </summary>
	public UM_TBM_Participant NextParticipant {
		get {
			foreach(UM_TBM_Participant p in Participants) {

				if(!p.Id.Equals(CurrentParticipant.Id)) {
					return p;
				}
			}
			return null;
		}
	}

	/// <summary>
	/// Can be used only for matches where total participants count 2
	/// </summary>
	public UM_TBM_Participant Competitor {
		get {
			foreach(UM_TBM_Participant p in Participants) {
				if(!p.Playerid.Equals(UM_GameServiceManager.Instance.Player.PlayerId )) {
					return p;
				}
			}
			return null;
		}
	}



	/// <summary>
	/// Return UM_TBM_Participant object of the local player
	/// </summary>
	public UM_TBM_Participant LocalParticipant {
		get {
			foreach(UM_TBM_Participant p in Participants) {
				if(p.Playerid.Equals(UM_GameServiceManager.Instance.Player.PlayerId )) {
					return p;
				}
			}
			return null;
		}
	}


	
}
