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
        public SkillHandler skillHandler;
        public EffectHandler effectHandler;
        public StatusTracker statusTracker;
        private string id;
        private string _name;
        private bool myTurn;
        private bool firstExecute;
        private Skill selectedSkill;
        private UnitType myUnitType;
        public bool IsDead { get; private set; }
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
        public void TurnChange()
        {
            myTurn = !myTurn;
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
            IsDead = true;
        }
        public virtual void Update()
        {
            if (myTurn)
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
        private void Awake()
        {
            id = Guid.NewGuid().ToString("N");
            statusTracker = new StatusTracker(this);
            myUnitType = unitData.UnitType;
        }
    }
}
