using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Gamestrap
{
#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1
    public class GamestrapEffect : BaseVertexEffect
#else
    public abstract class GamestrapEffect : BaseMeshEffect
#endif
    {

        //IMPORTANT: If you are using 5.2 and a patch 5.2.1p1 or above then delete from HERE
#if UNITY_5_2_0 || UNITY_5_2_1
        public override void ModifyMesh(Mesh mesh)
        {
            if (!this.IsActive())
                return;

            List<UIVertex> list = new List<UIVertex>();
            using (VertexHelper vertexHelper = new VertexHelper(mesh))
            {
                vertexHelper.GetUIVertexStream(list);
            }

            ModifyVerticesWrapper(list);  // calls the old ModifyVertices which was used on pre 5.2

            using (VertexHelper vertexHelper2 = new VertexHelper())
            {
                vertexHelper2.AddUIVertexTriangleStream(list);
                vertexHelper2.FillMesh(mesh);
            }
        }
#elif !(UNITY_4_6 || UNITY_5_0 || UNITY_5_1)
        //        // ------------------------------------------------------------------>>>>>>  TO HERE!! And...
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!this.IsActive())
                return;

            List<UIVertex> vertexList = new List<UIVertex>();
            vh.GetUIVertexStream(vertexList);

            ModifyVerticesWrapper(vertexList);

            vh.Clear();
            vh.AddUIVertexTriangleStream(vertexList);
        }
#endif // HERE (just this line)

#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1
        public override void ModifyVertices(List<UIVertex> vertexList){
            ModifyVerticesWrapper(vertexList);
        }
#endif

        public abstract void ModifyVerticesWrapper(List<UIVertex> vertexList);

        public void SetVertexColor(List<UIVertex> vertexList, int index, Color color)
        {
            UIVertex v = vertexList[index];
            v.color = color;
            vertexList[index] = v;
        }
    }
}