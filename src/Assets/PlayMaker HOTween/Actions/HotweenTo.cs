// (c) Copyright HutongGames, LLC 2010-2012. All rights reserved.

using UnityEngine;
using Holoville.HOTween;
	
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("HOTween")]
	[Tooltip("Animate an object using HOTween.")]
	public class HotweenTo : PlayMakerHOTweenAction
	{
		
		[RequiredField]
		[Tooltip("The object containing the properties to tween.")]
		public FsmObject target;
		
		[Tooltip("The name of the property or field to tween (must be public and non-static).")]
		public FsmString propName;
		
		[Tooltip("The seconds of duration of the tween.")]
		public FsmFloat duration;
		
				
		[Tooltip("If true, considers the endValue as relative instead than absolute (meaning it will animate BY instead than animate TO.")]
		public FsmBool isRelative;
		
		[Tooltip("The value the property/field should reach with the tween (or should be moved by, if isRelative is set to true.\n" +
		 	"Fill the appropriate data type in the 'Value' section below")]
		public HOTweenableFsmVariableEnum valueType;
		
		[ActionSection("Value")]
		
		public FsmInt IntData;
		public FsmFloat FloatData;
		public FsmString StringData;
		public FsmBool BoolData;
		public FsmVector2 Vector2Data;
		public FsmVector3 Vector3Data;
		public FsmGameObject GameObjectData;
		public FsmRect RectData;
		public FsmQuaternion QuaternionData;
		public FsmColor ColorData;
		public FsmMaterial MaterialData;
		public FsmTexture TextureData;
		public FsmObject setObjectData;
		
		
		public override void Reset()
		{
			target = null;
			propName = null;
			duration = null;
			isRelative = false;
			valueType = HOTweenableFsmVariableEnum.FsmVector3;
		}

		public override void OnEnter()
		{
			
			object _endValue = null;
			
			switch (valueType){
				case (HOTweenableFsmVariableEnum.FsmColor):
					_endValue = ColorData.Value;
					break;
				case (HOTweenableFsmVariableEnum.FsmFloat):
					_endValue = FloatData.Value;
					break;
				case (HOTweenableFsmVariableEnum.FsmInt):
					_endValue = IntData.Value;
					break;
				case (HOTweenableFsmVariableEnum.FsmQuaternion):
					_endValue = QuaternionData.Value;
					break;
				case (HOTweenableFsmVariableEnum.FsmRect):
					_endValue = RectData.Value;
					break;
				case (HOTweenableFsmVariableEnum.FsmString):
					_endValue = StringData.Value;
					break;
				case (HOTweenableFsmVariableEnum.FsmVector2):
					_endValue = Vector2Data.Value;
					break;
				case (HOTweenableFsmVariableEnum.FsmVector3):
					_endValue = Vector3Data.Value;
					break;
				default:
					//ERROR
					break;
				
			}
			
			HOTween.To(target.Value, duration.Value, propName.Value, _endValue,isRelative.Value);
			
			Finish();		
		}


	}
}