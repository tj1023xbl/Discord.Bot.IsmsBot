using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Bot.Database.Models;

namespace Discord.Bot.IsmsBot.Helpers;
public class AsciiTableGenerator
{
	private readonly int ISM_WIDTH;
	private readonly int ADDED_BY_WIDTH;
	private readonly int ADDED_ON_WIDTH;

	private readonly string COLUMN_DELIMITER = " | ";
	private readonly string CORNER_DELIMITER = " + ";

	public AsciiTableGenerator(int ismWidth, int addedByWidth, int addedOnWidth)
	{
		ISM_WIDTH = ismWidth;
		ADDED_BY_WIDTH = addedByWidth;
		ADDED_ON_WIDTH = addedOnWidth;
	}


	public async Task<MemoryStream> CreateTableAsync(List<Saying> isms)
	{
		MemoryStream stream = new();
		StreamWriter writer = new StreamWriter(stream);
		{
			// CREATE THE HEADERS
			string headers = CreateHeaders();
			await writer.WriteAsync(headers);
			foreach (Saying ism in isms)
			{
				string row = CreateRow(ism);
				await writer.WriteAsync(row);
			}
			writer.Flush();
			stream.Seek(0, 0);
			return stream;
		}

	}

	protected static string GetNCharacters(char character, int x)
	{
		List<char> chars = new(x);
		for (int i = 0; i < x; i++) chars.Add(character);
		return string.Join("", chars);
	}

	protected static string Center(string phrase, int maxWidth)
	{
		int start = (maxWidth - phrase.Length) / 2;
		int end = start + phrase.Length;
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < start; i++) sb.Append(' ');
		sb.Append(phrase);
		for (int i = end; i < maxWidth; i++) sb.Append(' ');
		return sb.ToString();
	}

	protected static string Justify(string phrase, int maxWidth, bool leftJustify = true)
	{
		if (phrase.Length > maxWidth)
			throw new InvalidOperationException($"{nameof(phrase)} is longer than {maxWidth} characters");

		StringBuilder sb = new();
		if (leftJustify)
		{
			sb.Append(phrase);
			sb.Append(GetNCharacters(' ', maxWidth - phrase.Length));
		}
		else
		{
			sb.Append(GetNCharacters(' ', maxWidth - phrase.Length));
			sb.Append(phrase);
		}
		return sb.ToString();
	}

	protected string HorizontalBorder()
	{
		StringBuilder sb = new();
		sb.Append(CORNER_DELIMITER);
		sb.Append(AsciiTableGenerator.GetNCharacters('-', ISM_WIDTH));
		sb.Append(CORNER_DELIMITER);
		sb.Append(AsciiTableGenerator.GetNCharacters('-', ADDED_BY_WIDTH));
		sb.Append(CORNER_DELIMITER);
		sb.Append(AsciiTableGenerator.GetNCharacters('-', ADDED_ON_WIDTH));
		sb.Append(CORNER_DELIMITER);
		return sb.ToString();
	}

	protected string CreateHeaders()
	{
		StringBuilder sb = new();
		sb.AppendLine(HorizontalBorder());
		sb.Append(COLUMN_DELIMITER);
		sb.Append(Center("Ism", ISM_WIDTH));
		sb.Append(COLUMN_DELIMITER);
		sb.Append(Center("Added By", ADDED_BY_WIDTH));
		sb.Append(COLUMN_DELIMITER);
		sb.Append(Center("Added On", ADDED_ON_WIDTH));
		sb.AppendLine(COLUMN_DELIMITER);
		sb.AppendLine(HorizontalBorder());
		return sb.ToString();
	}

	protected string CreateRow(Saying saying)
	{
		StringBuilder sb = new();
		List<string> ismLines = new();
		SplitLines(ismLines, saying.IsmSaying, ISM_WIDTH);

		List<string> adderLines = new();
		SplitLines(adderLines, saying.IsmRecorder, ADDED_BY_WIDTH);

		List<string> dateLines = new();
		SplitLines(dateLines, saying.DateCreated.ToString("g"), ADDED_ON_WIDTH);

		int max = (new int[] { ismLines.Count, adderLines.Count, dateLines.Count }).Max();

		for (int i = 0; i < max; i++)
		{
			sb.Append(COLUMN_DELIMITER);
			if (i <= ismLines.Count - 1)
				sb.Append(Justify(ismLines[i], ISM_WIDTH));
			else
				sb.Append(GetNCharacters(' ', ISM_WIDTH));

			sb.Append(COLUMN_DELIMITER);
			if (i <= adderLines.Count - 1)
				sb.Append(Justify(adderLines[i], ADDED_BY_WIDTH));
			else
				sb.Append(GetNCharacters(' ', ADDED_BY_WIDTH));

			sb.Append(COLUMN_DELIMITER);
			if (i <= dateLines.Count - 1)
				sb.Append(Justify(dateLines[i], ADDED_ON_WIDTH, leftJustify: false));
			else
				sb.Append(GetNCharacters(' ', ADDED_ON_WIDTH));

			sb.AppendLine(COLUMN_DELIMITER);
		}

		sb.AppendLine(HorizontalBorder());

		return sb.ToString();

	}

	protected static List<string> SplitLines(List<string> lines, string line, int width)
	{
		if (line.Length <= width)
		{
			lines.Add(line);
			return lines;
		}

		string[] words = line.Split(' ');
		SplitLines(lines, string.Join(" ", words.Take(words.Length / 2)), width);
		SplitLines(lines, string.Join(" ", words.Skip(words.Length / 2)), width);

		return lines;
	}


}