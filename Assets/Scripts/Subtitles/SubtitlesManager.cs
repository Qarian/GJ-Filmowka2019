﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(StreamVideo))]
public class SubtitlesManager : MonoBehaviour
{
	[SerializeField]
	GameObject subtitlesPrefab = default;

	[Space]
	public List<string> subtitles;
	[SerializeField]
	List<AudioClip> subtitlesClip = default;
	[SerializeField]
	List<int> extraOffset = default;

	int nextId = 0;

	SubtitleScript subtitleScript;
	StreamVideo streamVideoScript;

	public static SubtitlesManager singleton;

	private void Awake()
	{
		singleton = this;
	}

	private void Start()
	{
		if (subtitles.Count != subtitlesClip.Count)
			Debug.LogError("Rożna liczba napisów i plików dźwiękowych", gameObject);

		streamVideoScript = GetComponent<StreamVideo>();
		subtitleScript = Instantiate(subtitlesPrefab).GetComponent<SubtitleScript>();

		streamVideoScript.Begin(subtitleScript.background);
		subtitleScript.Begin(streamVideoScript);
	}

	public void ShowNextSubtitles()
	{
		if (subtitles.Count == nextId)
		{
			Debug.Log("Koniec napisów");
			subtitleScript.ShowSubtitles("Koniec napisów", null, 5f);
			return;
		}

		//disableMovement

		if (subtitlesClip.Count < nextId)
		{
			if (extraOffset.Count < nextId)
				subtitleScript.ShowSubtitles(subtitles[nextId], subtitlesClip[nextId], extraOffset[nextId]);
			else
				subtitleScript.ShowSubtitles(subtitles[nextId], subtitlesClip[nextId]);
		}
		else
		{
			if (extraOffset.Count < nextId)
				subtitleScript.ShowSubtitles(subtitles[nextId], null, extraOffset[nextId]);
			else
				Debug.LogError("Subtititles with no length", gameObject);
		}

		nextId++;
	}
}