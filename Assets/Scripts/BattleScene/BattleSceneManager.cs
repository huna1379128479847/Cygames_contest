using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Contest
{
    public class BattleSceneManager : SingletonBehavior<BattleSceneManager>, IManager
    {
        private Turn turn = Turn.Friend;
        private bool isRunning;
        List<GameObject> units = new List<GameObject>();
        List<UnitBase> unitBases = new List<UnitBase>();
        private bool shouldEndBattle = false;
        private bool endInisialize = false;
        private bool canProgress = true;
        private int friend = 0;
        private int enemy = 0;
        private int idx = 0;
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                }
            }
        }
        public Turn Turn
        {
            get
            {
                return turn;
            }
        }

        public void Execute(List<object> datas)
        {
            Notify_StartInitialize();
            foreach (object obj in datas)
            {

                if (obj is GameObject && (obj as GameObject).TryGetComponent<UnitBase>(out var unitBase) == true)
                {
                    units.Add(obj as GameObject);
                    unitBases.Add(unitBase);
                    if ((unitBase.MyUnitType & (UnitType.Enemy | UnitType.EnemyAI)) != 0)
                    {
                        enemy++;
                    }
                    else if ((unitBase.MyUnitType & (UnitType.Friend | UnitType.FriendAI)) != 0)
                    {
                        friend++;
                    }
                }
            }
            Notify_FinishInitialize();
            endInisialize = true;
        }

        private void TurnManage()//ターンを管理し、各ユニットにターンを渡す
        {
            if (!IsRunning) { return; }
            canProgress = false;

            //論理積で今のターンがユニットの種別と合致するか判定する
            if (unitBases[idx] != null && ((uint)unitBases[idx].MyUnitType & (uint)turn) != 0)
            {
                unitBases[idx].TurnChange();
            }
            else if (unitBases[idx] == null)
            {
                Debug.LogError($"{units[idx].name}にUnitBaseが設定されていません。");
            }

            
        }

        private void TurnChange()
        {
            turn ^= Turn.All;
        }

        private void IsBattleEnd()
        {
            //いろいろ判定し
            shouldEndBattle = true;
        }

        public virtual void RemoveUnit(string id)
        {
            for (int i = 0; i < unitBases.Count; i++)
            {
                if (unitBases[i].ID == id)
                {
                    unitBases.RemoveAt(i);
                    Destroy(units[i]);
                    units.RemoveAt(i);
                }
            }
        }


        protected virtual void Notify_FinishInitialize()
        {
        }

        protected virtual void Notify_StartInitialize()
        {
        }

        void Update()
        {
            if (endInisialize)
            {
                if (canProgress)
                {
                    TurnManage();
                }
            }
        }

        public void Notify_EndUnitAction()
        {
            canProgress = true;
        }
    }
}
