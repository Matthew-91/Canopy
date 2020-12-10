using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Canopy.Data
{
    [MetadataType(typeof(TransactionMetaData))]
    public partial class Transaction
    {

    }

    public class TransactionMetaData
    {
        public int TransactionsId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        [DataType (DataType.Currency)]
        public decimal Amount { get; set; }
        public string Description { get; set; }
        [DataType (DataType.Date)]
        [DisplayFormat (ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public System.DateTime When { get; set; }
        public string Memo { get; set; }
        public bool IsWithdraw { get; set; }
    }
}