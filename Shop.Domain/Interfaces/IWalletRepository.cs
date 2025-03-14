﻿using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
    public interface IWalletRepository
    {
        Task CreateWallet(UserWallet userWallet);
        Task<UserWallet> GetUserWalletById(long walletId);
        Task SaveChange();
        void UpdateWallet(UserWallet wallet);
        Task<FilterWalletViewModel> FilterWallets(FilterWalletViewModel filter);
        Task<int> GetUserWalletAmount(long userId);
    }
}
