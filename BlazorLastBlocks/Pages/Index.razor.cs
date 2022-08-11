using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json;

namespace BlazorLastBlocks.Pages {
    public partial class Index {
        public System.Timers.Timer LoaderTimer { get; set; } = new System.Timers.Timer();
        public List<BlockWithTransactionHashes> TxList { get; set; } = new List<BlockWithTransactionHashes>();
        public int LoaderMove { get; set; } = 1;
        public int RowCounter { get; set; } = 0;
        System.Timers.Timer RefreshTimer = new System.Timers.Timer();
        public string SelectedBlockHash { get; set; }
        public Transaction SelectedTx { get; set; } = new Transaction();
        Web3 web3 = new Web3("https://ropsten.infura.io/v3/aae73291d79e4e33bea656928edc045c");
        protected override async Task OnInitializedAsync() {
            _ = AddLastBlock();
            StartRefreshTimer();
            StartLoaderTimer();
        }

        public async Task RowClick(string bhash) {
            SelectedBlockHash = bhash;
        }

        public async Task TransactionClick(string txHash) {
            SelectedTx = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(txHash);
        }

        async Task AddLastBlock() {
            var latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var block = await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(latestBlockNumber);

            if (TxList?.Count==0 || TxList.LastOrDefault()?.Number != block.Number) {
                TxList.Add(block);
                RowCounter++;
            }
            StateHasChanged();
        }

        void CheckCounters() {
            if (LoaderMove == 100) {
                LoaderMove = 0;
            }
            if (RowCounter > 70) {
                TxList = new List<BlockWithTransactionHashes>();
            }
        }
        public void StartRefreshTimer() {
            RefreshTimer = new System.Timers.Timer(10000);
            RefreshTimer.Elapsed += RefreshTimer_Elapsed;
            RefreshTimer.Enabled = true;
            RefreshTimer.Start();
        }

        private void RefreshTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e) {
            _ = AddLastBlock();
        }

        public void StartLoaderTimer() {
            LoaderTimer = new System.Timers.Timer(100);
            LoaderTimer.Elapsed += LoaderTimer_Elapsed;
            LoaderTimer.Enabled = true;
            LoaderTimer.Start();
        }

        private void LoaderTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e) {
            LoaderMove++;
            CheckCounters(); 
            StateHasChanged();
        }
    }
}
