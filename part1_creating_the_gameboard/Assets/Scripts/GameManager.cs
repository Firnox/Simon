using UnityEngine;

public class GameManager : MonoBehaviour {

  [Header("Game Setup")]
  [SerializeField] private int numRows = 3;
  [SerializeField] private int numCols = 4;
  private int numTiles;
  private Tile[] tile;

  [Header("Game Objects")]
  [SerializeField] private Tile tilePrefab;
  [SerializeField] private Transform gameArea;

  void Start() {
    // numTiles is global as we'll use it in lots of places.
    numTiles = numRows * numCols;
    tile = new Tile[numTiles];

    // Create the grid of tiles.
    for (int row = 0; row < numRows; row++) {
      for (int col = 0; col < numCols; col++) {
        int index = (row * numCols) + col;

        // Instantiate the tile objects.
        tile[index] = Instantiate(tilePrefab, gameArea);
        tile[index].Init(this, index, Color.HSVToRGB((float)index / numTiles, 0.8f, 0.9f));

        // Center the tiles in the game area.
        float rowStart = (numRows / 2f) - 0.5f;
        float colStart = (-numCols / 2f) + 0.5f;
        tile[index].transform.localPosition = new Vector3(colStart + col, rowStart - row, 0f);
      }
    }

    // Scale the tiles to fit our vertical space (6 units)
    // (If there are too many cols they'll go off the edge).
    float scale = 6f / numRows;
    gameArea.localScale = Vector3.one * scale;
  }
}

