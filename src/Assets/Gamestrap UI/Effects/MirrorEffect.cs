using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Gamestrap
{
    [AddComponentMenu("UI/Gamestrap UI/Mirror")]
    public class MirrorEffect : GamestrapEffect
    {

        public float scale = 1f;

        public Vector2 offset = Vector2.zero;
        
        public float skew = 0f;

        public Color top = Color.white;

        public Color bottom = Color.white;

        public override void ModifyVerticesWrapper(List<UIVertex> vertexList)
        {
            if (!IsActive())
            {
                return;
            }
            ApplyMirror(vertexList, 0, vertexList.Count);

        }

        public void ApplyMirror(List<UIVertex> verts, int start, int end)
        {
            UIVertex vt;

            var neededCapacity = verts.Count * 2;

            if (verts.Capacity < neededCapacity)
            {
                verts.Capacity = neededCapacity;
            }
            float bottomPos = verts.Min(t => t.position.y);
            float topPos = verts.Max(t => t.position.y);
            float height = topPos - bottomPos;
            for (int i = start; i < end; i++)
            {
                vt = verts[i];
                verts.Add(vt);

                vt.color *= Color.Lerp(top, bottom, ((vt.position.y) - bottomPos) / height);

                Vector3 v = vt.position;
                v.y = bottomPos - (v.y - bottomPos) * scale;
                v.x = Mathf.Lerp(v.x, v.x + skew, (vt.position.y - bottomPos)/ height);
                v = v + (Vector3)offset;
                vt.position = v;


                verts[i] = vt;

            }
        }
    }
}
