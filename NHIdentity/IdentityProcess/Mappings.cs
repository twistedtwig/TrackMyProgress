using System;
using System.Collections.Generic;
using IdentityModelEntities;
using IdentityModels;

namespace IdentityProcess
{
    public class Mappings
    {

        public static ApplicationUser Map(ApplicationUserEntity model)
        {
            if (model == null) return null;

            var user = new ApplicationUser();
            user.Id = model.Id;
            user.UserName = model.UserName;
            user.SecurityStamp = model.SecurityStamp;
            user.PasswordHash = model.PasswordHash;

            var claims = new List<IdentityUserClaim>();
            foreach (var claim in model.Claims)
            {
                claims.Add(Map(claim, user));
            }
            user.Claims = claims;

            var logins = new List<IdentityUserLogin>();
            foreach (var login in model.Logins)
            {
                logins.Add(Map(login, user));
            }
            user.Logins = logins;


            var roles = new List<IdentityUserRole>();
            foreach (var role in model.Roles)
            {
                var newRole = new IdentityUserRole();
                newRole.Id = role.Id;
                newRole.RoleId = role.RoleId;
                newRole.UserId = model.Id;
                newRole.User = user;

                roles.Add(newRole);
            }
            user.Roles = roles;

            return user;
        }

        public static ApplicationUserEntity Map(ApplicationUser user)
        {
            if (user == null) return null;

            var userEntity = new ApplicationUserEntity();
            userEntity.Id = user.Id;
            userEntity.PasswordHash = user.PasswordHash;
            userEntity.SecurityStamp = user.SecurityStamp;
            userEntity.UserName = user.UserName;

            var claims = new List<IdentityUserClaimEntity>();
            foreach (var claim in user.Claims)
            {
                claims.Add(Map(claim));
            }
            userEntity.Claims = claims;

            var logins = new List<IdentityUserLoginEntity>();
            foreach (var login in user.Logins)
            {
                logins.Add(Map(login));
            }
            userEntity.Logins = logins;


            var roles = new List<IdentityUserRoleEntity>();
            foreach (var role in user.Roles)
            {
                var newRole = new IdentityUserRoleEntity();
                newRole.Id = role.Id;
                newRole.RoleId = role.RoleId;
                newRole.UserId = user.Id;

                roles.Add(newRole);
            }
            userEntity.Roles = roles;

            return userEntity;
        }


        public static IdentityUserClaim Map(IdentityUserClaimEntity claimEntity, IdentityUser user)
        {
            if (claimEntity == null) return null;

            var claim = new IdentityUserClaim();
            claim.Id = claimEntity.Id;
            claim.ClaimType = claimEntity.ClaimType;
            claim.ClaimValue = claimEntity.ClaimValue;

            if (user != null)
            {
                claim.User = user;
            }

            return claim;
        }

        public static IdentityUserClaimEntity Map(IdentityUserClaim claim)
        {
            if (claim == null) return null;

            var claimEntity = new IdentityUserClaimEntity();
            claimEntity.Id = claim.Id;
            claimEntity.ClaimType = claim.ClaimType;
            claimEntity.ClaimValue = claim.ClaimValue;
            claimEntity.UserId = claim.User.Id;

            return claimEntity;
        }



        public static IdentityUserLogin Map(IdentityUserLoginEntity loginEntity, IdentityUser user)
        {
            if (loginEntity == null) return null;

            var login = new IdentityUserLogin();
            login.Id = loginEntity.Id;
            login.LoginProvider = loginEntity.LoginProvider;
            login.ProviderKey = loginEntity.ProviderKey;
            login.UserId = user.Id;
            login.User = user;

            return login;
        }

        public static IdentityUserLoginEntity Map(IdentityUserLogin login)
        {
            if (login == null) return null;

            var loginEntity = new IdentityUserLoginEntity();
            loginEntity.Id = loginEntity.Id;
            loginEntity.LoginProvider = loginEntity.LoginProvider;
            loginEntity.ProviderKey = loginEntity.ProviderKey;
            loginEntity.UserId = login.User.Id;

            return loginEntity;
        }


        public static IdentityUserRole Map(IdentityUserRoleEntity roleEntity, IdentityUser user, IdentityRole r)
        {
            if (roleEntity == null) return null;

            var role = new IdentityUserRole();
            role.Id = roleEntity.Id;
            role.Role = r;
            role.RoleId = roleEntity.RoleId;
            role.UserId = roleEntity.UserId;
            role.User = user;

            return role;
        }

        public static IdentityUserRoleEntity Map(IdentityUserRole role)
        {
            if (role == null) return null;

            var roleEntity = new IdentityUserRoleEntity();
            roleEntity.Id = role.Id;            
            roleEntity.UserId = role.User.Id;

            if (!string.IsNullOrWhiteSpace(role.Role.Id))
            {
                roleEntity.RoleId = Guid.Parse(role.Role.Id);
            }

            return roleEntity;
        }

        public static IdentityRole Map(IdentityRoleEntity roleEntity)
        {
            if (roleEntity == null) return null;

            var role = new IdentityRole();
            role.Id = roleEntity.Id.ToString();
            role.Name = roleEntity.Name;

            return role;
        }
    }
}
