// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Kill ALL tweens")]
	public class HotweenKill: PlayMakerHOTweenAction
	{
		
		[Tooltip("The total number of killed Tweeners/Sequences.")]
		[UIHint(UIHint.Variable)]
		public FsmInt killedCount;
		
				
		public override void Reset()
		{
			killedCount = null;
		}
		
		public override void OnEnter()
		{
			killedCount.Value = HOTween.Kill();
			
			Finish();	
		}
	
	}
}