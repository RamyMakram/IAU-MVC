using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAU.Shared
{
    public enum NewAndFlollowRequestLoginEnum
    {
        Nafath,
        Mustafeed,
        All
    }
    class EnumTranalation_object
    {
        public string Key { get; set; }
        public string Ar { get; set; }
        public string En { get; set; }
    }
    public static class Translate_NewAndFlollowRequestLoginEnum
    {
        private static EnumTranalation_object[] Tran_obi = new EnumTranalation_object[]
        {
            new EnumTranalation_object
            {
                Key="Nafath",
                Ar="نفاذ",
                En="Nafath"
            },
            new EnumTranalation_object
            {
                Key="Mustafeed",
                Ar="منسوبي الجامعة",
                En="IAU Affiliated"
            },
            new EnumTranalation_object
            {
                Key="All",
                Ar="الجميع",
                En="All"
            },
        };
        public static string Translate(this NewAndFlollowRequestLoginEnum _enum, bool isar)
        {
            var data = Tran_obi.FirstOrDefault(q => q.Key == _enum.ToString());
            return isar ? data?.Ar : data?.En;
        }
    }
}
