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
        public int Id { get; }
        public UnitType UnitType { get; }
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

        public virtual void Update()
        {
            if ("現在のターン（未実装）" != UnitType.ToString())
            {
                return;
            }
        }
    }
}
