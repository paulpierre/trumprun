// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Plays ALL tweens of a given ID")]
	public class HotweenPlay: PlayMakerHOTweenAction
	{
		public enum PlayType {play,playForward,playBackward};
		
		public enum OnExitAction {Nothing,PauseOnExit,RewindOnExit};
		
		public FsmString tweenID;
		public PlayType playType;
		
		public OnExitAction onExit;
		
		public override void Reset()
		{
			tweenID = null;
			playType = PlayType.play;
			onExit = OnExitAction.Nothing;
		}
		public override void OnExit()
		{
			if (onExit == OnExitAction.Nothing )
			{
				return;
			}
			
			if (onExit == OnExitAction.RewindOnExit)
			{
				HOTween.Rewind(tweenID.Value);
			}else if (onExit == OnExitAction.PauseOnExit)
			{
				HOTween.Pause(tweenID.Value);
			}
		}
		
		public override void OnEnter()
		{
			if  (playType == PlayType.playForward)
			{
				HOTween.PlayForward(tweenID.Value);
			}else if  (playType == PlayType.playBackward)
			{
				HOTween.PlayBackwards(tweenID.Value);
			}else{
				HOTween.Play(tweenID.Value);
			}
			
			Finish();	
		}
	
	}
}