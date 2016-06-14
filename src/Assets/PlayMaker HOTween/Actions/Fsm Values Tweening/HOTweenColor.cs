// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
	
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Animate a color using HOTween.")]
	public class HOTweenColor : PlayMakerHotweenFsmValueBase
	{
		
		[ActionSection("Values")]
	
		public FsmColor start;
		
		public FsmColor end;
		
		[ActionSection("Result")]
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmColor result;

		public override void Reset()
		{
			base.Reset ();
			start = null;
			end = null;
			result = null;
		}
		
		public override void OnEnter()
		{
			if (_tweenProxy!=null)
			{
				return;
			}
			
			FsmColorHOTween _proxy = new FsmColorHOTween();
			
			if (!start.IsNone)
			{
				result.Value = start.Value;
			}
			
			_proxy.FsmResult = result;
			
			
			_proxy.Target = end.Value;

			_tweenProxy = _proxy;
			
			SetupTween();
			
			_proxy.start(duration.Value);
		}

	}

	
}