using System;
using System.Collections.Generic;
using GamepadMapper.Configuration;
using GamepadMapper.Input;

namespace GamepadMapper.Infrastructure
{
    public class ProfileCollection
    {
        public static ProfileCollection FromRootConfiguration(RootConfiguration config, IProfileFactory factory)
        {
            var profiles = new Dictionary<string, Profile>(StringComparer.OrdinalIgnoreCase);
            Profile menuProfile = null;
            foreach (var profileCfg in config.Profiles)
            {
                var profile = factory.CreateProfile(profileCfg);
                if (string.Equals(profile.Name, "menu", StringComparison.OrdinalIgnoreCase))
                {
                    menuProfile = profile;
                }
                else
                {
                    profiles[profile.Name] = profile;
                }
            }

            return new ProfileCollection(profiles, menuProfile);
        }

        public ProfileCollection(IReadOnlyDictionary<string, Profile> profiles, Profile menuProfile)
        {
            Profiles = profiles;
            MenuProfile = menuProfile;
        }

        public IReadOnlyDictionary<string, Profile> Profiles { get; }

        public Profile MenuProfile { get; }
    }
}
