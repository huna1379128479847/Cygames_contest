using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public static class Constants
    {
        public const string NamespacePrefix = "Contest.";
        public const string FactoryPostfix = "Factory";

        public static string GetFactory(string name, bool needNameSpace = false)
        {
            return needNameSpace ? NamespacePrefix + name + FactoryPostfix : name + FactoryPostfix;
        }

        public static string GetNameSpace(string name)
        {
            return NamespacePrefix + name;
        }
    }
}

