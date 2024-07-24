using _01_Framework.Domain;
using System;

namespace DeliveryManagement.Domain.DeliveryAgg
{
    public class Delivery : EntityBase
    {
        public string Address { get; private set; }

        public string PostalCode { get; private set; }
        public long AccountId { get; private set; }

        public Delivery(string address, string postalCode, long accountId)
        {
            Address = address;
            PostalCode = postalCode;
            AccountId = accountId;
        }

        public void Edit(string address, string postalCode)
        {
            Address = address;
            PostalCode = postalCode;
        }
    }
}
