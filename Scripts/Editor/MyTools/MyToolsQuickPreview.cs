using UnityEditor;
using UnityEngine;

namespace MyTools
{
	static class QuickPreview
	{
		private static AudioSource audioSource;
		private static GameObject audioSourceObject;
		private static AudioClip lastSelectedClip;

		// Initialize the AudioSource and set up the selection change detection
		[InitializeOnLoadMethod]
		static void Initialize()
		{
			// Subscribe to the selection changed event
			Selection.selectionChanged += OnSelectionChanged;
		}

		private static void OnSelectionChanged()
		{
			// Get the selected object
			Object selectedObject = Selection.activeObject;

			// Check if the selected object is an AudioClip
			if (selectedObject is AudioClip selectedClip)
			{
				// Stop the currently playing audio if a different clip is selected
				if (audioSource && audioSource.isPlaying && lastSelectedClip != selectedClip)
				{
					audioSource.Stop();
					DestroyAudioSource();
				}

				// Update the last selected clip
				lastSelectedClip = selectedClip;
			}
		}

		// Define a menu item that can be accessed via the top menu bar
		[MenuItem("My Tools/Quick Preview _SPACE", priority = 1)]
		private static void PlaySelectedAudioClip()
		{
			// Get the selected object
			Object selectedObject = Selection.activeObject;

			// Check if the selected object is an AudioClip
			if (selectedObject is AudioClip selectedClip)
			{
				ToggleAudioClip(selectedClip);
			}
		}

		private static void ToggleAudioClip(AudioClip clip)
		{
			if (audioSource && clip)
			{
				if (audioSource.isPlaying && audioSource.clip == clip)
				{
					// If the same clip is selected and playing, stop it and destroy the AudioSource
					audioSource.Stop();
					DestroyAudioSource();
				}
				else
				{
					// Play the selected clip
					PlayAudioClip(clip);
				}
			}
			else
			{
				// If there is no audioSource, create and play the clip
				PlayAudioClip(clip);
			}
		}

		private static void PlayAudioClip(AudioClip clip)
		{
			// Create the AudioSource GameObject if it doesn't exist
			if (audioSourceObject == null)
			{
				audioSourceObject = new GameObject("EditorAudioSource");
				audioSourceObject.hideFlags = HideFlags.HideAndDontSave;
				audioSource = audioSourceObject.AddComponent<AudioSource>();
			}

			// Assign the clip to the AudioSource and play it
			audioSource.clip = clip;
			audioSource.Play();
		}

		private static void DestroyAudioSource()
		{
			if (audioSourceObject != null)
			{
				Object.DestroyImmediate(audioSourceObject);
				audioSourceObject = null;
				audioSource = null;
			}
		}
	}
}