using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    public abstract class Skill : MonoBehaviour, IDoAction, IUniqueThing
    {
        public Guid id;
        public SkillData skillData;
        public SkillHandler parent;
        private bool inAction;
        private SkillFlgs skillFlgs;
        private TargetingPateren target;
        private bool isBad;
        private bool isAttack;
        public Guid ID
        {
            get
            {
                return id;
            }
        }
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
        public TargetingPateren Target
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
        public virtual int DamageAmount// 基本的な効果量の計算
        {
            get
            {
                return (int)(skillData.Amount + parent.parent.statusTracker.Atk.CurrentAmount * skillData.Magnification);
            }
        }
        public virtual void SetAction() // スキルエフェクトなどの描画処理へ移行
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
        public virtual void Notyfy_Selected(List<UnitBase> units)
        {
            if (units == null || units.Count == 0) { return; }
            foreach (UnitBase unit in units)
            {
                if (unit != null)
                {
                    unit.TakeDamage(new DamageInfo(this, parent.parent, unit), false);
                }
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
