// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
	
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Animate a Quaternion using HOTween.")]
	public class HOTweenQuaternion : PlayMakerHotweenFsmValueBase
	{
		
		[ActionSection("Values")]
	
		public FsmQuaternion start;
		
		public FsmQuaternion end;
		
		[ActionSection("Result")]
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion result;

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
			
			FsmQuaternionHOTween _proxy = new FsmQuaternionHOTween();
			
			_proxy.FsmResult = result;
			_proxy.Current = start.Value;
			_proxy.Target = end.Value;

			_tweenProxy = _proxy;
			
			SetupTween();
			
			_proxy.start(duration.Value);
		}

	}

	
}