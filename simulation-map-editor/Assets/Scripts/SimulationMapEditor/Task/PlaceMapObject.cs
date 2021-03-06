using System;
using UnityEditor;
using UnityEditor.Experimental.U2D;
using UnityEngine;

namespace AlterEditor.SimulationMapEditor
{
    public class PlaceMapObject : TaskBase
    {
        private Camera m_Camera;
        private Canvas m_Canvas;
        private float m_Distance = 0f;
        private Vector2 m_GridSize;

        public override void Init()
        {
            m_Camera = GameObject.Find("Camera").GetComponent<Camera>();
            m_Canvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
            m_Distance = m_Camera.WorldToScreenPoint(GameObject.Find("Object").transform.position).z;
            m_GridSize = SimulationMapEditorManager.Instance.GetGridSize();
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
                    if (!SimulationMapEditorManager.Instance.isGridMode)
                    {
                        //マウスカーソルの座標に合わせて
                        var pos = Input.mousePosition;
                        pos.z = m_Distance;
                        pos = m_Camera.ScreenToWorldPoint(pos);
                        obj.transform.position = pos;
                    }
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