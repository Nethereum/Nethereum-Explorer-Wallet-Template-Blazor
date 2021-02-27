using System.Numerics;

namespace NethereumExplorer.Messages
{
    public class NewBlock
    {
        public NewBlock(BigInteger blockNumber)
        {
            BlockNumber = blockNumber;
        }

        public BigInteger BlockNumber { get; }
    }
}
