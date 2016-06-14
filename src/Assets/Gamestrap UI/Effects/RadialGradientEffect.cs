using UnityEngine;
using System.Collections.Generic;


namespace Gamestrap
{
    [AddComponentMenu("UI/Gamestrap UI/Directional Gradient")]
    public class RadialGradientEffect : GamestrapEffect
    {
        public Vector2 centerPosition;
        public float radius;
        public Color centerColor = Color.white;

        public void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position + (Vector3)centerPosition, 2f);
        }

        public override void ModifyVerticesWrapper(List<UIVertex> vertexList)
        {
            if (!IsActive() || vertexList.Count < 4)
            {
                return;
            }

            if (radius == 0)
            {
                radius = 1;
            }
            for (int i = 0; i < vertexList.Count; i++)
            {
                UIVertex v = vertexList[i];

                v.color *= Color.Lerp(centerColor, Color.white, Mathf.Clamp01(((Vector2)v.position - centerPosition).magnitude / radius));
                vertexList[i] = v;
            }
        }

    }
}