namespace MochaLib.Audio
{
    public static class AudioPlayer
    {
        internal static BgmPlayer Bgm;
        internal static SePlayer Se;

        public static void Initialize(BgmPlayer bgm, SePlayer sePlayer)
        {
            Bgm = bgm;
            Se = sePlayer;
            Bgm.SetVolume(0);
            Se.SetVolume(0);
        }
    }
}
