using System;
using System.Linq;
using Nerva.Rpc.Daemon;

namespace Nerva.Rpc.Tests
{
    public class ChainAnalysis
    {
        public static void CheckChainForTxType(Tx_Type txType)
        {
            new GetBlockCount((r) =>
            {
                Console.WriteLine($"Block height - {r}");
                for (uint i = 16000; i < r; i++)
                {
                    if (i % 1000 == 0)
                        Console.WriteLine($"Parsed {i} blocks");
                    new GetBlockByHeight(new GetBlockByHeightRequestData
                    {
                        Height = i
                    }, (b) =>
                    {
                        if (b.JsonData.TxHashes.Length > 0)
                        {
                            new GetTransactions(new GetTransactionsRequestData
                            {
                                Hashes = b.JsonData.TxHashes.ToList()
                            }, (t) =>
                            {
                                //AngryWasp.Logger.Log.Instance.Write($"Transactions found on block {i}");
                                foreach (var tx in t.Transactions)
                                {
                                    if (tx.RingCtSignatures.Type == txType)
                                        Console.WriteLine($"Transactions type {txType.ToString()} found in block {i}");
                                }
                            }, null).Run();
                        }
                    }, null).Run();
                }
            }, (e) =>
            {

            }).Run();
        }
    }
}
