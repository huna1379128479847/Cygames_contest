using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    public class Stab : MonoBehaviour
    {
        [SerializeField] List<GameObject> units = new List<GameObject>();

        public void Start()
        {
            foreach (GameObject unit in units)
            {
                UnitBase unitBase = unit.GetComponent<UnitBase>();
                foreach(var skill in unitBase.unitData.SkillDatas)
                {
                    SkillRegister.Instance.RegisterFactory(skill);
                }
            }
            BattleSceneManager.instance.Execute(units, null);
        }
    }
}
