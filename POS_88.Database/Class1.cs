using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace POS_88.Database
{
    public static class EFResourceChecker
    {
        public static void Check()
        {
            Assembly assembly = typeof(POSDBEntities).Assembly;

            foreach (string resource in assembly.GetManifestResourceNames())
            {
                Console.WriteLine(resource);
            }

            //var assembly2 = typeof(POSDBEntities).Assembly;

            //System.Diagnostics.Debug.WriteLine(assembly2.FullName);
        }
    }
}
