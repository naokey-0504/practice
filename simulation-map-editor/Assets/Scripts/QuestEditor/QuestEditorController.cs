using System;
using System.Collections.Generic;
using UnityEngine;

namespace AlterEditor.QuestEditor
{
    public class QuestEditorController : MonoBehaviour
    {
        private static readonly Dictionary<TaskBase.Step, TaskBase> kTaskDic = new Dictionary<TaskBase.Step, TaskBase>
        {
            { TaskBase.Step.Init, new InitTask() },
            { TaskBase.Step.LoadSimurationStage, new LoadSimurationMap() },
            { TaskBase.Step.DrawGrid, new DrawGridTask() },
        };

        private TaskBase.Step m_Step = TaskBase.Step.None;
        private TaskBase m_Task;
        
        public void Start()
        {
            translateTask(TaskBase.Step.Init);
        }

        public void Update()
        {
            if (m_Task != null)
            {
                m_Task.Update();

                if (m_Task.IsFinish())
                {
                    m_Task.Finish();

                    translateTask(m_Task.GetNextStep());
                }
            }
        }

        private void translateTask(TaskBase.Step step)
        {
            m_Step = step;
            m_Task = kTaskDic.ContainsKey(step) ? kTaskDic[step] : null;
            m_Task.Init();
        }
    }
}