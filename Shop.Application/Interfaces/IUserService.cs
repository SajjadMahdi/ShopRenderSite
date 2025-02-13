using Microsoft.AspNetCore.Http;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
    public interface IUserService
    {
        #region account
        Task<RegisterUserResult> RegisteUser(RegisterUserViewModel register);
        Task<LoginUserResult> LoginUser(LoginUserViewModel login);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task<ActiveAccountResult> ActiveAccount(ActiveAccountViewModel activeAccount);
        Task<User> GetUserById(long userId);
        bool CheckPermission(long permissionId, string phoneNumber);
        #endregion

        #region profile
        Task<EditUserProfileViewModel> GetEditUserProfile(long userId);
        Task<EditUserProfileResult> EditProfile(long userId, IFormFile userAvatar, EditUserProfileViewModel editUserProfile);
        Task<ChangePasswordResult> ChangePassword(long userId, ChangePasswordViewModel changePassword);
        Task<bool> AddProductToFavoriet(long productId, long userId);
        Task<bool> AddProductToCompare(long productId, long userId);
        Task<List<UserCompare>> GetUserCompares(long userId);
        Task<int> UserFavoritCount(long userId);
        Task<List<UserFavorite>> GetUserFavorites(long userId);
        Task<bool> RemoveAllUserCompare(long userId);
        Task<bool> RemoveUserCompare(long id);

        Task<UserComparesViewModel> UserCompares(UserComparesViewModel userCompares);
        Task<UserFavoritsViewModel> UserFavorits(UserFavoritsViewModel userFavorits);

        #endregion

        #region admin
        Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter);
        Task<EditUserFromAdmin> GetEditUserFromAdmin(long userId);
        Task<EditUserFromAdminResult> EditUserFromAdmin(EditUserFromAdmin editUser);
        Task<CreateOrEditRoleViewModel> GetEditRoleById(long roleId);
        Task<CreateOrEditRoleResult> CreateOrEditRole(CreateOrEditRoleViewModel createOrEditRole);
        Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filter);
        Task<List<Permission>> GetAllActivePermission();
        Task<List<Role>> GetAllActiveRoles();

        #endregion


    }
}
