namespace Core.Audio
{
	public class AudioParameters
	{
		public bool Loop;
		public float Volume;
		public float FadeTime;

		public AudioParameters(bool loop, float volume, float fadeTime)
		{
			Loop = loop;
			Volume = volume;
			FadeTime = fadeTime;
		}
	}
}