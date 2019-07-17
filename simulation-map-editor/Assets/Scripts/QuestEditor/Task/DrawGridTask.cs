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

        public override Step GetNextStep()
        {
            return TaskBase.Step.DrawGrid;
        }
    }
}