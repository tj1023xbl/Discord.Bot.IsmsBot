using System.Text;
using Discord.Bot.Database.Models;
using Discord.Bot.IsmsBot.Helpers;
using Serilog;
namespace Discord.Bot.Tests;

[TestClass]
public class AsciiTableTests : AsciiTableGenerator
{

    private ILogger _logger;

    public AsciiTableTests() : base(60, 20, 20) { }

    [TestInitialize]
    public void Init()
    {
        _logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{Exception}{Properties}{NewLine}")
        .CreateLogger();
    }

    [TestMethod]
    public void TestGetNCharacters()
    {
        string characters = GetNCharacters('-', 200);
        Assert.AreEqual(characters.Length, 200);
    }

    [TestMethod]
    public void TestGetHorizontalBorder()
    {
        string border = HorizontalBorder();
        _logger.Debug(border);

    }

    [TestMethod]
    public async Task TestGetHeaders()
    {

        MemoryStream memoryStream = new();
        StreamWriter streamWriter = new(memoryStream);

        string headers = CreateHeaders();
        await streamWriter.WriteAsync(headers);
        streamWriter.Flush();
        memoryStream.Seek(0, 0);
        var reader = new StreamReader(memoryStream);

        _logger.Debug(reader.ReadToEnd());
    }

    [TestMethod]
    public void TestSplitLine()
    {
        string line = "The Quick Brown Fox Jumps Over The Lazy Dog.";
        List<string> lines = SplitLines(new List<string>(), line, line.Length / 2);
        _logger.ForContext("Lines List", lines, true).Debug("Lines");
        Assert.AreEqual(lines.Count, 3);
    }

    [TestMethod]
    public async Task TestCreateRow()
    {
        Saying saying = new()
        {
            Id = Guid.NewGuid(),
            DateCreated = DateTime.Now,
            GuildId = 0,
            IsmKey = "testism",
            IsmRecorder = "Tybird13",
            IsmSaying = "The Quick Brown Fox Jumps Over The Lazy Dog."
        };

        MemoryStream memoryStream = new();
        StreamWriter streamWriter = new(memoryStream);
        await streamWriter.WriteLineAsync();

        string s = CreateRow(saying);
        await streamWriter.WriteAsync(s);
        streamWriter.Flush();
        memoryStream.Seek(0, 0);
        var reader = new StreamReader(memoryStream);

        _logger.Debug(reader.ReadToEnd());

    }

    [TestMethod]
    public async Task TestCreateTable()
    {
        List<Saying> sayings = new()
        {
            new Saying()
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                GuildId = 0,
                IsmKey = "testism",
                IsmRecorder = "Tybird13",
                IsmSaying = "The Quick Brown Fox Jumps Over The Lazy Dog."
            },
            new Saying()
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                GuildId = 0,
                IsmKey = "testism",
                IsmRecorder = "Swoglordicusthethird",
                IsmSaying = "two The Quick Brown Fox Jumps Over The Lazy Dog.The Quick Brown Fox Jumps Over The Lazy Dog."
            },
            new Saying()
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                GuildId = 0,
                IsmKey = "testism",
                IsmRecorder = "his uber highness mr squeaky pants...",
                IsmSaying = "three The Quick Brown Fox Jumps Over The Lazy Dog.The Quick Brown Fox Jumps Over The Lazy Dog."
            },

        };

        var stream = await CreateTableAsync(sayings);
        var reader = new StreamReader(stream);

        _logger.Debug("\n" + reader.ReadToEnd());

    }



}