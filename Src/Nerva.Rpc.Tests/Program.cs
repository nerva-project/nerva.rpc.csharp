using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AngryWasp.Helpers;
using Nerva.Rpc.Wallet;
using Nerva.Rpc.Daemon;
using System.Threading;
using Log_Severity = AngryWasp.Logger.Log_Severity;

namespace Nerva.Rpc.Tests
{
    public class Program
    {
        // These tests use the following wallets.
        // NV2kiVrAgSjY7pZUcE6u9NFFg4urXDAnBBoJhC2b1xBpBfVx19rSLFBKbPq8ipeAEjau8Vm6mXRtN2AKugV7PG6v1Nw2V6m75
        // getting wiring physics richly maps upload sedan pool fifteen dying vampire voted recipe owed dice peeled ionic ugly foamy vary february ability afield ambush upload
        // viewkey
        // secret: a94338299a19a85c4fa14e9ae7d551ef492e9ae3c21625e2531e1cdeee664c01
        // public: 2af1aa5af95a6f2905cc13049378caaae57388a4ee9306f09f308265e7832c0a
        // spendkey
        // secret: f5dc8cae87b5b863eca14e9f187e41064ff9f790ba7b6a77f175d05b8e5cd505
        // public: aceec4bd0008ba0a366287754e6d55368c20f613b560408f27542c4654af3fc1


        // NV2K9m13tbr3NhSubtR3tD3kzWhEbqrkuZi3ts9XxZvqgeMCG1MkQQgF4cpiuEWdEeWiZhgzGdFKPcSNByCLaass2h6ftye3Q
        // because bicycle omission visited austere seeded runway upright stylishly often abducts pipeline toolbox abort segments vacation cider subtly ribbon gauze tanks huts eight factual abducts

        //Don't be a dingus and try to use these wallets yourself
        private const string ADDRESS_A = "NV2kiVrAgSjY7pZUcE6u9NFFg4urXDAnBBoJhC2b1xBpBfVx19rSLFBKbPq8ipeAEjau8Vm6mXRtN2AKugV7PG6v1Nw2V6m75";
        private const string SEED_A = "getting wiring physics richly maps upload sedan pool fifteen dying vampire voted recipe owed dice peeled ionic ugly foamy vary february ability afield ambush upload";
        private const string PVK_A = "a94338299a19a85c4fa14e9ae7d551ef492e9ae3c21625e2531e1cdeee664c01";
        private const string PSK_A = "f5dc8cae87b5b863eca14e9f187e41064ff9f790ba7b6a77f175d05b8e5cd505";
        
        private const string PID_A = "f9a540b30a3c82e4";
        private const string INT_A = "NizKkMN12wH2TfHaJQAMDFKHfcXRNTCfnH6mUNoWpZmuWK8quE4vB5JG8Ks261UdT8MCqAqZAFFy7RbM8kbdV7XcNwXvS2i6GYq3J11k35GGF";

        private const string ADDRESS_B = "NV2K9m13tbr3NhSubtR3tD3kzWhEbqrkuZi3ts9XxZvqgeMCG1MkQQgF4cpiuEWdEeWiZhgzGdFKPcSNByCLaass2h6ftye3Q";
        private const string SEED_B = "because bicycle omission visited austere seeded runway upright stylishly often abducts pipeline toolbox abort segments vacation cider subtly ribbon gauze tanks huts eight factual abducts";

        private static uint daemonPort = 18566;
        private static uint walletPort = 22525;

        [STAThread]
        public static void Main(string[] args)
        {
            AngryWasp.Logger.Log.CreateInstance(true);

            Process.Start("nerva-wallet-rpc", "--testnet --rpc-bind-port 22525 --daemon-address 127.0.0.1:18566 --disable-rpc-login --wallet-dir ./");
            Thread.Sleep(1000);
            CommandLineParser cmd = CommandLineParser.Parse(args);

            string w = StringHelper.GenerateRandomHexString(4, true);
            string p = StringHelper.GenerateRandomHexString(4, true);

            AngryWasp.Logger.Log.Instance.Write($"Generated: {w} {p}");
            Test_CreateHwWallet(w, p);
            Test_GetAccounts();
            Test_CloseWallet();
        }

        public static ulong ToAtomicUnits(double i)
        {
            return (ulong)(i * 1000000000000.0d);
        }

