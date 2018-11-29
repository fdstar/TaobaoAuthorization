using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using Abp.Reflection.Extensions;

namespace TaobaoAuthorization.Localization
{
    public static class TaobaoAuthorizationLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Languages.Add(new LanguageInfo("zh-Hans", "简体中文", "famfamfam-flags cn", isDefault: true));
            localizationConfiguration.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flags england"));

            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(TaobaoAuthorizationConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(TaobaoAuthorizationLocalizationConfigurer).GetAssembly(),
                        "TaobaoAuthorization.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}