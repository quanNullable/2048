
public class HistoryData
{
  public int score;
  public int best;
  public int[][] numbers;

  public void UpdateData(int score, int best, MyGrid[][] grids)
  {
    this.score = score;
    this.best = best;
    if (numbers == null)
    {
      numbers = new int[grids.Length][];
    }
    for (int i = 0; i < grids.Length; i++)
    {
      for (int j = 0; j < grids[i].Length; j++)
      {
        if (numbers[i] == null)
        {
          numbers[i] = new int[grids[i].Length];
        }
        numbers[i][j] = grids[i][j].HasNumber() ? grids[i][j].GetNumber().GetNumber() : 0;
      }
    }
  }
}
