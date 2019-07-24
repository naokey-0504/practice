using System;
using AlterEditor.QuestEditor;

namespace AlterEditor
{
    public interface ITask
    {
        void Init();
        void Update();
        void Finish();
        bool IsFinish();
    }
    
    public abstract class TaskBase : ITask
    {
        public abstract void Init();
        public abstract void Update();
        public abstract void Finish();
        public abstract bool IsFinish();
        protected abstract Enum GetNextStep();

        public T GetNextStep<T>() where T : System.Enum
        {
            return (T)GetNextStep();
        }
    }
}