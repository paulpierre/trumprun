// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Stops a predefine HOTween tween by its ID.")]
	public class HotweenStopById: PlayMakerHOTweenAction
	{
		
		public FsmString tweenID;

		private IHOTweenComponent tween;

		public FsmEvent failed;
		
		public override void Reset()
		{
			tweenID = null;
		}
		
		public override void OnEnter()
		{
			List<IHOTweenComponent> tweens = new List<IHOTweenComponent>();
			
			tweens = HOTween.GetTweensById(tweenID.Value,false);
			
			if (tweens.Count ==0)
			{
				LogWarning("HOTween "+tweenID.Value+" not found");
				Fsm.Event(failed);
				Finish();
				return;
			}else{
				tween = tweens[0];
				tween.Kill();
				Finish();
			}

		}
	
	}
}