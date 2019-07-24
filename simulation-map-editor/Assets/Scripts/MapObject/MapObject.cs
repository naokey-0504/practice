using UnityEditor;
using UnityEngine;

namespace AlterEditor
{
    public class MapObject : MonoBehaviour
    {
        [SerializeField] private Vector2 m_GridSize;
        public Vector2 gridSize
        {
            get { return m_GridSize; }
        }
    }
}