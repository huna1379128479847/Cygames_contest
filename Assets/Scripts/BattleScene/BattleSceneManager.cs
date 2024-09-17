using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Unity.VisualScripting;
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
        private bool endInitialize = false;
        private bool canProgress = true;
        private bool endAction = false;
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
                    ChangeUnitCount(unitBase);
                }
            }
            Notify_FinishInitialize();
            endInitialize = true;
        }
        private void ChangeUnitCount(UnitBase unitBase, int count = 1)
        {
            if ((unitBase.MyUnitType & (UnitType.Enemy | UnitType.EnemyAI)) != 0)
            {
                enemy += count;
            }
            else if ((unitBase.MyUnitType & (UnitType.Friend | UnitType.FriendAI)) != 0)
            {
                friend += count;
            }
        }
        private void TurnManage()//ターンを管理し、各ユニットにターンを渡す
        {
            if (!IsRunning) { return; }
            //論理積で今のターンがユニットの種別と合致するか判定する
            if (unitBases[idx] != null && FLG.FLGCheck((uint)unitBases[idx].MyUnitType ,(uint)turn))
            {
                canProgress = false;
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
            if (unitBases[idx] != null || friend == 0 || enemy == 0)
            {
                shouldEndBattle = true;
            }
        }
        void Update()
        {
            if (!IsRunning) { return; }
            if (endInitialize)  // スペルを修正
            {
                if (canProgress)
                {
                    TurnManage();
                }
                if (!canProgress && endAction)
                {
                    canProgress = true;
                    endAction = false;
                    idx++;
                    if (idx >= unitBases.Count)
                    {
                        TurnChange();
                    }
                }
            }
        }

        protected virtual void Notify_FinishInitialize()
        {
        }

        protected virtual void Notify_StartInitialize()
        {
        }
        public virtual void RemoveUnit(string id)
        {
            for (int i = 0; i < unitBases.Count; i++)
            {
                if (unitBases[i].ID == id)
                {
                    ChangeUnitCount(unitBases[i], -1);
                    unitBases.RemoveAt(i);
                    Destroy(units[i]);
                    units.RemoveAt(i);
                    IsBattleEnd();
                }
            }
        }
        public void Notify_EndUnitAction()
        {
            endAction = true;
        }
    }
}
