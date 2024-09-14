using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public abstract class Player : UnitBase, IPlayerHandler
    {
        
        public override void TurnBehavior()
        {
            GeneratePlayerOptions();
        }

        public abstract void GeneratePlayerOptions();

        public abstract void HandlePlayerSelection(int selectionId);
    }

}
