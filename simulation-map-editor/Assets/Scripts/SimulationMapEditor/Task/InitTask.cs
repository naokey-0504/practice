using System;
using AlterEditor.QuestEditor;

namespace AlterEditor.SimulationMapEditor
{
    public class InitTask : TaskBase
    {
        public override void Init()
        {
            SimulationMapEditorManager.Instance.Init();
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
            return TaskStep.DrawGrid;
        }
    }
}