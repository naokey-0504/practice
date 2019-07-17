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

        public override Step GetNextStep()
        {
            return Step.LoadSimurationStage;
        }
    }
}