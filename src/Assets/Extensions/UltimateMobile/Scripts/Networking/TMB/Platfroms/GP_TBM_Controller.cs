using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GP_TBM_Controller : iTBM_Matchmaker {


	public event Action<UM_TBM_MatchResult> MatchFoundEvent	 	= delegate {};
	public event Action<UM_TBM_MatchResult> MatchLoadedEvent	= delegate {};


	public event Action<UM_TBM_MatchResult> InvitationAccepted	= delegate {};
	public event Action<string> InvitationDeclined				= delegate {};

	public event Action<UM_TBM_MatchResult> TurnEndedEvent		= delegate {};
	public event Action<UM_TBM_MatchResult> MatchUpdatedEvent	= delegate {};


	public event Action<UM_TBM_MatchesLoadResult> MatchesListLoadedEvent		= delegate {};


	public event Action MatchesListUpdated		= delegate {};
	
	public List<UM_TBM_Match> _Matches 			= new List<UM_TBM_Match>();
	public List<UM_TBM_Invite> _Invitations 	= new List<UM_TBM_Invite>();



	public GP_TBM_Controller() {
		GooglePlayTBM.ActionMatchInitiated += HandleActionMatchInitiated;



		GooglePlayTBM.ActionMatchUpdated += HandleActionMatchUpdated;
		GooglePlayTBM.ActionMatchDataLoaded += HandleActionMatchDataLoaded;
		GooglePlayTBM.ActionMatchLeaved += HandleActionMatchLeaved;


		GooglePlayTBM.ActionMatchTurnFinished += HandleActionTurnFinished;


		GooglePlayInvitationManager.ActionInvitationReceived  += HandleActionInvitationReceived;
		GooglePlayInvitationManager.ActionInvitationAccepted  += HandleActionInvitationAccepted;
		GooglePlayInvitationManager.ActionInvitationsListLoaded += HandleActionInvitationsListLoaded;



		GooglePlayTBM.ActionMatchesResultLoaded += HandleActionMatchesResultLoaded;


		GooglePlayTBM.ActionMatchInvitationAccepted += HandleActionMatchInvitationAccepted;
		GooglePlayTBM.ActionMatchInvitationDeclined += HandleActionMatchInvitationDeclined;


		GooglePlayConnection.ActionPlayerConnected += HandleActionPlayerConnected;
	}






	//--------------------------------------
	// Get / Set
	//--------------------------------------
	
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




	//--------------------------------------
	// Methods
	//--------------------------------------


	public void SetGroup(int group) {
		GooglePlayTBM.Instance.SetVariant(group);
	}
	
	
	public void SetMask(int mask) {
		GooglePlayTBM.Instance.SetExclusiveBitMask(mask);
	}


	public void FindMatch(int minPlayers, int maxPlayers, string[] recipients = null) {
		GooglePlayTBM.Instance.CreateMatch(minPlayers - 1, maxPlayers - 1, recipients);
	}
	
	public void ShowNativeFindMatchUI(int minPlayers, int maxPlayers) {
		GooglePlayTBM.Instance.StartSelectOpponentsView(minPlayers - 1, maxPlayers - 1, true);
	}


	private int DataEventCount = 0;
	public void LoadMatchesInfo() {

		if(DataEventCount != 0) {
		 	return;
		}

		DataEventCount = 2;

		List<GP_TBM_MatchTurnStatus> statuses =  new List<GP_TBM_MatchTurnStatus>();
		statuses.Add(GP_TBM_MatchTurnStatus.MATCH_TURN_STATUS_MY_TURN);
		statuses.Add(GP_TBM_MatchTurnStatus.MATCH_TURN_STATUS_COMPLETE);
		statuses.Add(GP_TBM_MatchTurnStatus.MATCH_TURN_STATUS_INVITED);
		statuses.Add(GP_TBM_MatchTurnStatus.MATCH_TURN_STATUS_THEIR_TURN);
		
		GooglePlayTBM.Instance.LoadMatchesInfo(GP_TBM_MatchesSortOrder.SORT_ORDER_MOST_RECENT_FIRST, statuses.ToArray());
		GooglePlayInvitationManager.Instance.LoadInvitations();
	}

	private void CheckDataCounter()  {
		DataEventCount--;
		if(DataEventCount == 0) {
			UM_TBM_MatchesLoadResult result = new UM_TBM_MatchesLoadResult(new GooglePlayResult(GP_GamesStatusCodes.STATUS_OK));
			result.SetMatches(_Matches);
			result.SetInvitations(_Invitations);

			MatchesListUpdated();
			MatchesListLoadedEvent(result);
		}
	}

	public void LoadMatch(string matchId) {
		GooglePlayTBM.Instance.LoadMatchInfo(matchId);
	}
	


	public void TakeTurn(string matchId, byte[] matchData, UM_TBM_Participant nextParticipant) {
		string nextParticipantId = string.Empty;
		if(nextParticipant != null) {
			nextParticipantId = nextParticipant.Id;
		}
		GooglePlayTBM.Instance.TakeTrun(matchId, matchData, nextParticipantId);
	}



	public void QuitInTurn(string matchId, UM_TBM_Participant nextParticipant) {

		string nextParticipantId = string.Empty;
		if(nextParticipant != null) {
			nextParticipantId = nextParticipant.Id;
		}

		GooglePlayTBM.Instance.LeaveMatchDuringTurn(matchId, nextParticipantId);

	}

		
	public void QuitOutOfTurn(string matchId) {
		GooglePlayTBM.Instance.LeaveMatch(matchId);
	}

	public void RemoveMatch(string matchId) {
		GooglePlayTBM.Instance.DismissMatch(matchId);
		RemoveMatchFromTheList(matchId);
	}

	
	private const int PLACING_UNINITIALIZED = -1;
	

	public void FinishMatch(string matchId, byte[] matchData, params UM_TMB_ParticipantResult[] results) {
		List<GP_ParticipantResult> resultsList =  new List<GP_ParticipantResult>();
		
		foreach(UM_TMB_ParticipantResult r in results) {

			GP_TBM_ParticipantResult result = GP_TBM_ParticipantResult.MATCH_RESULT_UNINITIALIZED;
			switch(r.Outcome) {
			case UM_TBM_Outcome.Won:
				result = GP_TBM_ParticipantResult.MATCH_RESULT_WIN;
				break;

			case UM_TBM_Outcome.Lost:
				result = GP_TBM_ParticipantResult.MATCH_RESULT_LOSS;
				break;

			case UM_TBM_Outcome.Tied:
				result = GP_TBM_ParticipantResult.MATCH_RESULT_TIE;
				break;

			case UM_TBM_Outcome.Disconnected:
				result = GP_TBM_ParticipantResult.MATCH_RESULT_DISCONNECT;
				break;

			case UM_TBM_Outcome.None:
				result = GP_TBM_ParticipantResult.MATCH_RESULT_UNINITIALIZED;
				break;
			}

			GP_ParticipantResult res = new GP_ParticipantResult(r.ParticipantId, result, PLACING_UNINITIALIZED);
			resultsList.Add(res);
		}

		GooglePlayTBM.Instance.FinishMatch(matchId, matchData, resultsList.ToArray());
	}

	public void ConfirmhMatchFinis(string matchId) {
		GooglePlayTBM.Instance.ConfirmMatchFinish(matchId);
	}


	public void Rematch(string matchId) {
		GooglePlayTBM.Instance.Rematch(matchId);
	}



	public void AcceptInvite(UM_TBM_Invite invite){
		GooglePlayTBM.Instance.AcceptInvitation(invite.Id);
	}
	public void DeclineInvite(UM_TBM_Invite invite) {
		GooglePlayTBM.Instance.DeclineInvitation(invite.Id);
	}




	//--------------------------------------
	// Action Handlers
	//--------------------------------------

	void HandleActionMatchInitiated (GP_TBM_MatchInitiatedResult res) {
		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);

		if(res.Match != null) {
			UM_TBM_Match match =  new UM_TBM_Match(res.Match);
			result.SetMatch(match);
			UpdateMatchData(match);
		}

		MatchFoundEvent(result);

	}

	
	void HandleActionMatchDataLoaded (GP_TBM_LoadMatchResult res) {
		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);
		if(res.Match != null) {
			UM_TBM_Match match =  new UM_TBM_Match(res.Match);
			result.SetMatch(match);
			UpdateMatchData(match);
		}

		MatchLoadedEvent(result);

	}
	
	void HandleActionMatchesResultLoaded (GP_TBM_LoadMatchesResult res) {
	
		_Matches.Clear();

		if(res.IsSucceeded) {
			foreach(KeyValuePair<string, GP_TBM_Match> pair in res.LoadedMatches) {
				GP_TBM_Match match = pair.Value;
				UM_TBM_Match m =  new UM_TBM_Match(match);
				_Matches.Add(m);
			}
		}

		CheckDataCounter();
	}


	void HandleActionInvitationsListLoaded (List<GP_Invite> res) {

		_Invitations.Clear();

		foreach(GP_Invite inv in res) {
			UM_TBM_Invite invite =  new UM_TBM_Invite(inv);
			_Invitations.Add(invite);
		}
		
		CheckDataCounter();
	}


	void HandleActionMatchUpdated (GP_TBM_UpdateMatchResult res) {

		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);
		if(res.Match != null) {
			UM_TBM_Match match =  new UM_TBM_Match(res.Match);
			result.SetMatch(match);
			UpdateMatchData(match);
		}
		
		MatchUpdatedEvent(result);
	}

	void HandleActionTurnFinished (GP_TBM_UpdateMatchResult res) {
		
		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);
		if(res.Match != null) {
			UM_TBM_Match match =  new UM_TBM_Match(res.Match);
			result.SetMatch(match);
			UpdateMatchData(match);
		}
		
		TurnEndedEvent(result);
	}


	void HandleActionPlayerConnected () {
		GooglePlayTBM.Instance.RegisterMatchUpdateListener();
		GooglePlayInvitationManager.Instance.RegisterInvitationListener();
	}
	

	
	void HandleActionInvitationAccepted (GP_Invite invite) {
		Debug.Log("GP_TBM_Controller::HandleActionInvitationAccepted");
	}
	
	void HandleActionInvitationReceived (GP_Invite invite) {
		Debug.Log("GP_TBM_Controller::HandleActionInvitationReceived");
		//probably just add this invite...?
		LoadMatchesInfo();
	}


	void HandleActionMatchInvitationDeclined (string invitationId) {
		RemoveInvitationsFromTheList(invitationId);
		MatchesListUpdated();

		InvitationDeclined(invitationId);
	}
	
	void HandleActionMatchInvitationAccepted (string invitationId, GP_TBM_MatchInitiatedResult res) {
		Debug.Log("GP_TBM_Controller::HandleActionMatchInvitationAccepted");


		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);
		if(res.IsSucceeded) {
			RemoveInvitationsFromTheList(invitationId);
			UM_TBM_Match match =  new UM_TBM_Match(res.Match);
			result.SetMatch(match);

			UpdateMatchData(match);
			Debug.Log("GP_TBM_Controller::HandleActionMatchInvitationAccepted, list updated");
		}

	


		InvitationAccepted(result);

	}


	void HandleActionMatchLeaved (GP_TBM_LeaveMatchResult res) {
		if(res.IsSucceeded) {
			RemoveMatchFromTheList(res.MatchId);
		}
	}


	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	

	private void UpdateMatchData(UM_TBM_Match match) {
		bool isFound = false;


		//autofinish
		if(match.IsEnded && match.IsLocalPlayerTurn) {
			GooglePlayTBM.Instance.ConfirmMatchFinish(match.Id);
		}
		
		for (int i = 0; i < Matches.Count; i++) {
			if (Matches[i].Id.Equals(match.Id)) {
				isFound = true;
				
				Matches[i]= match;
			}
		}
		
		if (!isFound) {
			Matches.Add(match);
		}
		
		MatchesListUpdated();
	}
	
	private void RemoveMatchFromTheList(string matchId) {
		foreach(UM_TBM_Match m in _Matches) {
			if(m.Id.Equals(matchId)) {
				_Matches.Remove(m);
				MatchesListUpdated();
				return;
			}
		}
	}
	
	private void RemoveInvitationsFromTheList(string inviteId) {
		foreach(UM_TBM_Invite invite in _Invitations) {
			if(invite.Id.Equals(inviteId)) {
				_Invitations.Remove(invite);
				return;
			}
		}
	}

}
