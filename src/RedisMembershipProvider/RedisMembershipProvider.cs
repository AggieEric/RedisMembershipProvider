using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;
using System.Web.Security;
using RedisMembershipProvider.Core;
using WebMatrix.WebData;

namespace RedisMembershipProvider.Data
{
    public class RedisMembershipProvider : ExtendedMembershipProvider
    {
        public const string UrlKey = "URLKEY";
        public const string Url = "URL";

        private string _providerName;
        private IUserRepository _userRepository;

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override string Name
        {
            get { return _providerName; }
        }

        public override string Description
        {
            get { throw new NotImplementedException(); }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            _providerName = name;

            var urlKeyConfigKey = config.AllKeys.SingleOrDefault(x => x.ToUpperInvariant().Equals(UrlKey));
            var urlConfigKey = config.AllKeys.SingleOrDefault(x => x.ToUpperInvariant().Equals(Url));
            var neitherDefined = urlKeyConfigKey == null && urlConfigKey == null;
            var bothDefined = urlKeyConfigKey != null && urlConfigKey != null;
            if (neitherDefined || bothDefined)
            {
                var message = string.Format(
                    "{0} requires a settings for exactly one of '{1}' or '{2}' in the configuration for the Redis membership provider",
                    typeof (RedisMembershipProvider).Name, UrlKey, Url);
                throw new ApplicationException(message);
            }
            string url;
            if (urlKeyConfigKey != null)
            {
                var urlKey = config[urlKeyConfigKey];
                url = ConfigurationManager.AppSettings[urlKey];
            }
            else
            {
                url = config[urlConfigKey];
            }

            _userRepository = new UserRepository(url);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
                                                             string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email,
                                                  string passwordQuestion, string passwordAnswer, bool isApproved,
                                                  object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = _userRepository.GetByUsername(username);
            if (!user.Password.Equals(oldPassword))
                return false;
            user.Password = newPassword;
            _userRepository.Save(user);
            return true;
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = _userRepository.GetByUsername(username);
            var email = string.Empty;
            var providerUserKey = user.Id;
            return new MembershipUser(_providerName, user.Username, providerUserKey, email, null, null, true, false,
                                      DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue,
                                      DateTime.MinValue);
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                                  out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            return (user != null && user.Password.Equals(password));
        }

        protected override byte[] EncryptPassword(byte[] password)
        {
            throw new NotImplementedException();
        }

        protected override byte[] EncryptPassword(byte[] password,
                                                  MembershipPasswordCompatibilityMode legacyPasswordCompatibilityMode)
        {
            throw new NotImplementedException();
        }

        protected override byte[] DecryptPassword(byte[] encodedPassword)
        {
            throw new NotImplementedException();
        }

        protected override void OnValidatingPassword(ValidatePasswordEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override ICollection<OAuthAccountData> GetAccountsForUser(string userName)
        {
            //TODO:  Replace trivial implementation
            return new OAuthAccountData[0];
        }

        public override string CreateUserAndAccount(string userName, string password, bool requireConfirmation,
                                                    IDictionary<string, object> values)
        {
            var user = new User(userName, password);
            _userRepository.Save(user);
            return user.ConfirmationToken;
        }

        public override string CreateAccount(string userName, string password, bool requireConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override bool ConfirmAccount(string userName, string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override bool ConfirmAccount(string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAccount(string userName)
        {
            throw new NotImplementedException();
        }

        public override string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow)
        {
            throw new NotImplementedException();
        }

        public override int GetUserIdFromPasswordResetToken(string token)
        {
            throw new NotImplementedException();
        }

        public override bool IsConfirmed(string userName)
        {
            throw new NotImplementedException();
        }

        public override bool ResetPasswordWithToken(string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override int GetPasswordFailuresSinceLastSuccess(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetCreateDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetPasswordChangedDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastPasswordFailureDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override bool HasLocalAccount(int userId)
        {
            return (!string.IsNullOrEmpty(_userRepository.GetById(userId).Password));
        }
    }
}