using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    public abstract class UnitBase : MonoBehaviour, IUnit, IUniqueThing, IDoAction
    {
        public UnitData unitData;//インスペクターでアタッチすること
        public EffectHandler effectHandler;
        public StatusTracker statusTracker;
        private string id;
        private string _name;
        private bool myTurn;
        private bool firstExecute;
        private Skill selectedSkill;
        private UnitType myUnitType;
        private SkillHandler skillHandler;

        public virtual bool IsDead
        {
            get
            {
                return statusTracker.CurrentHP.IsDead || statusTracker.CurrentMP.IsDead;
            }
        }
        public bool InAction
        {
            get
            {
                return (selectedSkill != null && selectedSkill.InAction);
            }
        }
        public string ID { get { return id; } }
        public virtual UnitType MyUnitType 
        {
            get
            {
                return myUnitType;
            }
            
        }
        public SkillHandler SkillHandler
        {
            get
            {
                return skillHandler;
            }
        }
        public virtual string Name
        {
            get
            {
                return _name;
            }
        }
        public virtual bool CanAction
        {
            get
            {
                if (IsDead) return false;
                foreach (var effects in effectHandler.effectsByTiming)
                {
                    foreach (var effect in effects.Value)
                    {
                        if (FLG.FLGCheck((uint)effect.Value.Flgs, (uint)EffectFlgs.Controll))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        protected virtual void Awake()
        {
            id = Guid.NewGuid().ToString("N");
            statusTracker = new StatusTracker(this);
            myUnitType = unitData.UnitType;
        }
        public virtual void Update()
        {
            if (BattleSceneManager.instance.IsRunning) return;
            if (InAction)
            {
                selectedSkill.SetAction();
                return;
            }
            if (myTurn && CanAction)
            {
                if (firstExecute)
                {
                    effectHandler.EffectExecution(EffectTiming.After);
                    firstExecute = false;
                }
                if (selectedSkill == null || !selectedSkill.InAction)
                {
                    TurnBehavior();
                }
                else
                {
                    selectedSkill.SetAction();
                }

            }
            else if (!firstExecute)
            {
                firstExecute = true;
            }
        }
        public void EnterTurn()
        {
            myTurn = true;
        }
        public void ExitTurn()
        {
            myTurn = false;
        }

        public virtual void DeadBehavior()
        {
            BattleSceneManager.instance.RemoveUnit(ID);
        }
        /// <summary>
        /// 必ず最後に「TurnChange()」と書くこと
        /// </summary>
        public abstract void TurnBehavior();
        public virtual void Notify_Dead()
        {
        }
        public virtual void Pre_TakeDamage()
        {
        }
        public virtual void Post_TakeDamage()
        {
        }

        public virtual void TakeDamage(DamageInfo info, bool isFix)
        {
            Pre_TakeDamage();
            float damage = info.skill.DamageAmount;
            float finalDamage = damage;
            if (!isFix)
            {
            finalDamage = Mathf.Clamp(damage-info.damageTaker.StatusTracker.Def.CurrentAmount, damage / 4, 99999);
            }
            if (info.isBad) finalDamage *= -1;
            statusTracker.CurrentHP.CurrentAmount -= finalDamage;
        }
    }
}
