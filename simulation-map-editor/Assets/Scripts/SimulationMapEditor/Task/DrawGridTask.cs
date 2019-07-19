using System;

namespace AlterEditor.SimulationMapEditor
{
    public class DrawGridTask : TaskBase
    {
        public override void Init()
        {
            SimulationMapEditorManager.Instance.DrawGrid(12, 5);
        }

        public override void Update()
        {
            
        }

        public override void Finish()
        {
            
        }

        public override bool IsFinish()
        {
            return true;
        }

        protected override Enum GetNextStep()
        {
            return TaskStep.PlaceMapObject;
        }
    }
}