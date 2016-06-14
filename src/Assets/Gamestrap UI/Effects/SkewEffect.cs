using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Gamestrap
{
    [AddComponentMenu("UI/Gamestrap UI/Skew")]
    public class SkewEffect : GamestrapEffect
    {
        public float skew = 0f;
        public float perspective = 0f;

        public override void ModifyVerticesWrapper(List<UIVertex> vertexList)
        {
            if (skew != 0)
                ApplySkew(vertexList, 0, vertexList.Count);
            if (perspective != 0)
                ApplyPerspective(vertexList, 0, vertexList.Count);
        }

        public void ApplySkew(List<UIVertex> verts, int start, int end)
        {
            UIVertex vt;
            float bottomPos = verts.Min(t => t.position.y);
            float topPos = verts.Max(t => t.position.y);
            float height = topPos - bottomPos;
            for (int i = start; i < end; i++)
            {
                vt = verts[i];
                Vector3 v = vt.position;
                v.x += Mathf.Lerp(-skew, skew, (vt.position.y - bottomPos) / height);
                vt.position = v;

                verts[i] = vt;
            }
        }

        public void ApplyPerspective(List<UIVertex> verts, int start, int end)
        {
            UIVertex vt;
            float bottomPos = verts.Min(t => t.position.y);
            float topPos = verts.Max(t => t.position.y);
            float height = topPos - bottomPos;

            float leftPos = verts.Min(t => t.position.x);
            float rightPos = verts.Max(t => t.position.x);
            float middleX = leftPos + (rightPos - leftPos) / 2f;
            for (int i = start; i < end; i++)
            {
                vt = verts[i];
                Vector3 v = vt.position;
                float percentage = Mathf.Lerp(perspective, 1, (vt.position.y - bottomPos) / height);
                float offset = (v.x - middleX) * percentage;
                v.x = middleX + offset;
                vt.position = v;
                verts[i] = vt;
            }
        }
    }
}
