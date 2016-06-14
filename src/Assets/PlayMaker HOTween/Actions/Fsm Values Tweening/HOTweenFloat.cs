// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
	
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Animate a float using HOTween.")]
	public class HOTweenFloat : PlayMakerHotweenFsmValueBase
	{
		
		[ActionSection("Values")]
	
		public FsmFloat start;
		
		public FsmFloat end;
		
		[ActionSection("Result")]
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat result;

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
				Debug.Log ("_tweenProxy!=null...");
				return;
			}
			
			FsmFloatHOTween _proxy = new FsmFloatHOTween();

			
			_proxy.FsmResult = result;
			_proxy.Current = start.Value;
			_proxy.Target = end.Value;

			_tweenProxy = _proxy;
			
			SetupTween();
			
			_proxy.start(duration.Value);
		}

	}

	
}