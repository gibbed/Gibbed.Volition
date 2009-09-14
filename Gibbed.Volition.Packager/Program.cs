using NConsoler;

namespace Gibbed.Volition.Packager
{
    internal partial class Program
    {
        public static void Main(string[] args)
        {
            //Consolery.Run(typeof(Program), args);
            //Unpack("table.vpp_xbox2", "table_test", true);
            //Unpack("misc.vpp_xbox2", "misc_test", true);
            Unpack(args[0], args[1], true);
        }
    }
}
