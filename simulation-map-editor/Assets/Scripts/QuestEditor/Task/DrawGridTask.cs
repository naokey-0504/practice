using System;

namespace AlterEditor.QuestEditor
{
    public class DrawGridTask : TaskBase
    {
        public override void Init()
        {
            QuestEditorManager.Instance.DrawGrid(12, 5);
        }

        public override void Update()
        {
            
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
            return TaskStep.DrawGrid;
        }
    }
}