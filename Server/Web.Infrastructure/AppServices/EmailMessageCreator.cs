using CoreEngine.Model.Common;

namespace Student.Infrastructure.AppServices
{
    class EmailMessageCreator
    {
        internal static string CreatePasswordRecovery()
        {
            return string.Format("We have recently received a password " +
                "recovery request from the admin panel. " +
                "\nIf this was not intentional, please contact admin " +
                "as soon as possible");
        }

        internal static string CreateInvitation(string mobile, string rollNo)
        {
            return string.Format("You have been successfully registered for IIT, DU." +
                "\nPlease download the application from  " +
                AppConstants.BaseUrl + "api/files/index?id=iit.apk " +
                 "And then follow the registration process" +
                 "\nMobile No : " + mobile +
                 "\nRoll No   : " + rollNo);
        }
    }
}
