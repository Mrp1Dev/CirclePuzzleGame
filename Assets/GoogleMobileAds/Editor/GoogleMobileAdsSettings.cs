using UnityEditor;
using UnityEngine;
namespace GoogleMobileAds.Editor
{

    internal class GoogleMobileAdsSettings : ScriptableObject
    {
        private const string MobileAdsSettingsDir = "Assets/GoogleMobileAds";

        private const string MobileAdsSettingsResDir = "Assets/GoogleMobileAds/Resources";

        private const string MobileAdsSettingsFile =
            "Assets/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset";

        private static GoogleMobileAdsSettings instance;

        [SerializeField] private string adMobAndroidAppId = string.Empty;

        [SerializeField] private string adMobIOSAppId = string.Empty;

        [SerializeField] private bool delayAppMeasurementInit;

        public string GoogleMobileAdsAndroidAppId
        {
            get => Instance.adMobAndroidAppId;

            set => Instance.adMobAndroidAppId = value;
        }

        public string GoogleMobileAdsIOSAppId
        {
            get => Instance.adMobIOSAppId;

            set => Instance.adMobIOSAppId = value;
        }

        public bool DelayAppMeasurementInit
        {
            get => Instance.delayAppMeasurementInit;

            set => Instance.delayAppMeasurementInit = value;
        }

        public static GoogleMobileAdsSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    if (!AssetDatabase.IsValidFolder(MobileAdsSettingsDir))
                        AssetDatabase.CreateFolder("Assets", "GoogleMobileAds");

                    if (!AssetDatabase.IsValidFolder(MobileAdsSettingsResDir))
                        AssetDatabase.CreateFolder(MobileAdsSettingsDir, "Resources");

                    instance = (GoogleMobileAdsSettings) AssetDatabase.LoadAssetAtPath(
                        MobileAdsSettingsFile, typeof(GoogleMobileAdsSettings));

                    if (instance == null)
                    {
                        instance = CreateInstance<GoogleMobileAdsSettings>();
                        AssetDatabase.CreateAsset(instance, MobileAdsSettingsFile);
                    }
                }

                return instance;
            }
        }

        internal void WriteSettingsToFile()
        {
            AssetDatabase.SaveAssets();
        }
    }
}