using System;
using UnityEngine;

namespace AlterEditor.QuestEditor
{
    public class LoadSimurationMap : TaskBase
    {
        public override void Init()
        {
            QuestEditorManager.Instance.LoadSimurationStage("Prefabs/Stage/Stage1-1");
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