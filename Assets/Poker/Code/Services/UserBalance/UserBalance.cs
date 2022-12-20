using Poker.Code.Core.Players;
using Poker.Code.Core.UI.Gameplay.TopPanel;
using Poker.Code.Services.PersistentProgress;
using Poker.Code.Services.SaveLoad;

namespace Poker.Code.Services.UserBalance
{
    public class UserBalance : IUserBalance
    {
        private readonly IPersistentProgress _persistentProgress;
        private readonly ISaveLoad _saveLoadService;
        private readonly int _startChips;

        private Player _user;
        private UserInfoTexts _infoTexts;

        public UserBalance(IPersistentProgress persistentProgress, ISaveLoad saveLoadService, int startChips)
        {
            _persistentProgress = persistentProgress;
            _saveLoadService = saveLoadService;
            _startChips = startChips;
        }

        public void SetUser(Player user, UserInfoTexts infoTexts)
        {
            _infoTexts = infoTexts;
            _user = user;
            SubscribeUser();
        }

        public void ResetUserBalance()
        {
            _persistentProgress.Progress.Balance.Chips = _startChips;
            _saveLoadService.SaveProgress();

            if (_user != null) _user.Balance = _startChips;
        }

        public void ClearGameplayDependencies()
        {
            _user = null;
            _infoTexts = null;
        }

        public void SaveBalance()
        {
            _persistentProgress.Progress.Balance.Chips = _user.Balance;
            _infoTexts.SetBalance(_user.Balance);

            DefineRecord(_user.Balance);

            _saveLoadService.SaveProgress();
        }

        public void AddBalance(int chips)
        {
            int balance = _persistentProgress.Progress.Balance.Chips += chips;
            DefineRecord(balance);
            
            _saveLoadService.SaveProgress();
        }

        private void DefineRecord(int balance)
        {
            int record = _persistentProgress.Progress.Balance.Record;

            if (balance > record)
                _persistentProgress.Progress.Balance.Record = balance;
        }

        private void SubscribeUser() =>
            _user.OnBalanceChanged += SaveBalance;

        private void UnsubscribeUser()
        {
            if (_user == null) return;
            _user.OnBalanceChanged -= SaveBalance;
        }

        ~UserBalance() => UnsubscribeUser();
    }
}