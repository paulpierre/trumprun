using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GK_TBM_Controller : iTBM_Matchmaker {

	public event Action<UM_TBM_MatchResult> MatchFoundEvent	 	= delegate {};
	public event Action<UM_TBM_MatchResult> MatchLoadedEvent	= delegate {};

	public event Action<UM_TBM_MatchResult> InvitationAccepted	= delegate {};
	public event Action<string> InvitationDeclined				= delegate {};


	public event Action<UM_TBM_MatchResult> TurnEndedEvent		= delegate {};
	public event Action<UM_TBM_MatchResult> MatchUpdatedEvent	= delegate {};


	public event Action<UM_TBM_MatchesLoadResult> MatchesListLoadedEvent		= delegate {};
	public event Action<List<UM_TBM_Invite>> InvitationsListLoadedEvent		= delegate {};



	public event Action MatchesListUpdated		= delegate {};

	public List<UM_TBM_Match> _Matches 			= new List<UM_TBM_Match>();
	public List<UM_TBM_Invite> _Invitations 	= new List<UM_TBM_Invite>();



	public GK_TBM_Controller() {
		GameCenter_TBM.ActionMatchFound += HandleActionMatchFound;
		GameCenter_TBM.ActionRematched  += HandleActionRematched;

		GameCenter_TBM.ActionMatchQuit += HandleActionMatchQuit;
		GameCenter_TBM.ActionTrunEnded += HandleActionTrunEnded;
		GameCenter_TBM.ActionMacthEnded += HandleActionMacthEnded;



		GameCenter_TBM.ActionPlayerQuitForMatch += HandleActionPlayerQuitForMatch;
		GameCenter_TBM.ActionTrunReceived += HandleActionTrunReceived;



		GameCenter_TBM.ActionMatchInfoLoaded += HandleActionMatchInfoLoaded;
		GameCenter_TBM.ActionMatchesInfoLoaded += HandleActionMatchesInfoLoaded;

		GameCenter_TBM.ActionMatchRemoved += HandleActionMatchRemoved;
		GameCenter_TBM.ActionMatchInvitationAccepted += HandleActionMatchInvitationAccepted;
		GameCenter_TBM.ActionMatchInvitationDeclined += HandleActionMatchInvitationDeclined;

	}



	void HandleActionMatchInvitationDeclined (GK_TBM_MatchRemovedResult res) {
		RemoveInvitationsFromTheList(res.MatchId);
		MatchesListUpdated();

		InvitationDeclined(res.MatchId);
	}

	void HandleActionMatchInvitationAccepted (GK_TBM_MatchInitResult res) {

		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);

		if(res.IsSucceeded) {
			RemoveInvitationsFromTheList(res.Match.Id);

			UM_TBM_Match match =  new UM_TBM_Match(res.Match);
			result.SetMatch(match);

			UpdateMatchData(match);
		}
		
		InvitationAccepted(result);

	}

	void HandleActionMatchRemoved (GK_TBM_MatchRemovedResult res) {
		if(res.IsSucceeded) {
			RemoveMatchFromTheList(res.MatchId);
		}

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

	public void SetGroup(int playerGroup) {
		GameCenter_TBM.Instance.SetPlayerGroup(playerGroup);
	}
	
	public void SetMask(int mask) {
		GameCenter_TBM.Instance.SetPlayerAttributes(mask);
	}





	public void FindMatch(int minPlayers, int maxPlayers, string[] recipients = null) {
		GameCenter_TBM.Instance.FindMatch(minPlayers, maxPlayers, string.Empty, recipients);
	}

	public void ShowNativeFindMatchUI(int minPlayers, int maxPlayers) {
		GameCenter_TBM.Instance.FindMatchWithNativeUI(minPlayers, maxPlayers);
	}


	public void LoadMatchesInfo() {
		GameCenter_TBM.Instance.LoadMatchesInfo();
	}
	
	public void LoadMatch(string matchId) {
		GameCenter_TBM.Instance.LoadMatch(matchId);
	}

	public void RemoveMatch(string matchId) {
		GameCenter_TBM.Instance.RemoveMatch(matchId);
	}

	public void TakeTurn(string matchId, byte[] matchData, UM_TBM_Participant nextParticipant) {
		GameCenter_TBM.Instance.EndTurn(matchId, matchData, nextParticipant.Id);
	}

	public void FinishMatch(string matchId, byte[] matchData, params UM_TMB_ParticipantResult[] results) {

	

		foreach(UM_TMB_ParticipantResult r in results) {

			int resultID = (int) GK_TurnBasedMatchOutcome.Quit;

			switch(r.Outcome) {
			case UM_TBM_Outcome.Won:
				resultID = (int) GK_TurnBasedMatchOutcome.Won;
				break;

			case UM_TBM_Outcome.Lost:
				resultID =  (int) GK_TurnBasedMatchOutcome.Lost;
				break;

			case UM_TBM_Outcome.Tied:
				resultID = (int) GK_TurnBasedMatchOutcome.Tied;
				break;

			case UM_TBM_Outcome.Disconnected:
				resultID = (int) GK_TurnBasedMatchOutcome.Quit;
				break;

			case UM_TBM_Outcome.None:
				resultID = (int) GK_TurnBasedMatchOutcome.None;
				break;
			}
				

			GameCenter_TBM.Instance.UpdateParticipantOutcome(matchId, resultID, r.ParticipantId);
		}
			

		GameCenter_TBM.Instance.EndMatch(matchId, matchData);

	}

	public void Rematch(string matchId) {
		GameCenter_TBM.Instance.Rematch(matchId);
	}
	

	public void AcceptInvite(UM_TBM_Invite invite) {
		GameCenter_TBM.Instance.AcceptInvite(invite.Id);
	}

	public void DeclineInvite(UM_TBM_Invite invite) {
		GameCenter_TBM.Instance.DeclineInvite(invite.Id);
	}





	public void QuitInTurn(string matchId, UM_TBM_Participant nextParticipant) {
		GameCenter_TBM.Instance.QuitInTurn(matchId, GK_TurnBasedMatchOutcome.Quit, nextParticipant.Id, new byte[0]);
	}


	public void QuitOutOfTurn(string matchId) {
		GameCenter_TBM.Instance.QuitOutOfTurn(matchId, GK_TurnBasedMatchOutcome.Quit);
	}


	//--------------------------------------
	// Private Methods
	//--------------------------------------


	public void SendMatchUpdateEvent(ISN_Result res, GK_TBM_Match match) {
		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);
		if(match != null) {
			UM_TBM_Match m =  new UM_TBM_Match(match);
			result.SetMatch(m);
			UpdateMatchData(m);
		}

		MatchUpdatedEvent(result);
	}

	private void UpdateMatchData(UM_TBM_Match match) {
		bool isFound = false;
		
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

	//--------------------------------------
	// Action Handlers
	//--------------------------------------
	
	
	void HandleActionMatchFound (GK_TBM_MatchInitResult res) {
	

		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);
		if(res.Match != null) {
			UM_TBM_Match match =  new UM_TBM_Match(res.Match);
			result.SetMatch(match);

			UpdateMatchData(match);
		}



		MatchFoundEvent(result);
	}

	void HandleActionRematched (GK_TBM_RematchResult res) {
		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);
		if(res.Match != null) {
			UM_TBM_Match match =  new UM_TBM_Match(res.Match);
			result.SetMatch(match);
			
			UpdateMatchData(match);
		}
		
		
		MatchFoundEvent(result);
	}


	//remote
	void HandleActionTrunReceived (GK_TBM_MatchTurnResult res) {
		Debug.Log("GK_TBM_Controller::HandleActionTrunReceived");

		SendMatchUpdateEvent(res, res.Match);
	}
	
	void HandleActionPlayerQuitForMatch (GK_TBM_Match match) {
		Debug.Log("GK_TBM_Controller::HandleActionPlayerQuitForMatch");

		ISN_Result r = new ISN_Result(true);

		SendMatchUpdateEvent(r, match);
	}


	//local
	void HandleActionMatchQuit (GK_TBM_MatchQuitResult res) {
		Debug.Log("GK_TBM_Controller::HandleActionMatchQuit");
		//GK_MatchUpdateTask.Create(res.MatchId, this);
		LoadMatchesInfo();
	}

	private void HandleActionMacthEnded(GK_TBM_MatchEndResult res) {
		SendMatchUpdateEvent(res, res.Match);
	}



	void HandleActionMatchInfoLoaded (GK_TBM_LoadMatchResult res){

		Debug.Log("GK_TBM_Controller::HandleActionMatchInfoLoaded");

		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);
		
		if(res.Match != null) {
			UM_TBM_Match match =  new UM_TBM_Match(res.Match);
			UpdateMatchData(match);
			result.SetMatch(match);
		}
		
		MatchLoadedEvent(result);
	}



	void HandleActionTrunEnded (GK_TBM_EndTrunResult res) {
		UM_TBM_MatchResult result  = new UM_TBM_MatchResult(res);
		
		if(res.Match != null) {
			UM_TBM_Match match =  new UM_TBM_Match(res.Match);
			UpdateMatchData(match);
			result.SetMatch(match);
		}
		
		TurnEndedEvent(result);
	}


	void HandleActionMatchesInfoLoaded (GK_TBM_LoadMatchesResult res){
		UM_TBM_MatchesLoadResult result  = new UM_TBM_MatchesLoadResult(res);

		if(res.IsSucceeded) {

			_Matches.Clear();
			_Invitations.Clear();

			foreach( KeyValuePair<string, GK_TBM_Match> pair  in res.LoadedMatches) {
				GK_TBM_Match match = pair.Value;

				if(match.LocalParticipant == null) {
					continue;
				}

				if(match.LocalParticipant.Status == GK_TurnBasedParticipantStatus.Invited) {
					UM_TBM_Invite invite =  new UM_TBM_Invite(match);
					_Invitations.Add(invite);
				} else {
					UM_TBM_Match m =  new UM_TBM_Match(match);
					_Matches.Add(m);
				}

				result.SetMatches(_Matches);
				result.SetInvitations(_Invitations);
			}
			
		}

		MatchesListUpdated();
		MatchesListLoadedEvent(result);
	}

}
