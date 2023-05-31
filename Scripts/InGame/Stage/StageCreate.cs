using System.Collections.Generic;
using System.Linq;
using InGame;
using UnityEngine;
using UnityEngine.UI;
namespace Unity1week202303.InGame.Tile
{
    public class StageCreate : MonoBehaviour
    {
        internal static readonly int[] StageRow = { 0, 5, 6, 7 };
        private static readonly int[] BreakableObjectNum = { 0, 15, 24, 33 };
        private static readonly int[] UnbreakableObjectNum = { 0, 4, 8, 12 };

        [SerializeField] private StageController stageController;
        [SerializeField] private GameObject tile;
        [SerializeField] private GameObject gridTilesContainer;
        private GridLayoutGroup _gridLayout;
        private bool _isFirst = true;
        private TileType[] _stageTiles;
        private Tile[] _tiles;

        private static int GridSize => StageRow[StageController.Stage] * StageRow[StageController.Stage];

        private void Start()
        {
            _gridLayout = gridTilesContainer.GetComponent<GridLayoutGroup>();
            Init();
        }

        internal void Init()
        {
            _isFirst = true;
            _tiles = new Tile[GridSize];
            CreateStage();
        }

        internal void Shuffle()
        {
            CreateStage();
        }

        private void CreateStage()
        {
            DecideStageTileTypes();
            InstantiateTiles();
        }

        private void DecideStageTileTypes()
        {
            _gridLayout.constraintCount = StageRow[StageController.Stage];
            _stageTiles = new TileType[GridSize];

            _stageTiles[0] = TileType.Visited;
            _stageTiles[GridSize - 1] = TileType.Goal;

            var numList = Enumerable.Range(1, GridSize - 2).ToList();
            int noneObjectNum = GridSize - (BreakableObjectNum[StageController.Stage] +
                UnbreakableObjectNum[StageController.Stage] + 2);

            Decide(noneObjectNum, TileType.None, numList);
            Decide(UnbreakableObjectNum[StageController.Stage], TileType.Unbreakable, numList);
            Decide(BreakableObjectNum[StageController.Stage] / 3, TileType.Breakable1, numList);
            Decide(BreakableObjectNum[StageController.Stage] / 3, TileType.Breakable2, numList);
            Decide(BreakableObjectNum[StageController.Stage] / 3, TileType.Breakable3, numList);
        }

        private void Decide(int num, TileType tileType, List<int> numList)
        {
            for (var i = 0; i < num; i++)
            {
                int randomIdx = Random.Range(0, numList.Count);
                _stageTiles[numList[randomIdx]] = tileType;
                numList.RemoveAt(randomIdx);
            }
        }

        private void InstantiateTiles()
        {
            if (_isFirst)
            {
                if (gridTilesContainer.transform.childCount != 0)
                {
                    foreach (Transform child in gridTilesContainer.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }

            for (var i = 0; i < GridSize; i++)
            {
                if (_isFirst)
                {
                    _tiles[i] = Instantiate(tile, gridTilesContainer.transform)
                        .transform.GetChild(0).GetComponent<Tile>();
                }
                else
                {
                    if (_tiles[i].TileType == TileType.Visited) continue;
                }

                _tiles[i].TileType = _stageTiles[i];
            }

            if (!_isFirst) return;
            stageController.InstantiateCharacters(gridTilesContainer.transform);
            _isFirst = false;
        }
    }
}
