namespace ministryofjusticeDomain.Interfaces.Services
{
	public interface ILoginCheckServices
	{
		string IsUserLoginStill(string userId);
		void UserLoginedIn(string userId, string UserAgent);
		void UserLogOut(string userId);
	}
}
