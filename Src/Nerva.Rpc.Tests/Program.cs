using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AngryWasp.Helpers;
using AngryWasp.Logger;
using Nerva.Rpc.Wallet;

namespace Nerva.Rpc.Tests
{
    public class Program
    {
        private static string walletName = "mywallet";
        private static string password = "password";
        private static uint port = 18566;

        [STAThread]
        public static void Main(string[] args)
        {
            Log.CreateInstance(true);
            CommandLineParser cmd = CommandLineParser.Parse(args);

            if (cmd["test"] == null)
            {
                Log.Instance.Write("No tests selected");
                return;
            }

            if (cmd["port"] != null)
                port = uint.Parse(cmd["port"].Value);
            
            string test = cmd["test"].Value;

            //Test_CreateWallet().Wait();
            Test_OpenWallet().Wait();
            Test_QueryKey().Wait();
            Test_CreateAccount().Wait();
            Test_GetAccounts().Wait();
            Test_GetTransfers().Wait();
            Test_StopWallet().Wait();
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
                Environment.Exit(1);
            }, port).Run();  
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
            }, port).Run();  
        }

        public static Task Test_StopWallet()
        {
            return new StopWallet((string result) => {
                Log.Instance.Write("StopWallet: Passed");
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "StopWallet: Failed");
                Environment.Exit(1);
            }, port).Run();  
        }

        public static Task Test_GetAccounts()
        {
            return new GetAccounts((GetAccountsResponseData result) => {
                Log.Instance.Write("GetAccounts: Passed, {0} XNV", result.TotalBalance);
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "GetAccounts: Failed");
                Environment.Exit(1);
            }, port).Run();  
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
            }, port).Run();  
        }

        public static Task Test_QueryKey()
        {
            return new QueryKey(new QueryKeyRequestData {
                KeyType = Key_Type.All_Keys.ToString().ToLower()
            }, (QueryKeyResponseData result) => {
                Log.Instance.Write("QueryKey: Passed, {0}", result.PublicViewKey);
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "QueryKery: Failed");
            }, port).Run();  
        }

        public static Task Test_CreateAccount()
        {
            return new CreateAccount(new CreateAccountRequestData {
                Label = "New Account"
            }, (CreateAccountResponseData result) => {
                Log.Instance.Write("CreateAccount: Passed");
            }, (RequestError e) => {
                Log.Instance.Write(Log_Severity.Error, "CreateAccount: Failed");
            }, port).Run();  
        }
    }
}