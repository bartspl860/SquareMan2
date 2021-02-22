using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Audio
{
	public class AudioManager : MonoBehaviour 
	{
		public static AudioManager instance;

		[SerializeField]
		private bool dontDestroyOnLoad = true;

		[SerializeField][Range(0f, 1f)]
		private float volume = 1f;
		[SerializeField]
		private bool saveVolumeValues = true;

		[Space]

		[SerializeField]
		private List<SoundCategory> soundCategories = new List<SoundCategory>();

		public void PlaySound(string _name)
		{
			for(int i = 0; i < soundCategories.Count; i++)
			{
				foreach(Sound _snd in soundCategories[i].sounds)
				{
					if(_snd.name == _name)
					{
						_snd.Play();
						return;
					}
				}
			}
		}

		public void PlaySound(string _name, bool _fadeIn = false)
		{
			for(int i = 0; i < soundCategories.Count; i++)
			{
				foreach(Sound _snd in soundCategories[i].sounds)
				{
					if(_snd.name == _name)
					{
						_snd.Play(_fadeIn);
						return;
					}
				}
			}
		}

		public void PauseSound(string name)
		{
			for(int i = 0; i < soundCategories.Count; i++)
			{
				foreach(Sound _snd in soundCategories[i].sounds)
				{
					if(_snd.name == name)
					{
						_snd.Pause();
						return;
					}
				}
			}
		}

		public void StopSound(string _name)
		{
			for(int i = 0; i < soundCategories.Count; i++)
			{
				foreach(Sound _snd in soundCategories[i].sounds)
				{
					if(_snd.name == _name)
					{
						_snd.Stop();
						return;
					}
				}
			}
		}

		public void StopAll()
		{
			for(int i = 0; i < soundCategories.Count; i++)
			{
				foreach(Sound _snd in soundCategories[i].sounds)
				{
					_snd.Stop();
				}
			}
		}

		public void SetGlobalVolume(float _volume)
		{
			volume = _volume;
			for(int i = 0; i < soundCategories.Count; i++)
			{
				soundCategories[i].UpdateGlobalVolume(_volume);
			}

			PlayerPrefs.SetFloat("AudioManager.MainVolume", volume);
		}

		public void SetCategoryVolume(string category, float _volume)
		{
			for(int i = 0; i < soundCategories.Count; i++)
			{
				if(category == soundCategories[i].categoryName)
					soundCategories[i].UpdateVolume(volume, _volume);
			}
		}

		public float GetCategoryVolume(string category)
		{
			for(int i = 0; i < soundCategories.Count; i++)
			{
				if(category == soundCategories[i].categoryName)
					return soundCategories[i].volume;
			}

			return 0;
		}

		public void PlayRandomFromCategory(string category, bool fadeIn = false)
		{
			foreach(SoundCategory sndCat in soundCategories)
			{
				if(sndCat.categoryName == category)
				{
					int rand = Random.Range(0, sndCat.sounds.Count);

					sndCat.sounds[rand].Play(fadeIn);
				}
			}
		}

		public void ResumeSoundCategory(string category)
		{
			foreach(SoundCategory sndCat in soundCategories)
			{
				if(sndCat.categoryName == category)
					sndCat.ResumePaused();
			}
		}

		public void PauseSoundCategory(string category)
		{
			foreach(SoundCategory sndCat in soundCategories)
			{
				if(sndCat.categoryName == category)
					sndCat.PauseAll();
			}
		}

		public void StopSoundCategory(string category)
		{
			foreach(SoundCategory sndCat in soundCategories)
			{
				if(sndCat.categoryName == category)
					sndCat.StopAll();
			}
		}

		public float GetSoundClipLength(string sound)
		{
			foreach(SoundCategory sndCat in soundCategories)
			{
				foreach(Sound snd in sndCat.sounds)
				{
					if(snd.name == sound)
						return snd.GetClipLength();
				}
			}
			return 0;
		}

		private void Awake()
		{
			//Check if AudioManager instance exists, and if no - create one, else - delete this AudioManager
			if(instance == null)
				instance = this;
			else
			{
				Destroy(this);
				return;
			}

			if(saveVolumeValues)
				volume = PlayerPrefs.GetFloat("AudioManager.MainVolume", volume);

			foreach(SoundCategory sndCat in soundCategories)
				sndCat.SetUpSounds(this.transform, volume, saveVolumeValues);

			if(dontDestroyOnLoad)
				DontDestroyOnLoad(this);
		}

		void LevelLoaded(Scene s1, Scene s2)
		{
			StopAll();
			StopAllCoroutines();

			foreach(SoundCategory sndCat in soundCategories)
			{
				if(sndCat.autoPlay)
				{
					int rand = Random.Range(0, sndCat.sounds.Count);

					sndCat.sounds[rand].Play();

					StartCoroutine(AutoPlay(sndCat, rand, sndCat.sounds[rand].clip.length + sndCat.autoPlayOffset));
				}
			}
		}

		void Update()
		{
			for(int i = 0; i < soundCategories.Count; i++)
			{
				foreach(Sound _snd in soundCategories[i].sounds)
				{
					_snd.Update(Time.unscaledDeltaTime);
				}
			}
		}

		void OnEnable()
		{
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged += LevelLoaded;
		}

		void OnDisable()
		{
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= LevelLoaded;
		}

		private IEnumerator AutoPlay(SoundCategory category, int previous, float offset)
		{
			yield return new WaitForSeconds(offset);

			int rand = Random.Range(0, category.sounds.Count);
			if(rand == previous)
			{
				rand++;
				if(rand >= category.sounds.Count)
					rand = 0;
			}

			category.sounds[rand].Play();
			//Debug.Log("Playing " + category.sounds[rand].name + ". Current offset: " + offset + ". Previous clip: " + previous);

			StartCoroutine(AutoPlay(category, rand, category.sounds[rand].clip.length + category.autoPlayOffset));
		}
	}
}
