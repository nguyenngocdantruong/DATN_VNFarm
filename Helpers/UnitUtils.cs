using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Helpers
{
    public static class UnitUtils
    {
        private static Dictionary<Unit, string> UnitDictionary = new Dictionary<Unit, string>
        {
            { Unit.Kg, "kg" },
            { Unit.Box, "hộp" },
            { Unit.Piece, "cái" },
            { Unit.Pack, "gói" },
            { Unit.Bag, "túi" },
            { Unit.Can, "thùng" },
            { Unit.Roll, "cuộn" },
            { Unit.L, "lít" },
            { Unit.Ml, "ml" },
        };

        public static Dictionary<int, string> GetUnitsForForm()
        {
            return UnitDictionary.OrderBy(x => x.Value).ToDictionary(x => (int)x.Key, x => x.Value);
        }

        public static string GetUnitName(Unit? unit)
        {
            if (unit == null)
            {
                return "Unknown";
            }
            return UnitDictionary[unit.Value];
        }
    }
}