        public static bool Test_RestoreWalletFromSeed(string wallet_file, string wallet_pass)
        {
            return new RestoreWalletFromSeed(new RestoreWalletFromSeedRequestData {
                Seed = SEED_A,
                FileName = wallet_file,
                Password = wallet_pass,
            }, (RestoreWalletFromSeedResponseData result) => {
                if (ADDRESS_A == result.Address)
                    AngryWasp.Logger.Log.Instance.Write("RestoreWalletFromSeed: Passed");
                else
                    AngryWasp.Logger.Log.Instance.Write("RestoreWalletFromSeed: Failed. Mismatch");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "RestoreWalletFromSeed: Failed");
                Environment.Exit(1);
            }, walletPort).Run(); 
        }

        public static bool Test_RestoreWalletFromKeys(string wallet_file, string wallet_pass)
        {
            return new RestoreWalletFromKeys(new RestoreWalletFromKeysRequestData {
                ViewKey = PVK_A,
                SpendKey = PSK_A,
                Address = ADDRESS_A,
                FileName = wallet_file,
                Password = wallet_pass
            }, (RestoreWalletFromKeysResponseData result) => {
                if (ADDRESS_A == result.Address)
                    AngryWasp.Logger.Log.Instance.Write("RestoreWalletFromKeys: Passed");
                else
                    AngryWasp.Logger.Log.Instance.Write("RestoreWalletFromKeys: Failed. Mismatch");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "RestoreWalletFromKeys: Failed");
                Environment.Exit(1);
            }, walletPort).Run(); 
        }

        public static bool Test_GetBlockTemplate()
        {
            return new GetBlockTemplate(new GetBlockTemplateRequestData {
                Address = ADDRESS_A,
                ReserveSize = 60
            }, (GetBlockTemplateResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write("GetBlockTemplate: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "GetBlockTemplate: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static bool Test_CreateWallet(string wallet_file, string wallet_pass)
        {
            return new CreateWallet(new CreateWalletRequestData {
                FileName = wallet_file,
                Password = wallet_pass
            }, (CreateWalletResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write("CreateWallet: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "CreateWallet: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static bool Test_CreateHwWallet(string wallet_file, string wallet_pass)
        {
            return new CreateHwWallet(new CreateHwWalletRequestData {
                FileName = wallet_file,
                Password = wallet_pass
            }, (CreateHwWalletResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write("CreateHwWallet: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "CreateHwWallet: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static bool Test_OpenWallet(string wallet_file, string wallet_pass)
        {
            return new OpenWallet(new OpenWalletRequestData {
                FileName = wallet_file,
                Password = wallet_pass
            }, (string result) => {
                AngryWasp.Logger.Log.Instance.Write("OpenWallet: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "OpenWallet: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static bool Test_CloseWallet()
        {
            return new CloseWallet((string result) => {
                AngryWasp.Logger.Log.Instance.Write("CloseWallet: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "CloseWallet: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static bool Test_StopWallet()
        {
            return new StopWallet((string result) => {
                AngryWasp.Logger.Log.Instance.Write("StopWallet: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "StopWallet: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static bool Test_GetAccounts()
        {
            return new GetAccounts((GetAccountsResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write($"GetAccounts: Passed, {result.TotalBalance} XNV");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "GetAccounts: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static bool Test_GetTransfers()
        {
            return new GetTransfers(new GetTransfersRequestData {
                AccountIndex = 0
            }, (GetTransfersResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write($"GetTransfers: Passed, {result.Incoming.Count}/{result.Outgoing.Count} (in/out)");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "GetTransfers: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  
        }

        public static bool Test_QueryKey()
        {
            return new QueryKey(new QueryKeyRequestData {
                KeyType = Key_Type.All_Keys.ToString().ToLower()
            }, (QueryKeyResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write($"QueryKey: Passed, {result.PublicViewKey}");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "QueryKery: Failed");
            }, walletPort).Run();  
        }

        public static bool Test_CreateAccount()
        {
            return new CreateAccount(new CreateAccountRequestData {
                Label = "New Account"
            }, (CreateAccountResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write("CreateAccount: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "CreateAccount: Failed");
            }, walletPort).Run();  
        }

        public static bool Test_Transfer_NoPaymentId()
        {
            return new Transfer(new TransferRequestData {
                Destinations = new List<TransferDestination>{
                    new TransferDestination{
                        Amount = ToAtomicUnits(0.1),
                        Address = ADDRESS_A
                    },
                    new TransferDestination{
                        Amount = ToAtomicUnits(0.1),
                        Address = ADDRESS_B
                    }
                }
            }, (TransferResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write($"Transfer Without PID: Passed, {result.Amount} XNV");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "Transfer Without PID: Failed");
            }, walletPort).Run();  
        }

        public static bool Test_MakeIntegratedAddress(string w1, string w2)
        {
            new MakeIntegratedAddress(new MakeIntegratedAddressRequestData {
                StandardAddress = w1
            }, (MakeIntegratedAddressResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write($"MakeIntegratedAddress: Passed, {result.IntegratedAddress}");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "MakeIntegratedAddress: Failed");
                Environment.Exit(1);
            }, walletPort).Run(); 

            new MakeIntegratedAddress(new MakeIntegratedAddressRequestData {
                PaymentId = PID_A,
                StandardAddress = w1
            }, (MakeIntegratedAddressResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write($"MakeIntegratedAddress: Passed, {result.IntegratedAddress}");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "MakeIntegratedAddress: Failed");
                Environment.Exit(1);
            }, walletPort).Run();  

            return true;
        }

        public static bool Test_SplitIntegratedAddress(string ia, string sa, string pid)
        {
            new SplitIntegratedAddress(new SplitIntegratedAddressRequestData {
                IntegratedAddress = ia
            }, (SplitIntegratedAddressResponseData result) => {
                if (result.StandardAddress == sa && result.PaymentId == pid)
                    AngryWasp.Logger.Log.Instance.Write($"SplitIntegratedAddress: Passed");
                else
                    AngryWasp.Logger.Log.Instance.Write($"SplitIntegratedAddress: Failed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "SplitIntegratedAddress: Failed");
                Environment.Exit(1);
            }, walletPort).Run(); 

            return true;
        }

        public static bool Test_Transfer_PaymentId()
        {
            return new Transfer(new TransferRequestData {
                Destinations = new List<TransferDestination>{
                    new TransferDestination{
                        Amount = ToAtomicUnits(0.1),
                        Address = ADDRESS_A,
                    },
                    new TransferDestination{
                        Amount = ToAtomicUnits(0.1),
                        Address = ADDRESS_B
                    }
                },
                PaymentId = StringHelper.GenerateRandomHexString(64)
            }, (TransferResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write($"Transfer: Passed, {result.Amount} XNV");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "Transfer: Failed");
            }, walletPort).Run();  
        }

        public static bool Test_GetBlockCount()
        {
            return new GetBlockCount((uint result) => {
                AngryWasp.Logger.Log.Instance.Write($"GetBlockCount: Passed, {result}");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "GetBlockCount: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static bool Test_GetInfo()
        {
            return new GetInfo((GetInfoResponseData result) => {
                AngryWasp.Logger.Log.Instance.Write($"GetInfo: Passed, {result.Version}");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "GetInfo: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static bool Test_GetConnections()
        {
            return new GetConnections((List<GetConnectionsResponseData> result) => {
                AngryWasp.Logger.Log.Instance.Write($"GetConnections: Passed, {result.Count} connections");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "GetConnections: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static bool Test_StartMining()
        {
            return new StartMining(new StartMiningRequestData {
                MinerAddress = ADDRESS_A,
                MiningThreads = 2
            }, (string result) => {
                AngryWasp.Logger.Log.Instance.Write("StartMining: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "StartMining: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static bool Test_StopMining()
        {
            return new StopMining((string result) => {
                AngryWasp.Logger.Log.Instance.Write("StopMining: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "StopMining: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static bool Test_StopDaemon()
        {
            return new StopDaemon((string result) => {
                AngryWasp.Logger.Log.Instance.Write("StopDaemon: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "StopDaemon: Failed");
                Environment.Exit(1);
            }, daemonPort).Run();  
        }

        public static void Test_SetBans()
        {
            Test_SetBans(true);
            Test_SetBans(false);
        }

        private static bool Test_SetBans(bool ban)
        {
            return new SetBans(new SetBansRequestData {
                Bans = new List<Ban> {
                    new Ban { Host = "0.0.0.0", Banned = ban },
                    new Ban { Host = "0.1.0.1", Banned = ban }
                }
            }, (string result) => {
                AngryWasp.Logger.Log.Instance.Write("SetBans: Passed");
            }, (RequestError e) => {
                AngryWasp.Logger.Log.Instance.Write(Log_Severity.Error, "SetBans: Failed");
                Environment.Exit(1);
            }, daemonPort).Run(); 
        }
    }
}