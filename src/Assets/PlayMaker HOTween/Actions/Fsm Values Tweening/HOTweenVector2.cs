// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
	
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Animate a Vector2 using HOTween.")]
	public class HOTweenVector2 : PlayMakerHotweenFsmValueBase
	{
		
		[ActionSection("Values")]
	
		public FsmVector2 start;
		
		public FsmVector2 end;
		
		[ActionSection("Result")]
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector2 result;

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
			
			FsmVector2HOTween _proxy = new FsmVector2HOTween();
			
			_proxy.FsmResult = result;
			_proxy.Current = start.Value;
			_proxy.Target = end.Value;

			_tweenProxy = _proxy;
			
			SetupTween();
			
			_proxy.start(duration.Value);
		}

	}

	
}