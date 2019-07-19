using System;
using UnityEditor;
using UnityEngine;

namespace AlterEditor.SimulationMapEditor
{
    public class PlaceMapObject : TaskBase
    {
        private Camera m_Camera;
        private Canvas m_Canvas;
        private float m_Distance = 0f;
        private Vector2 m_GridPos;
        
        public override void Init()
        {
            m_Camera = GameObject.Find("Camera").GetComponent<Camera>();
            m_Canvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
            m_Distance = m_Camera.WorldToScreenPoint(GameObject.Find("Object").transform.position).z;

#if false
            var pos = Vector2.zero;
            var screenPos = RectTransformUtility.WorldToScreenPoint(m_Camera,)
            RectTransformUtility.ScreenPointToLocalPointInRectangle();
            m_GridPos = m_Camera.ViewportToWorldPoint(new Vector3(75f, 75f, m_Distance));
#endif
        }

        public override void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var obj = Selection.activeGameObject != null
                    ? Selection.activeGameObject.GetComponent<MapObject>()
                    : null;
                if (obj != null)
                {
#if true
                    var gridParent = GameObject.Find("GridParent");
                    var grid0 = gridParent.transform.GetChild(0).GetComponent<GridBase>().transform.position;
                    var grid1 = gridParent.transform.GetChild(1).GetComponent<GridBase>().transform.position;

                    var diff = Vector3.Distance(grid0, grid1);
                    
                    var mouse = Input.mousePosition;
                    mouse.z = m_Distance;
                    mouse = m_Camera.ScreenToWorldPoint(mouse);

                    float x = (int) (mouse.x / diff) * diff;
                    float y = (int) (mouse.y / diff) * diff;
                    //obj.transform.position = mouse;
                    obj.transform.position = new Vector3(x, y, obj.transform.position.z);
#endif

#if false
                    var pos = Input.mousePosition;
                    pos.z = m_Distance;
                    pos = m_Camera.ScreenToWorldPoint(pos);
                    obj.transform.position = pos;
#endif
                }
            }
        }

        public override void Finish()
        {
            
        }

        public override bool IsFinish()
        {
            return false;
        }

        protected override Enum GetNextStep()
        {
            return TaskStep.PlaceMapObject;
        }
    }
}