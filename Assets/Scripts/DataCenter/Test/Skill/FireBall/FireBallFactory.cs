using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class FireBallFactory : IFactory
    {
        public IUniqueThing CreateClass(DataBase data, IHandler handler)
        {
            return new FireBall(data as SkillData, handler);
        }
    }
}
