namespace NethereumExplorer.Messages
{
    public class AccountTransactionCompleted
    {
        public AccountTransactionCompleted(string transactionHash, string accountAddress, bool? failed = false)
        {
            TransactionHash = transactionHash;
            AccountAddress = accountAddress;
            Failed = failed;
        }

        public bool? Failed { get; }
        public string TransactionHash { get; }
        public string AccountAddress { get; }
    }
}
