using System.Collections.Generic;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public string CardName { get; set; } = "";
        public PaymentType PaymentType { get; set; } = PaymentType.Bank;
        public string AccountNumber { get; set; } = "";
        public string AccountHolderName { get; set; } = "";
        public string BankName { get; set; } = "";
        
        // Foreign key
        public int UserId { get; set; }
    }
} 