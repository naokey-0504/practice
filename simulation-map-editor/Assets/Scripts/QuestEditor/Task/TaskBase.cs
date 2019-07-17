namespace AlterEditor.QuestEditor
{
    public interface ITask
    {
        void Init();
        void Update();
        void Finish();
        bool IsFinish();

        TaskBase.Step GetNextStep();
    }
    
    public abstract class TaskBase : ITask
    {
        public enum Step
        {
            None = 0,
            
            Init,
            LoadSimurationStage,
            DrawGrid,
        }
        
        public abstract void Init();
        public abstract void Update();
        public abstract void Finish();
        public abstract bool IsFinish();
        public abstract TaskBase.Step GetNextStep();
    }
}