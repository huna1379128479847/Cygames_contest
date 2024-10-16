using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class PowerBarrirerFactory : IFactory
    {
        public IUniqueThing CreateClass(DataBase data, IHandler handler)
        {
            return new PowerBarrirer(data as SkillData, handler);
        }
    }
}
