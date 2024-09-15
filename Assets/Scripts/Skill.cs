using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    public abstract class Skill : MonoBehaviour, IDoAction
    {
        public SkillData skillData;
        public SkillHandler parent;
        private bool inAction;
        private SkillFlgs skillFlgs;
        private SkillTarget target;
        public bool InAction
        {
            get
            {
                return inAction;
            }
        }
        public SkillFlgs SkillFlgs
        {
            get
            {
                return skillFlgs;
            }
        }
        public SkillTarget Target
        {
            get
            {
                return target;
            }
        }
        public virtual bool CanUse
        {
            get
            {
                return skillData.Cost >= parent.parent.statusTracker.CurrentMP.CurrentAmount;
            }
        }
        public virtual void SetAction()
        {
            if (!InAction)
            {
                inAction = true;
            } 
        }
        public virtual void EndAction()
        {
            if (InAction)
            {
                inAction = false;
            }
        }
        void Update()
        {

        }
    }
}
