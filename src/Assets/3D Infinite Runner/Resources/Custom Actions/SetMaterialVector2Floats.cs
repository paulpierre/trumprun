// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets a named vector in a game object's material.")]
	public class SetMaterialVector2Floats : FsmStateAction
	{
		[Tooltip("The GameObject that the material is applied to.")]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
		public FsmInt materialIndex;
		
		[Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
		public FsmMaterial material;
		
		[RequiredField]
		[Tooltip("A named float parameter in the shader.")]
		public FsmString namedVector;
		
		[RequiredField]
		[Tooltip("Set the parameter value.")]
		public FsmFloat valueX;
		
		[RequiredField]
		[Tooltip("Set the parameter value.")]
		public FsmFloat valueY;
		
		[Tooltip("Repeat every frame. Useful if the value is animated.")]
		public bool everyFrame;
		
		public override void Reset()
		{
			gameObject = null;
			materialIndex = 0;
			material = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoSetMaterialFloat();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate ()
		{
			DoSetMaterialFloat();
		}
		
		void DoSetMaterialFloat()
		{
			if (material.Value != null)
			{
				material.Value.SetVector(namedVector.Value, new Vector2(valueX.Value, valueY.Value));
				return;
			}
			
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;
			
			if (go.GetComponent<Renderer>() == null)
			{
				LogError("Missing Renderer!");
				return;
			}
			
			if (go.GetComponent<Renderer>().material == null)
			{
				LogError("Missing Material!");
				return;
			}
			
			if (materialIndex.Value == 0)
			{
				material.Value.SetVector(namedVector.Value, new Vector2(valueX.Value, valueY.Value));
			}
			else if (go.GetComponent<Renderer>().materials.Length > materialIndex.Value)
			{
				var materials = go.GetComponent<Renderer>().materials;
				material.Value.SetVector(namedVector.Value, new Vector2(valueX.Value, valueY.Value));
				go.GetComponent<Renderer>().materials = materials;                     
			}      
		}
	}
}