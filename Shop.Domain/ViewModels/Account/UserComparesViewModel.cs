﻿using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Account
{
    public class UserComparesViewModel:BasePaging
    {
        public List<UserCompare> UserCompares { get; set; }

        #region methods
        public UserComparesViewModel SetCompares(List<UserCompare> userCompares)
        {
            this.UserCompares = userCompares;
            return this;
        }

        public UserComparesViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntityCount = paging.AllEntityCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.CountForShowAfterAndBefor = paging.CountForShowAfterAndBefor;
            this.SkipEntitiy = paging.SkipEntitiy;
            this.PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }
}
