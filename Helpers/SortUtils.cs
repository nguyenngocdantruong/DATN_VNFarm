using System.Collections.Generic;
using System.Linq;
using VNFarm.Enums;

namespace VNFarm.Helpers
{
    public static class SortUtils
    {
        private static readonly Dictionary<SortType, string> SortTypeMap = new()
        {
            { SortType.Ascending, "Tên A-Z" },
            { SortType.Descending, "Tên Z-A" },
            { SortType.Latest, "Mới nhất" },
            { SortType.Oldest, "Cũ nhất" },
            { SortType.AscendingPrice, "Giá tăng dần" },
            { SortType.DescendingPrice, "Giá giảm dần" }
        };

        private static readonly Dictionary<string, HashSet<SortType>> EntityAllowedSortTypes = new()
        {
            { "Product", new HashSet<SortType> { SortType.Ascending, SortType.Descending, SortType.Latest, SortType.Oldest, SortType.AscendingPrice, SortType.DescendingPrice } },
            { "Order", new HashSet<SortType> { SortType.Oldest, SortType.Latest } },
            { "BusinessRegistration", new HashSet<SortType> { SortType.Latest, SortType.Oldest} },
            { "ChatRoom", new HashSet<SortType> { SortType.Latest, SortType.Oldest } },
            { "ChatMessage", new HashSet<SortType> { SortType.Latest, SortType.Oldest } },
            { "Discount", new HashSet<SortType> { SortType.Ascending, SortType.Descending, SortType.Latest, SortType.AscendingPrice, SortType.DescendingPrice } },
            { "Notification", new HashSet<SortType> { SortType.Latest, SortType.Oldest } },
            { "OrderTimeline", new HashSet<SortType> { SortType.Latest, SortType.Oldest } },
            { "Transaction", new HashSet<SortType> { SortType.Latest, SortType.Oldest } },
            { "User", new HashSet<SortType> { SortType.Latest, SortType.Oldest } },
            { "Store", new HashSet<SortType> { SortType.Latest, SortType.Oldest, SortType.Ascending, SortType.Descending } },
            { "Review", new HashSet<SortType> { SortType.Latest, SortType.Oldest } },
            { "Category", new HashSet<SortType> { SortType.Ascending, SortType.Descending} },
            { "PaymentMethod", new HashSet<SortType> { SortType.Latest, SortType.Oldest } },
            { "RegistrationApprovalResult", new HashSet<SortType> { SortType.Latest, SortType.Oldest } },
            { "RegistrationRequest", new HashSet<SortType> { SortType.Latest, SortType.Oldest } }
        };

        public static Dictionary<int, string> GetAvailableSortTypes(string entityName)
        {
            if (EntityAllowedSortTypes.TryGetValue(entityName, out var allowedTypes))
            {
                return SortTypeMap
                    .Where(kvp => allowedTypes.Contains(kvp.Key))
                    .ToDictionary(kvp => (int)kvp.Key, kvp => kvp.Value);
            }

            return SortTypeMap.ToDictionary(kvp => (int)kvp.Key, kvp => kvp.Value);
        }

        public static Dictionary<string, Dictionary<int, string>> GetSortEntities()
        {
            var entites = new List<string> { "Product", "Order", "BusinessRegistration", "ChatRoom", "ChatMessage", "Discount", "Notification", "OrderTimeline", "Transaction", "User", "Store", "Review", "Category", "PaymentMethod", "RegistrationApprovalResult", "RegistrationRequest" };
            return entites
            .ToDictionary(entity => entity, 
                        entity => GetAvailableSortTypes(entity));
        }
    }
}
