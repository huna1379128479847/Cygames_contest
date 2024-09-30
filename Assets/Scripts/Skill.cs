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
        protected SkillHandler parent;
        private bool inAction;
        private SkillFlgs skillFlgs;
        private SkillTarget target;
        private bool isBad;
        private bool isAttack;
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
        public virtual int DamageAmount
        {
            get
            {
                return (int)(skillData.Amount + parent.parent.statusTracker.Atk.CurrentAmount * skillData.Magnification);
            }
        }
        public virtual void SetAction()
        {
            if (!inAction)
            {
                inAction = true;
            } 
        }
        public virtual void EndAction()
        {
            if (inAction)
            {
                inAction = false;
            }
        }
        void Update()
        {
            if (inAction)
            {

            }
        }
    }
}
