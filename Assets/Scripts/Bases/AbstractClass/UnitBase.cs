using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    public abstract class UnitBase : MonoBehaviour, IUnit, IStats
    {
        private string _name;
        private int hp;
        private int mp;
        private int speed;
        private int atk;
        private int def;
        private SkillTracker skillTracker;
        public int Id { get; }
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

        public virtual void TurnBehavior()
        {
            if (!FLGChecker.FLGCheck(((uint)BattleSceneManager.instance.Turn), (uint)MyUnitType))
            {
                return;
            }
        }

        public UnitBase()
        {
            hp = MaxHP;
            mp = MaxMP;
            speed = MaxSpeed;
        }
    }
}
