using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public interface IUnit
    {
        bool IsPlayer { get; }

        void Update();
    }
}
