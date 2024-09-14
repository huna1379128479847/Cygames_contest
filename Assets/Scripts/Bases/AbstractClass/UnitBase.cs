using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    public abstract class UnitBase : MonoBehaviour, IUnit, IStats, IUniqueThing
    {
        [SerializeField] object UnitData;//未実装のため仮でobject
        private string id;
        private string _name;
        private int hp;
        private int mp;
        private int speed;
        private int atk;
        private int def;
        private bool myTurn;
        private bool firstExecute;
        private SkillTracker skillTracker;
        public List<StatusEffect> statusEffects = new List<StatusEffect>();
        public bool IsDead {  get; private set; }
        public string ID { get { return id; } }
        public UnitType MyUnitType { get; }
        public UnitBase MyUnitBase
        {
            get
            {
                return this;
            }
        }
        public SkillTracker SkillTracker 
        {
            get
            {
                return skillTracker;
            }
        }
        public virtual string Name
        {
            get
            {
                return _name;
            }
        }

        public int MaxHP { get; }
        public int CurrentHP
        {
            get
            {
                return hp;
            }
        }
        public int MaxMP { get; }
        public int CurrentMP
        {
            get
            {
                return mp;
            }
        }
        public int MaxSpeed { get; }
        public int CurrentSpeed
        {
            get
            {
                return speed;
            }
        }
        public int Atk
        {
            get
            {
                return atk;
            }
        }
        public int Def
        {
            get
            {
                return def;
            }
        }

        public void TurnChange()
        {
            myTurn ^= true;
        }
        public virtual void DeadBehavior()
        {
            BattleSceneManager.instance.RemoveUnit(ID);
        }
        /// <summary>
        /// 必ず最後に「TurnChange()」と書くこと
        /// </summary>
        public abstract void TurnBehavior();
        public abstract void ActionBehavior();
        public virtual void Notify_Dead()
        {
            IsDead = true;
        }
        public virtual void Notify_CC() 
        { 
        }
        public virtual void AddEffect(StatusEffect effect)
        {
            if (effect != null && !statusEffects.Contains(effect))
            {
                statusEffects.Add(effect);
                effect.Parent = this;
            }
            else if (statusEffects.Contains(effect))
            {
                statusEffects.ForEach(haveEffect =>
                {
                    if (haveEffect.Name == effect.Name)
                    {
                        haveEffect.UpdateStatsEffect();
                    }
                });
            }
        }
        public virtual void Update()
        {
            if (myTurn)
            {
                if (firstExecute)
                {
                    statusEffects.ForEach(effect => effect.ExecuteEffect());
                    firstExecute = false;
                }
                TurnBehavior();
            }
            else if (!firstExecute)
            {
                firstExecute = true;
            }
        }
        private void Awake()
        {
            id = Guid.NewGuid().ToString("N");
            hp = MaxHP;
            mp = MaxMP;
            speed = MaxSpeed;
        }
    }
}
