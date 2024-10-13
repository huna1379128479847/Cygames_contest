using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class DecreaseDefence : StatusEffect
    {
        private int decreaseDefAmount = 0;
        private StatusBase def;

        public DecreaseDefence(StatusEffectData data, IHandler handler)
            : base(data, handler)
        {
            this.decreaseDefAmount = data.Amount;
            def = ParentHandler.ParentUnit.statusTracker.Def;
        }

        public override void Apply()
        {
            base.Apply();
            def.AddEffect(ID, decreaseDefAmount);
            RemoveEvent += def.RemoveEffect;
        }

        public override void Remove()
        {
            base.Remove();
            RemoveEvent -= def.RemoveEffect;
        }
        public override void ExecuteEffect()
        {
        }
    }
}
