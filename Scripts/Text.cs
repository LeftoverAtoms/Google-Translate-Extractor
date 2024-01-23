namespace GTE
{
	public static class Text
	{
		public const string Intro = "Google Translate Extractor (Version {0}) by Adam Calvelage\n";
		public const string URL = "https://translate.google.com/translate_tts?q={0}&tl={1}&client=tw-ob";

		public const string Dialogue =
			"%YAML 1.1\n" +
			"%TAG !u! tag:unity3d.com,2011:\n" +
			"--- !u!114 &11400000\n" +
			"MonoBehaviour:\n" +
			"  m_ObjectHideFlags: 0\n" +
			"  m_CorrespondingSourceObject: {{fileID: 0}}\n" +
			"  m_PrefabInstance: {{fileID: 0}}\n" +
			"  m_PrefabAsset: {{fileID: 0}}\n" +
			"  m_GameObject: {{fileID: 0}}\n" +
			"  m_Enabled: 1\n" +
			"  m_EditorHideFlags: 0\n" +
			"  m_Script: {{fileID: 11500000, guid: 2b0513477a0d25c40830b7e2d061c089, type: 3}}\n" +
			"  m_Name: {0}\n" +
			"  m_EditorClassIdentifier:\n" +
			"  Dialogue:\n" +
			"{1}" +
			"  Language: {2}\n";

		public const string Subtitle =
			"  - Recording: {{fileID: 8300000, guid: {0}, type: 3}}\n" +
			"    Subtitle: {1}\n";

		public const string Sound =
			"fileFormatVersion: 2\n" +
			"guid: {0}\n" +
			"AudioImporter:\n" +
			"  externalObjects: {{}}\n" +
			"  serializedVersion: 6\n" +
			"  defaultSettings:\n" +
			"    loadType: 2\n" +
			"    sampleRateSetting: 0\n" +
			"    sampleRateOverride: 44100\n" +
			"    compressionFormat: 1\n" +
			"    quality: 1\n" +
			"    conversionMode: 0\n" +
			"  platformSettingOverrides: {{}}\n" +
			"  forceToMono: 1\n" +
			"  normalize: 1\n" +
			"  preloadAudioData: 1\n" +
			"  loadInBackground: 0\n" +
			"  ambisonic: 0\n" +
			"  3D: 1\n" +
			"  userData:\n" +
			"  assetBundleName:\n" +
			"  assetBundleVariant:\n";
	}
}