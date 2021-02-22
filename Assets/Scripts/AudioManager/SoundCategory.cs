using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	[System.Serializable]
	public class SoundCategory
	{
		public string categoryName = "New category";
		[Range(0f, 1f)]
		public float volume = 1f;

		[Space]

		public bool autoPlay = false;
		public float autoPlayOffset = 10f;

		[Space]

		public List<Sound> sounds = new List<Sound>();
		
		[HideInInspector]
		public int currentSnd = -1;

		///<summary>Sets up all sounds. It should be done only once for AudioManager instance.</summary>
		public void SetUpSounds(Transform parent, float startingVolume, bool loadVolume)
		{
			if(loadVolume)
				volume = PlayerPrefs.GetFloat("AudioManager.Category." + categoryName + ".Volume", volume);

			for(int i = 0; i < sounds.Count; i++)
				sounds[i].SetUp(parent, startingVolume * volume, loadVolume);
		}

		///<summary>Updates volume of all sounds of this category.</summary>
		public void UpdateVolume(float globalVolume, float newVolume)
		{
			volume = newVolume;

			foreach(Sound snd in sounds)
				snd.UpdateVolume(globalVolume * newVolume);

			PlayerPrefs.SetFloat("AudioManager.Category." + categoryName + ".Volume", volume);
		}

		///<summary>Updates volume of all sounds of this category based on given global volume.</summary>
		public void UpdateGlobalVolume(float globalVolume)
		{
			foreach(Sound snd in sounds)
				snd.UpdateVolume(globalVolume * volume);
		}

		///<summary>Resumes all paused sounds of this category.</summary>
		public void ResumePaused()
		{
			for(int i = 0; i < sounds.Count; i++)
			{
				if(sounds[i].isPaused)
					sounds[i].Resume();
			}
		}

		///<summary>Pauses all sounds of this category.</summary>
		public void PauseAll()
		{
			for(int i = 0; i < sounds.Count; i++)
			{
				sounds[i].Pause();
			}
		}

		///<summary>Stops all sounds of this category.</summary>
		public void StopAll()
		{
			for(int i = 0; i < sounds.Count; i++)
			{
				sounds[i].Stop();
			}
		}
	}
}