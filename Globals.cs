using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GlobalVariable
{
    public static class Globals
    {


#if DEBUG

        public static string databasePath = @"C:\Users\ASL Technologies\source\repos\Contextual\Contextual\concdb.mdf";

#else

        static string machineName = Directory.GetCurrentDirectory();

        //static string machineName = |DataDirectory|;

        public static string databasePath = machineName + @"\concdb.mdf";

#endif




    }
}
