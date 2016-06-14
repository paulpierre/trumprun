// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;

namespace HutongGames.PlayMaker.Actions
{

	[Tooltip("HOTween base action - don't use!")]
	public abstract class PlayMakerHOTweenAction : FsmStateAction
	{
		public enum HOTweenDirection {To,From};
		
		public enum HOTweenableFsmVariableEnum{
		 FsmInt,FsmFloat,FsmString,FsmVector2,FsmVector3,FsmRect,FsmQuaternion,FsmColor
		}

		
	}
}