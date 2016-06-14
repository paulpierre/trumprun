using UnityEngine;
using System.Collections;

public class GK_MatchUpdateTask : MonoBehaviour {


	private GK_TBM_Controller _controller = null;
	private string _matchId = string.Empty;

	public static void Create(string matchId, GK_TBM_Controller controller) {
		GK_MatchUpdateTask task = new GameObject("GK_MatchUpdateTask").AddComponent<GK_MatchUpdateTask>();
		task.LoadMatchInfo(matchId, controller);
	}




	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	public void LoadMatchInfo(string matchId, GK_TBM_Controller controller) {
		_matchId = matchId;
		_controller = controller;

		GameCenter_TBM.ActionMatchInfoLoaded += GameCenter_TBM_ActionMatchInfoLoaded;
		GameCenter_TBM.instance.LoadMatch(_matchId);
	}

	void GameCenter_TBM_ActionMatchInfoLoaded (GK_TBM_LoadMatchResult res) {
		GameCenter_TBM.ActionMatchInfoLoaded -= GameCenter_TBM_ActionMatchInfoLoaded;
		_controller.SendMatchUpdateEvent(res, res.Match);
		Destroy(gameObject);
	}
}
