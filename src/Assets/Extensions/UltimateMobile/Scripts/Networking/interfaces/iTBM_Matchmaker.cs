using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface iTBM_Matchmaker {

	event Action<UM_TBM_MatchResult> MatchFoundEvent;
	event Action<UM_TBM_MatchResult> MatchLoadedEvent;

	event Action<UM_TBM_MatchResult> InvitationAccepted;
	event Action<string> InvitationDeclined;

	event Action<UM_TBM_MatchResult> MatchUpdatedEvent;
	event Action<UM_TBM_MatchResult> TurnEndedEvent;
	

	event Action<UM_TBM_MatchesLoadResult> MatchesListLoadedEvent;

	event Action MatchesListUpdated;


	/// <summary>
	/// Avaliable matches list
	/// </summary>
	List<UM_TBM_Match> Matches { get;}

	/// <summary>
	/// Pending invitations list
	/// </summary>
	List<UM_TBM_Invite> Invitations { get;}


	/// <summary>
	/// If your game sets the playerGroup property, only players whose requests share the same playerGroup value 
	/// are automatched Setting the playerGroup property to 0 allows the player to be matched into 
	/// any waiting match. Set the playerGroup property to a nonzero number to match the player only with players 
	/// whose match request shares the same player group number. The value of a player group is arbitrary.
	/// </summary>
	void SetGroup(int group);


	/// <summary>
	/// A mask that specifies the role that the local player would like to play in the game.
	/// 
	/// If this value is 0xFFFFFFFF (the default), this property is ignored. If the value is nonzero, then automatching 
	/// uses the value as a mask that restricts the role the player can play in the group. Automatching with player 
	/// attributes matches new players into the game so that the bitwise OR of the masks of all the players in the 
	/// resulting match equals 0xFFFFFFFF.
	/// </summary>
	void SetMask(int mask);


	/// <summary>
	/// Programmatically searches for a new match to join.
	/// 
	/// This method may either create a new match or it may place the player into an existing match that needs a new 
	/// player to advance the match further. Regardless of how the player is placed in the match, the local player is 
	/// always the current participant in the returned match. Your game should immediately display the match in its 
	/// user interface and allow the player to take a turn.
	/// <param name="minPlayers">he minimum number of players that may join the match, must be at least 2.</param>
	/// <param name="maxPlayers">The maximum number of players that may join the match.</param>
	/// <param name="recipients">A list of player identifiers for players to invite to the match.</param>
	/// </summary>
	void FindMatch(int minPlayers, int maxPlayers, string[] recipients = null);

	/// <summary>
	/// Displays a user interface that allows players to manage the turn-based matches that they are participating in.
	/// 
	/// Note, that user can also selcet one of teh existing matches instead of creating new one.
	/// <param name="minPlayers">he minimum number of players that may join the match, must be at least 2.</param>
	/// <param name="maxPlayers">The maximum number of players that may join the match.</param>
	/// </summary>
	void ShowNativeFindMatchUI(int minPlayers, int maxPlayers);



	/// <summary>
	/// Loads the turn-based matches involving the local player and creates a match object for each match.
	/// 
	/// When this method is called, it creates a new background task to handle the request. The method then returns
	/// control to your game. Later, when the task is complete, it calls ActionMatchesInfoLoaded handler. The  
	/// completion handler is always called on the main thread.
	/// </summary>
	void LoadMatchesInfo();

	/// <summary>
	/// Loads a specific match.
	/// 
	/// When this method is called, it creates a new background task to handle the request. The method then returns
	/// control to your game. Later, when the task is complete, it calls ActionMatchInfoLoaded handler. The  
	/// completion handler is always called on the main thread.
	/// <param name="matchId">The identifier for the turn-based match.</param>
	/// </summary>
	void LoadMatch(string matchId);

	/// <summary>
	/// Programmatically removes a match from the Server and local storage. 
	/// It will not change the state of the match for the other participants, 
	/// but removed matches will never be shown to the dismissing play
	/// 
	/// Even after a player’s participation in a match ends, the data associated with the match continues to be stored 
	/// Storing the data allows the player to continue to watch the match’s progress, 
	/// or even see the final state of the match when it ends. However, players may also want to delete 
	/// matches that they have finished playing. If you choose not to use the standard matchmaker user interface, 
	/// your game should offer the ability to delete a finished match. When a player chooses to 
	/// delete a match, call this method. It is a programming error to call this method on a match 
	/// that has the local player as an active participant.
	/// 
	/// When the task completes, the match is no longer visible to the local player whose device made the call. Other 
	/// players involved in the match still see the match.
	/// <param name="matchId">The identifier for the turn-based match.</param>
	/// </summary>
	void RemoveMatch(string matchId);


	/// <summary>
	/// Updates the data stored for the current match.
	/// 
	/// When this method is called, it creates a new background task to handle the request. The method then returns
	/// control to your game. Later, when the task is complete, it calls ActionMatchInfoLoaded handler. The  
	/// completion handler is always called on the main thread.
	/// <param name="matchId">The identifier for the turn-based match.</param>
	/// <param name="matchData">A serialized blob of data reflecting the game-specific state for the match. Do not pass null as an argument</param>
	/// <param name="nextParticipant">The next player in the match who needs to take an action. It must be one of the object’s stored in the match’s Participants property.</param>
	/// </summary>
	void TakeTurn(string matchId, byte[] matchData, UM_TBM_Participant nextParticipant);


	/// <summary>
	/// Resigns the current player from the match without ending the match.
	/// 
	/// Your game calls this method on an instance of your game that is processing the current player’s turn, but that 
	/// player has left the match. For example, the player may have willingly resigned from the match or that player 
	/// may have been eliminated by the other players (based on your game’s internal logic).
	/// 
	/// If the next player to act does not take their turn in the specified interval, the next player in the array receives a 
	/// notification to act. This process continues until a player takes a turn or the last player in the list is notified.
	/// 
	/// <param name="matchId">The identifier for the turn-based match.</param>
	/// <param name="nextParticipant">The next player in the match who needs to take an action. It must be one of the object’s stored in the match’s Participants property.</param>
	/// </summary>
	void QuitInTurn(string matchId, UM_TBM_Participant nextParticipant);


	/// <summary>
	/// Resigns the player from the match when that player is not the current player. This action does not end the match
	/// 
	/// If the local player decided they wanted to resign from the match but is not the current participant in the match, 
	/// your game calls this method.
	/// 
	/// When this method is called, it creates a new background task to handle the request. The method then returns 
	/// control to your game. Later, when the task is complete, Game Kit calls your completion handler. The 
	/// completion handler is always called on the main thread.
	/// 
	/// <param name="matchId">The identifier for the turn-based match.</param>
	/// </summary>
 	void QuitOutOfTurn(string matchId);





	/// <summary>
	/// Create a new turn-based match with the same participants as an existing match.
	/// 
	/// When this method is called, it creates a new background task to handle the request. The method then returns 
	/// control to your game. Later, when the task is complete, Game Kit calls your completion handler. The 
	/// completion handler is always called on the main thread.
	/// 
	/// <param name="matchId">The identifier for the turn-based match.</param>
	/// </summary>
	void Rematch(string matchId);


	/// <summary>
	/// Ends the match. 
	/// 
	/// Calling this method ends the match for all players. This method may only be called by the current participant. 
	/// When this method is called, it creates a new background task to handle the request. The method then returns 
	/// control to your game. Later, when the task is complete, Game Kit calls your completion handler. The 
	/// completion handler is always called on the main thread.
	/// 
	/// 
	/// <param name="matchId">The identifier for the turn-based match.</param>
	/// <param name="matchData">A data reflecting the end state for the match. Do not pass nil as an argument.</param>
	/// <param name="results">List of UM_TMB_ParticipantResult objects for this match. 
	/// The client which calls finishMatch is responsible for reporting the results for all appropriate participants in the match. 
	/// Not every participant is required to have a result, but providing results for participants 
	/// who are not in the match is an error.</param>
	/// </summary>
	void FinishMatch(string matchId, byte[] matchData, params UM_TMB_ParticipantResult[] results);


	/*

	/// <summary>
	/// Asynchronously load the list of invitations for the current game. 
	/// Invitations are returned sorted by most recent first.
	/// </summary>
	void LoadInvitations();

*/


	/// <summary>
	/// Programmatically accept an invitation to a turn-based match. 
	/// <param name="invite">Received invitation object</param>
	/// </summary>
	void AcceptInvite(UM_TBM_Invite invite);

	/// <summary>
	/// Programmatically decline an invitation to a turn-based match.
	/// <param name="invite">Received invitation object</param>
	/// </summary>
	void DeclineInvite(UM_TBM_Invite invite);







}