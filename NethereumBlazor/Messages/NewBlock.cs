using System.Numerics;

namespace NethereumBlazor.Messages
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
