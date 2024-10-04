using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    public class Skill : IDoAction, IUniqueThing
    {
        public SkillData skillData;
        public SkillHandler parent;
        private Guid id;
        private bool inAction;
        private SkillFlgs skillFlgs;
        private bool isBad;
        private bool isAttack;
        public Guid ID
        {
            get
            {
                return id;
            }
        }
        /// <summary>
        /// アニメーション実行中かどうか。エフェクト再生中等でもTrue.
        /// メモ：コルーチンを使った方法に書き換えるかもしれない
        /// </summary>
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
                SkillSelectManager.instance.Init(this);
            }
        }
        public virtual void EndAction()
        {
            if (inAction)
            {
                inAction = false;
            }
        }
        public virtual void InvokeSkill(List<UnitBase> units)
        {
            if (units == null || units.Count == 0) { return; }
            AnimationHandler.InvokeAnimation(parent.parent.gameObject, skillData.AnimationType);
            foreach (UnitBase unit in units)
            {
                if (unit != null)
                {
                    unit.TakeDamage(new DamageInfo(this, parent.parent, unit), false);
                }
            }
        }
        public Skill(SkillData skillData, SkillHandler skillHandler)
        {
            id = Guid.NewGuid();
            this.skillData = skillData;
            parent = skillHandler;
        }
    }
}
