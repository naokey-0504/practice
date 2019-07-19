using System;
using System.Collections.Generic;
using UnityEngine;

namespace AlterEditor.QuestEditor
{
    public class QuestEditorController : MonoBehaviour
    {
        private static readonly Dictionary<TaskStep, TaskBase> kTaskDic = new Dictionary<TaskStep, TaskBase>
        {
            { TaskStep.Init, new InitTask() },
            { TaskStep.LoadSimurationStage, new LoadSimurationMap() },
            { TaskStep.DrawGrid, new DrawGridTask() },
        };

        private TaskStep m_Step = TaskStep.None;
        private TaskBase m_Task;
        
        public void Start()
        {
            translateTask(TaskStep.Init);
        }

        public void Update()
        {
            if (m_Task != null)
            {
                m_Task.Update();

                if (m_Task.IsFinish())
                {
                    m_Task.Finish();

                    translateTask(m_Task.GetNextStep<TaskStep>());
                }
            }
        }

        private void translateTask(TaskStep step)
        {
            m_Step = step;
            m_Task = kTaskDic.ContainsKey(step) ? kTaskDic[step] : null;
            m_Task.Init();
        }
    }
}