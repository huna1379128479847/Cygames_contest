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
            BattleSceneManager.instance.Execute(units, null);
        }
    }
}
