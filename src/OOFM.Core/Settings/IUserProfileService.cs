namespace OOFM.Core.Settings;

public interface IUserProfileService
{
    UserProfile CurrentUserProfile { get; }

    void LoadUserProfile();
    void SaveUserProfile();
}
