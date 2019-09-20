using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace test_we.Models
{
    [Serializable]
    [DataContract]
    public class SumStruct
    {
        [DataMember]
        private string address;
        [DataMember]
        string cash { get; set; }
        [DataMember]
        int money { get; set; }
        [DataMember]
        int price { get; set; }
        public override bool Equals(object obj)
        {
            return this.GetType() == obj.GetType() && this.address.Equals(((SumStruct)obj).address) && this.cash.Equals(((SumStruct)obj).cash);
        }
        public void sum(SumStruct dto)
        {
            this.money += dto.money;
            this.price+=dto.price;
        }
        public SumStruct() { }
        public SumStruct(string address, string cash, int money, int price)
        {
            this.address = address;
            this.cash = cash;
            this.money = money;
            this.price = price;
        }
        public string GetAddress() { return address; }
        public string GetCash() { return cash; }
        public int GetMoney() { return money; }
        public int GetPrice() { return price; }
    }
}
