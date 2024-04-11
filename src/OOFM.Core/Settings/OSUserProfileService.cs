using System.Text.Json;

namespace OOFM.Core.Settings;

public class OSUserProfileService : IUserProfileService
{
    public readonly string ProfilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "OOFM", "profile.json"
    );

    private UserProfile? _currentUserProfile;
    public UserProfile CurrentUserProfile
    {
        get => _currentUserProfile ??= new UserProfile();
        private set => _currentUserProfile = value;
    }

    public void LoadUserProfile()
    {
        if (File.Exists(ProfilePath))
        {
            using (var fs = File.OpenRead(ProfilePath))
            {
                CurrentUserProfile = JsonSerializer.Deserialize<UserProfile>(fs, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new();
            }
        }
        else
        {
            CurrentUserProfile = new UserProfile();
        }
    }

    public void SaveUserProfile()
    {
        EnsureProfileDirectoryExists();

        string jsonContent = JsonSerializer.Serialize(CurrentUserProfile ?? new(), new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(ProfilePath, jsonContent);
    }

    private void EnsureProfileDirectoryExists()
    {
        string? profileDirectory = Path.GetDirectoryName(ProfilePath);

        if (!Directory.Exists(profileDirectory))
        {
            Directory.CreateDirectory(profileDirectory!);
        }
    }
}
