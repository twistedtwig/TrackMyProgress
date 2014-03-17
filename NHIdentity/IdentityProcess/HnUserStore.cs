using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModelEntities;
using IdentityModels;
using Microsoft.AspNet.Identity;
using RepositoryInterfaces;

namespace IdentityProcess
{
    public class HnUserStore<TUser> : IUserPasswordStore<TUser>, IUserSecurityStampStore<TUser>, IUserLoginStore<TUser>,  IUserRoleStore<TUser> where TUser : ApplicationUser
    {
        private IRepo _repository;

        public HnUserStore(IRepo repository)
        {
            _repository = repository;
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task CreateAsync(TUser user)
        {
            using (var uow = _repository.CreateUnitOfWork())
            {
                uow.Add(Mappings.Map(user));
            }

            return Task.FromResult(0);
        }

        public Task DeleteAsync(TUser user)
        {
            //            _context.Users.Remove(user);

            return Task.FromResult(0);
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            return Task.FromResult(Mappings.Map(_repository.First<ApplicationUserEntity>(x => x.Id == userId)) as TUser);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(Mappings.Map(_repository.First<ApplicationUserEntity>(u => u.UserName == userName)) as TUser);
        }

        public Task UpdateAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            using (var uow = _repository.CreateUnitOfWork())
            {
                var userEntity = uow.First<IdentityUserEntity>(x => x.Id.ToString() == user.Id);
                if (userEntity != null)
                {
                    userEntity.PasswordHash = user.PasswordHash;
                    userEntity.SecurityStamp = user.SecurityStamp;
                    userEntity.UserName = user.UserName;
                }
                uow.Update(userEntity);
            }

            return Task.FromResult(0);
        }

        public void Dispose()
        {

        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            if (user == null) throw new ArgumentNullException("user");

            //TODO not sure why this is not saving it!
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (login == null) throw new ArgumentNullException("login");

            using (var uow = _repository.CreateUnitOfWork())
            {
                var userEntity = uow.First<ApplicationUserEntity>(u => u.Id.ToString() == user.Id);
                if (userEntity != null)
                {
                    var loginEntity = new IdentityUserLoginEntity
                    {
                        LoginProvider = login.LoginProvider,
                        ProviderKey = login.ProviderKey,
                        UserId = userEntity.Id
                    };

                    userEntity.Logins.Add(loginEntity);
                    uow.Update(userEntity);
                }
            }

            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            //            var log = user.Logins.FirstOrDefault(l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey);
            //            if (log != null)
            //            {
            //                user.Logins.Remove(log);
            //                _context.Users.Attach(user);
            //                _context.SaveChanges();
            //            }

            return Task.FromResult(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var result = (IList<UserLoginInfo>)new List<UserLoginInfo>();

            var u = _repository.First<ApplicationUserEntity>(x => x.Id == user.Id);
            if (u != null)
            {
                foreach (IdentityUserLoginEntity login in u.Logins)
                {
                    result.Add(new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }
            }

            return Task.FromResult(result);
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null) throw new ArgumentNullException("login");

            var userEntity = _repository.First<ApplicationUserEntity>(u => u.Logins.Any(l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey));
            return Task.FromResult(Mappings.Map(userEntity) as TUser);
        }

        public Task AddToRoleAsync(TUser user, string role)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentNullException("role");
            
            using (var uow = _repository.CreateUnitOfWork())
            {
                var userEntity = uow.First<ApplicationUserEntity>(u => u.Id.ToString() == user.Id);
                if (userEntity != null)
                {
                    var dbrole = uow.First<IdentityRoleEntity>(r => r.Name == role);
                    if (dbrole == null)
                    {
                        dbrole = new IdentityRoleEntity(role);
                        uow.Add(dbrole);
                    }

                    var roleEntity = new IdentityUserRoleEntity
                    {
                        RoleId =  dbrole.Id,
                        UserId = user.Id
                    };
                    
                    userEntity.Roles.Add(roleEntity);
                    uow.Update(userEntity);
                }
            }

            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            return Task.FromResult(0);



            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var result = (IList<string>)new List<string>();
            var u = _repository.First<ApplicationUserEntity>(x => x.Id == user.Id);

            if (u != null)
            {
                var roles = _repository.All<IdentityRoleEntity>();

                foreach (IdentityUserRoleEntity roleEntity in u.Roles)
                {
                    var role = roles.FirstOrDefault(r => r.Id.Equals(roleEntity.RoleId));
                    if (role != null)
                    {
                        result.Add(role.Name);
                    }
                }
            }

            return Task.FromResult(result);
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentNullException("role");
            
            var u = _repository.First<ApplicationUserEntity>(x => x.Id == user.Id);

            if (u != null)
            {
                var dbrole = _repository.First<IdentityRoleEntity>(r => r.Name == role);
                if (dbrole == null) Task.FromResult(false);

                return Task.FromResult(u.Roles.Any(x => x.RoleId == dbrole.Id));
            }

            return Task.FromResult(false);
        }
    }
}

