// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
	
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Animate an int using HOTween.")]
	public class HOTweenInt : PlayMakerHotweenFsmValueBase
	{
		
		[ActionSection("Values")]
	
		public FsmInt start;
		
		public FsmInt end;
		
		[ActionSection("Result")]
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt result;

		public override void Reset()
		{
			base.Reset();
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
			
			FsmIntHOTween _proxy = new FsmIntHOTween();
			
			_proxy.FsmResult = result;
			_proxy.Current = start.Value;
			_proxy.Target = end.Value;

			_tweenProxy = _proxy;
			
			SetupTween();
			
			_proxy.start(duration.Value);
		}

	}

	
}