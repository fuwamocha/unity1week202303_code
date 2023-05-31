using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
namespace Unity1week202303.System
{
    public class NovelManager : MonoBehaviour
    {
        private const char StartDialogueSeparate = '「';
        private const char EndDialogueSeparate = '」';
        private const char PageSeparate = '&';
        private const char CommandSeparate = '!';
        private const char CommandSeparateParam = '=';
        private const string CharacterImageCommand = "charaimg";
        private const string SpriteCommand = "_sprite";
        private const string SizeCommand = "_size";
        private const string PositionCommand = "_pos";
        private const string CharacterImagePrefabPath = "CharacterImage";
        private const string CharacterSpritePath = "InNovel/";

        [SerializeField] private Transform charactersTransform;
        [SerializeField] private GameObject printerObject;
        [SerializeField] private GameObject continuePanelObject;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI mainText;
        private readonly List<Image> _charaImageList = new();
        private bool _isEnd;
        private bool _isTexting;
        private AsyncOperationHandle<GameObject> _opCharacterGameObject;
        private TextAsset _opTextAsset;
        private Queue<string> _pageQueue;
        private string _text;
        private string _textFilePath;

        internal void SetTextFilePath(string textFilePath)
        {
            _textFilePath = textFilePath;
        }

        private static string LoadTextFile(string fileName)
        {
            return Resources.Load<TextAsset>(fileName).text.Replace("\n", "&").Replace("\r", "");
        }

        internal void Init()
        {
            _text = LoadTextFile(_textFilePath);
            _pageQueue = SeparateString(_text, PageSeparate);
            ShowNextPage();

            var clickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
            clickStream.Subscribe(_ => { CheckTexting(); }).AddTo(this);
        }

        private void SetCharacterImage(string characterName, string cmd, string parameter)
        {
            cmd = cmd.Replace(CharacterImageCommand, "");
            characterName = characterName.Substring(characterName.IndexOf('"') + 1,
                characterName.LastIndexOf('"') - characterName.IndexOf('"') - 1);
            var image = _charaImageList.Find(n => n.name == characterName);
            if (image == null)
            {
                image = Instantiate(Resources.Load<GameObject>(CharacterImagePrefabPath).GetComponent<Image>(),
                    charactersTransform);
                image.transform.localScale = Vector3.one * 0.5f;
                image.name = characterName;
                _charaImageList.Add(image);
            }

            ChangeImageParameter(cmd, parameter, image);
        }

        private static Vector3 ParameterToVector3(string parameter)
        {
            string[] ps = parameter.Replace(" ", "").Split(',');
            return new Vector3(float.Parse(ps[0]), float.Parse(ps[1]), float.Parse(ps[2]));
        }

        private static Queue<string> SeparateString(string str, char sep)
        {
            var queue = new Queue<string>();
            foreach (string l in str.Split(sep)) queue.Enqueue(l);
            return queue;
        }

        private bool ShowNextPage()
        {
            if (_pageQueue.Count <= 0) return false;
            ReadLine(_pageQueue.Dequeue());
            return true;
        }

        private void CheckTexting()
        {
            if (_isTexting)
            {
                mainText.DOComplete();
            }
            else
            {
                if (ShowNextPage() || _isEnd) return;

                DisplayContinuePanel();
                _isEnd = true;
            }
        }

        private void DisplayContinuePanel()
        {
            printerObject.SetActive(false);
            continuePanelObject.SetActive(true);
            continuePanelObject.transform.localScale = Vector3.zero;
            continuePanelObject.transform.DOScale(0.6f, 0.2f);
        }

        private void ReadLine(string text)
        {
            if (text[0].Equals(CommandSeparate))
            {
                ReadCommand(text);
                ShowNextPage();
                return;
            }

            string[] ts = text.Split(StartDialogueSeparate);
            nameText.text = ts[0];
            mainText.text = "";
            _isTexting = true;
            mainText.DOText(ts[1].Remove(ts[1].LastIndexOf(EndDialogueSeparate)), 1f, scrambleMode: ScrambleMode.None)
                .OnComplete(() => _isTexting = false);
        }

        private void ReadCommand(string commandLine)
        {
            commandLine = commandLine.Remove(0, 1);
            var cmdQueue = SeparateString(commandLine, CommandSeparate);

            foreach (string[] commands in cmdQueue.Select(cmd => cmd.Split(CommandSeparateParam))
                .Where(commands => commands[0].Contains(CharacterImageCommand)))
            {
                SetCharacterImage(commands[1], commands[0], commands[2]);
            }
        }
        private void ChangeImageParameter(string command, string parameter, Image image)
        {
            parameter = parameter.Substring(parameter.IndexOf('"') + 1,
                parameter.LastIndexOf('"') - parameter.IndexOf('"') - 1);
            switch (command.Replace(" ", ""))
            {
                case SpriteCommand:
                    image.sprite = LoadSprite(parameter);
                    break;
                case SizeCommand:
                    image.transform.localScale = ParameterToVector3(parameter);
                    break;
                case PositionCommand:
                    image.transform.position = ParameterToVector3(parameter);
                    break;
            }
        }

        private Sprite LoadSprite(string characterName)
        {
            return Instantiate(Resources.Load<Sprite>(CharacterSpritePath + characterName));
        }
    }
}
