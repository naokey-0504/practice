using System;

namespace AlterEditor.QuestEditor
{
    public class InitTask : TaskBase
    {
        public override void Init()
        {
            QuestEditorManager.Instance.Init();
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
            return TaskStep.LoadSimurationStage;
        }
    }
}