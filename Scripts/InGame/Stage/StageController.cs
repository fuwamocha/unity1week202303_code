using MochaLib.Audio;
using MochaLib.Scene;
using UniRx;
using Unity1week202303.InGame.Tile;
using Unity1week202303.Player;
using Unity1week202303.Score;
using UnityEngine;
using UnityEngine.UI;
namespace InGame
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] private AudioClip epilogueAudioClip;
        [SerializeField] private AudioClip seAudioClip;
        [SerializeField] private CanvasGroup _fadeInCanvas;
        [SerializeField] private CanvasGroup _fadeOutCanvas;
        [SerializeField] private StageCreate stageCreate;
        [SerializeField] private Button shuffleButton;
        [SerializeField] private TimeManager timeManager;
        [SerializeField] private GameObject boyObject;
        [SerializeField] private GameObject girlObject;
        private Player player;
        internal static int Stage { get; private set; } = 1;

        private void Awake()
        {
            shuffleButton.onClick.AddListener(Shuffle);
        }
        private void OnDisable()
        {
            player.OnClear -= OnClearPuzzle;
        }

        internal void Initialize()
        {
            Stage = 1;
            stageCreate.Init();
        }
        private void Shuffle()
        {
            stageCreate.Shuffle();
            AudioPlayer.Se.Play(seAudioClip, false);
        }

        internal void InstantiateCharacters(Transform gridTilesTransform)
        {
            InstantiateCharacter(girlObject, 1, gridTilesTransform);
            player = InstantiateCharacter(boyObject, -1, gridTilesTransform).GetComponent<Player>();
            player.OnClear += OnClearPuzzle;
        }

        private static GameObject InstantiateCharacter(GameObject characterObject, int position,
            Transform gridTilesTransform)
        {
            var character = Instantiate(characterObject, gridTilesTransform);
            float point = 0.5f * Stage + 1.5f;
            character.transform.localPosition =
                new Vector3(105 * point * position, 105 * point * position);
            return character;
        }

        private void OnClearPuzzle()
        {
            player.OnClear -= OnClearPuzzle;
            if (Stage >= StageCreate.StageRow.Length - 1)
                // if (Stage >= 1)
            {
                Complete();
                return;
            }

            Stage++;
            stageCreate.Init();
        }

        private void Complete()
        {
            timeManager.StopCount();
            var _sceneTransition = new SceneTransition();
            _sceneTransition.CrossFade(_fadeOutCanvas, _fadeInCanvas, 0.5f).Subscribe(_ =>
            {
                AudioPlayer.Bgm.Play(epilogueAudioClip, true);
            });
        }
    }
}
