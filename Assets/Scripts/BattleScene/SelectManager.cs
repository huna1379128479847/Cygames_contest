using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    public class SelectManager : SingletonBehavior<SelectManager>, IManager
    {
        private Skill skill;
        private List<UnitBase> selectUnits;
        public bool IsRunning { get; set; }
        public bool InSelecting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public void Execute(Skill skill)
        {
            this.skill = skill;
        }
        public void Exit()
        {
            skill = null;
        }
        public bool SelectionFilter(UnitBase selected)//skillに設定された
        {
            //未完成
            return true;
        }
        private void Update()
        {
            if (IsRunning && Input.GetMouseButtonDown(0) && skill != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                UnitBase unit;
                if (Physics.Raycast(ray, out hit) && hit.collider.TryGetComponent<UnitBase>(out unit))
                {
                    if (SelectionFilter(unit) && !selectUnits.Contains(unit))
                    {
                        selectUnits.Add(unit);
                    }
                    else if (selectUnits.Contains(unit))
                    {
                        selectUnits.Remove(unit);
                    }
                }
            }
        }
    }
}
