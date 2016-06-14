// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Kills all the Tweeners/Sequences with the given ID, and returns the total number of killed Tweeners/Sequences.")]
	public class HotweenKillById: PlayMakerHOTweenAction
	{
		[Tooltip("the given ID to kill")]
		public FsmString tweenID;
		
		[Tooltip("The total number of killed Tweeners/Sequences.")]
		[UIHint(UIHint.Variable)]
		public FsmInt killedCount;
		
		public override void Reset()
		{
			tweenID = null;
			killedCount = null;
		}
		
		public override void OnEnter()
		{
			killedCount = HOTween.Kill(tweenID.Value);
			Finish();	
		}
	
	}
}