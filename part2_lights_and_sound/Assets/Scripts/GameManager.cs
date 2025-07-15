using System.Collections;
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

  [Header("Audio Setup")]
  [SerializeField] private float duration = 0.2f;
  [SerializeField] private AudioSource audioSource;
  
  enum GameMode {
    None,
    Menu,
    Listening,
    Playing
  }

  private GameMode gameMode = GameMode.None;

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

    // Start in the menu game mode, with flashing lights and no sound.
    gameMode = GameMode.Menu;
    StartCoroutine(MenuTileAnimation());
  }

  private IEnumerator MenuTileAnimation() {
    while (gameMode == GameMode.Menu) {
      // Light a random tile.
      yield return FlashTile(Random.Range(0, numTiles));
      // Wait before flashing the next one.
      yield return new WaitForSeconds(duration);
    }
  }

  private IEnumerator FlashTile(int index) {
    tile[index].TurnOn();
    yield return new WaitForSeconds(duration);
    tile[index].TurnOff();
  }

  public void PlayLightAndTone(int index) {
    StartCoroutine(FlashTile(index));
    PlayTone(index);
  }

  private void PlayTone(int index) {
    // Adjust pitch to create unique sound for each tile.
    if (numTiles > 1) {
      audioSource.pitch = Mathf.Lerp(0.5f, 2.0f, index / (numTiles - 1f));
    }

    // Schedule the tone to play.
    double currentTime = AudioSettings.dspTime;
    audioSource.PlayScheduled(currentTime);
    audioSource.SetScheduledEndTime(currentTime + duration);
  }
}

