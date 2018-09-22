using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AngryWasp.Helpers;
using AngryWasp.Logger;
using Nerva.Rpc.Wallet;
using Nerva.Rpc.Daemon;

namespace Nerva.Rpc.Tests
{
    public class Program
    {
        private static string walletName = "testnet";
        private static string password = "";
        private static uint daemonPort = 17566;
        private static uint walletPort = 18789;

        [STAThread]
        public static void Main(string[] args)
        {
            Log.CreateInstance(true);

            //Test_GetBlockCount().Wait();
            //Test_GetInfo().Wait();
            //Test_StopMining().Wait();
            //Test_StartMining().Wait();
            //Test_StopDaemon().Wait();
            //Test_SetBans();
        }

        public static void TestWallet()
        {
            //Commented out test methods have been run and verified
            //Test_CreateWallet().Wait();
            //Test_OpenWallet().Wait();
            //Test_QueryKey().Wait();
            //Test_CreateAccount().Wait();
            //Test_GetAccounts().Wait();
            //Test_GetTransfers().Wait();
            //Test_Transfer_NoPaymentId().Wait();
            //Test_Transfer_PaymentId().Wait();
            //Test_StopWallet().Wait();
        }

        public static ulong ToAtomicUnits(double i)
        {
            return (ulong)(i * 1000000000000.0d);
        }

        public static Task Test_CreateWallet()
        {
            return new CreateWallet(new CreateWalletRequestData {
                FileName = walletName,
                Password = password
            }, (string result) => {
                Log.Instance.Write("CreateWallet: Passed");
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "CreateWallet: Failed");
                //Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static Task Test_OpenWallet()
        {
            return new OpenWallet(new OpenWalletRequestData {
                FileName = walletName,
                Password = password
            }, (string result) => {
                Log.Instance.Write("OpenWallet: Passed");
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "OpenWallet: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static Task Test_StopWallet()
        {
            return new StopWallet((string result) => {
                Log.Instance.Write("StopWallet: Passed");
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "StopWallet: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static Task Test_GetAccounts()
        {
            return new GetAccounts((GetAccountsResponseData result) => {
                Log.Instance.Write("GetAccounts: Passed, {0} XNV", result.TotalBalance);
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "GetAccounts: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static Task Test_GetTransfers()
        {
            return new GetTransfers(new GetTransfersRequestData {
                AccountIndex = 0
            }, (GetTransfersResponseData result) => {
                Log.Instance.Write("GetTransfers: Passed, {0}/{1} (in/out)", result.Incoming.Count, result.Outgoing.Count);
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "GetTransfers: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static Task Test_QueryKey()
        {
            return new QueryKey(new QueryKeyRequestData {
                KeyType = Key_Type.All_Keys.ToString().ToLower()
            }, (QueryKeyResponseData result) => {
                Log.Instance.Write("QueryKey: Passed, {0}", result.PublicViewKey);
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "QueryKery: Failed");
            }, walletPort).Run();  
        }

        public static Task Test_CreateAccount()
        {
            return new CreateAccount(new CreateAccountRequestData {
                Label = "New Account"
            }, (CreateAccountResponseData result) => {
                Log.Instance.Write("CreateAccount: Passed");
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "CreateAccount: Failed");
            }, walletPort).Run();  
        }

        public static Task Test_Transfer_NoPaymentId()
        {
            return new Transfer(new TransferRequestData {
                Destinations = new List<TransferDestination>{
                    new TransferDestination{
                        Amount = ToAtomicUnits(0.1),
                        Address = "NV3EMJj8P3n6oddSWqRPAd9W1zJGaXSQCG4Xec6zv5vehgJfGzCoa4bSimxwvT3yXEZ9NeerdLcwZE6edEHkyv981ciN2fKQG"
                    },
                    new TransferDestination{
                        Amount = ToAtomicUnits(0.1),
                        Address = "NV3Nb8bAtKKHX723yEaTxWR6Qvy2UWS28SAmfZHk2ohRNCrw37x7HZH3ubj3P1dz9mD21JP3FUgTXiy3s7gvJob21R3q7TbV4"
                    }
                }
            }, (TransferResponseData result) => {
                Log.Instance.Write("Transfer Without PID: Passed, {0} XNV", result.Amount);
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "Transfer Without PID: Failed");
            }, walletPort).Run();  
        }

        public static Task Test_Transfer_PaymentId()
        {
            return new Transfer(new TransferRequestData {
                Destinations = new List<TransferDestination>{
                    new TransferDestination{
                        Amount = ToAtomicUnits(0.1),
                        Address = "NV3EMJj8P3n6oddSWqRPAd9W1zJGaXSQCG4Xec6zv5vehgJfGzCoa4bSimxwvT3yXEZ9NeerdLcwZE6edEHkyv981ciN2fKQG",
                    },
                    new TransferDestination{
                        Amount = ToAtomicUnits(0.1),
                        Address = "NV3Nb8bAtKKHX723yEaTxWR6Qvy2UWS28SAmfZHk2ohRNCrw37x7HZH3ubj3P1dz9mD21JP3FUgTXiy3s7gvJob21R3q7TbV4"
                    }
                },
                PaymentId = StringHelper.GenerateRandomHexString(64)
            }, (TransferResponseData result) => {
                Log.Instance.Write("Transfer: Passed, {0} XNV", result.Amount);
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "Transfer: Failed");
            }, walletPort).Run();  
        }

        public static Task Test_GetBlockCount()
        {
            return new GetBlockCount((uint result) => {
                Log.Instance.Write("GetBlockCount: Passed, {0}", result);
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "GetBlockCount: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static Task Test_GetInfo()
        {
            return new GetInfo((GetInfoResponseData result) => {
                Log.Instance.Write("GetInfo: Passed, {0}", result.Version);
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "GetInfo: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static Task Test_GetConnections()
        {
            return new GetConnections((List<GetConnectionsResponseData> result) => {
                Log.Instance.Write("GetConnections: Passed, {0} connections", result.Count);
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "GetConnections: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static Task Test_StartMining()
        {
            return new StartMining(new StartMiningRequestData {
                MinerAddress = "NV1r8P6THPASAQX77re6hXTMJ1ykXXvtYXFXgMv4vFAQNYo3YatUvZ8LFNRu4dPQBjTwqJbMvqoeiipywmREPHpD2AgWnmG7Q",
                MiningThreads = 8
            }, (string result) => {
                Log.Instance.Write("StartMining: Passed");
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "StartMining: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static Task Test_StopMining()
        {
            return new StopMining((string result) => {
                Log.Instance.Write("StopMining: Passed");
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "StopMining: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static Task Test_StopDaemon()
        {
            return new StopDaemon((string result) => {
                Log.Instance.Write("StopDaemon: Passed");
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "StopDaemon: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static void Test_SetBans()
        {
            Test_SetBans(true).Wait();
            Test_SetBans(false).Wait();
        }

        private static Task Test_SetBans(bool ban)
        {
            return new SetBans(new SetBansRequestData {
                Bans = new List<Ban> {
                    new Ban { Host = "0.0.0.0", Banned = ban },
                    new Ban { Host = "0.1.0.1", Banned = ban }
                }
            }, (string result) => {
                Log.Instance.Write("SetBans: Passed");
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "SetBans: Failed");
                Environment.Exit(1);
            }, daemonPort).Run(); 
        }
    }
}