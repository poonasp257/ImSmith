using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeManager : MonoBehaviour {
    [SerializeField] private Slider fxVolume;
    [SerializeField] private Slider musicVolume;

    private void Update() {
        GameData.Instance.FXVolume = fxVolume.value;
        GameData.Instance.MusicVolume = musicVolume.value;
    }
}