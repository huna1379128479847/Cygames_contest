using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class BurningFactory : IFactory
    {
        public IUniqueThing CreateClass(DataBase data, IHandler effectHandler)
        {
            return new Burning(data as StatusEffectData, effectHandler);
        }
    }
}
