using System;
using UnityEngine;

namespace AlterEditor.SimulationMapEditor
{
    public class DrawGridTask : TaskBase
    {
        private const int kGridCol = 12;
        private const int kGridRow = 5;
        
        private enum Step
        {
            DrawGrid,
            CalcGridSpace,
            
            Finish,
        }
        private Step m_Step = Step.DrawGrid;
        
        public override void Init()
        {
        }

        public override void Update()
        {
            switch (m_Step)
            {
                case Step.DrawGrid:
                    m_Step = Step.CalcGridSpace;
                    SimulationMapEditorManager.Instance.DrawGrid(kGridCol, kGridRow);
                    break;
                
                case Step.CalcGridSpace:
                    m_Step = Step.Finish;
                    SimulationMapEditorManager.Instance.CalcGridSpace(kGridCol);
                    break;
                
                case Step.Finish:
                    break;
            }
        }

        public override void Finish()
        {
            
        }

        public override bool IsFinish()
        {
            return m_Step == Step.Finish;
        }

        protected override Enum GetNextStep()
        {
            return TaskStep.PlaceMapObject;
        }
    }
}