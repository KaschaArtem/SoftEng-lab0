#pragma warning disable CA1416

using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

class Program
{

    static Dictionary<string, Color> colorsDict = new(StringComparer.OrdinalIgnoreCase)
    {
        { "красн", Color.Red },
        { "ал", Color.Crimson },
        { "багр", Color.DarkRed },
        { "зелен", Color.Green },
        { "изумруд", Color.MediumSeaGreen },
        { "малахит", Color.MediumSeaGreen },
        { "син", Color.Blue },
        { "голуб", Color.LightBlue },
        { "лазур", Color.LightSkyBlue },
        { "ультрамарин", Color.Blue },
        { "желт", Color.Yellow },
        { "золот", Color.Gold },
        { "лимон", Color.LemonChiffon },
        { "бел", Color.White },
        { "черн", Color.Black },
        { "сер", Color.Gray },
        { "фиолетов", Color.Purple },
        { "лилов", Color.Purple },
        { "оранжев", Color.Orange },
        { "коричнев", Color.Brown },
        { "розов", Color.Pink },
        { "бирюз", Color.Turquoise },
    };

    static string[] colorEndings = { "ый", "ая", "ое", "ые", "ий", "яя", "ее", "ие", "ого", "ой", "ому", "ыми", "им", "ой" };

    static string GetText(string path)
    {
        string text = File.ReadAllText(path);
        return text;
    }

    static (List<string>, List<Color>) CountColors(string text)
    {
        var colors = new List<Color>();
        var coloredWords = new List<string>();

        var words = Regex.Matches(text, @"\b[\p{IsCyrillic}a-zA-Z]+\b");

        foreach (Match wordMatch in words)
        {
            string word = wordMatch.Value.ToLower();
            var baseColor = colorsDict.FirstOrDefault(kvp => word.StartsWith(kvp.Key));
            if (!baseColor.Equals(default(KeyValuePair<string, Color>)) && colorEndings.FirstOrDefault(e => word.EndsWith(e)) != null)
            {
                colors.Add(baseColor.Value);
                coloredWords.Add(word);
            }
        }
        return (coloredWords, colors);
    }

    static void DrawColors(List<Color> colors, string outputFile = "colors.png")
    {
        const int cellSize = 20;
        const int gridSize = 20;
        const int width = cellSize * gridSize;
        const int height = cellSize * gridSize;

        using var file = new Bitmap(width, height);
        using var g = Graphics.FromImage(file);

        int totalCells = gridSize * gridSize;

        for (int i = 0; i < totalCells; i++)
        {
            Color color = colors[i % colors.Count];

            int x = (i % gridSize) * cellSize;
            int y = (i / gridSize) * cellSize;

            using var brush = new SolidBrush(color);
            g.FillRectangle(brush, x, y, cellSize, cellSize);
        }

        file.Save(outputFile, ImageFormat.Png);
    }

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var text = GetText("Podarok.txt");
        var arrayColor = CountColors(text);
        DrawColors(arrayColor.Item2);
    }
}