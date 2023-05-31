using DG.Tweening;
using MochaLib.Audio;
using Unity1week202303.Score;
using UnityEngine;
using UnityEngine.UI;
namespace Unity1week202303.InGame
{
    public class TutorialPop : MonoBehaviour
    {
        [SerializeField] private GameObject tutorial;
        [SerializeField] private TimeManager timeManager;
        [SerializeField] private Button startButton;
        [SerializeField] private AudioClip seAudioClip;
        internal static bool IsDidTutorial { get; private set; }
        private void Awake()
        {
            startButton.onClick.AddListener(StartGame);
        }

        private void OnEnable()
        {
            tutorial.transform.localScale = new Vector3(0f, 0f, 0f);
            tutorial.transform.DOScale(1f, 0.2f);
        }

        private void StartGame()
        {
            AudioPlayer.Se.Play(seAudioClip, false);
            tutorial.transform.DOScale(0f, 0.2f)
                .OnComplete(() =>
                {
                    IsDidTutorial = true;
                    Destroy(gameObject);
                    timeManager.StartCount();
                });
        }
    }
}
