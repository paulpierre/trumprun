// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Pauses ALL tweens of a given ID")]
	public class HotweenPause: PlayMakerHOTweenAction
	{
		
		public FsmString tweenID;
		
		public override void Reset()
		{
			tweenID = null;
		}
		
		public override void OnEnter()
		{
			HOTween.Pause(tweenID.Value);
			Finish();	
		}
	
	}
}