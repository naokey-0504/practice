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
                    if (SimulationMapEditorManager.Instance.isGridMode)
                    {
                        //マス目に合わせて吸着するように
                        var sizeDelta = obj.GetComponent<MapObject>().gridSize;
                        var scale = obj.transform.localScale;
                        var gridSize = new Vector2(sizeDelta.x * scale.x, sizeDelta.y * scale.y);

                        var point = Input.mousePosition;
                        var offsetX = (int) gridSize.x % 2 == 0 ? m_GridSize.x / 2f : 0f;
                        var offsetY = (int) gridSize.y % 2 == 0 ? m_GridSize.y / 2f : 0f;
                        point.x = (int) (point.x / m_GridSize.x) * m_GridSize.x + offsetX;
                        point.y = (int) (point.y / m_GridSize.y) * m_GridSize.y + offsetY;
                        point.z = m_Distance;
                        obj.transform.position = m_Camera.ScreenToWorldPoint(point) + new Vector3(0.1f, 0.1f, 0f);
                    } else {
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