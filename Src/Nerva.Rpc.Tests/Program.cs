using System;
using AngryWasp.Logger;
using Nerva.Rpc.Wallet;

namespace Nerva.Rpc.Tests
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            
            //CreateWallet
            new CreateWallet((string result) => {
                Log.Instance.Write("CreateWallet: Passed");
                Log.Instance.Write("Data: {0}", result);
            }, () => {
                Log.Instance.Write("CreateWallet: Failed");
            }).Run();

            //StopWallet
            new StopWallet((string result) => {
                Log.Instance.Write("StopWallet: Passed");
            }, () => {
                Log.Instance.Write("StopWallet: Failed");
            }).Run();
        }
    }
}