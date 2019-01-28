using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NethereumBlazor.Model
{
    public class AccountInfo
    {
        public string Address { get; set; }
        private ConcurrentDictionary<string, TransactionInfo> _transactions = new ConcurrentDictionary<string, TransactionInfo>();

        public void AddOrUpdateTransaction(TransactionInfo transaction)
        {
            _transactions.AddOrUpdate(transaction.TransactionHash, transaction, (x, y)=> transaction);
        }

        public List<TransactionInfo> GetTransactions(string chainId)
        {
            return _transactions.Values.Where(x => x.ChainId == chainId).ToList();
        }

        public List<TransactionInfo> GetPendingTransactions(string chainId)
        {
            return _transactions.Values.Where(x => x.ChainId == chainId && x.Pending == true).ToList();
        }
    }
}
