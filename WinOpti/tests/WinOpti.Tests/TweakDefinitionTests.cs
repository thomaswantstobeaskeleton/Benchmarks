using System.Text.Json;
using WinOpti.App.Models;
using WinOpti.App.Services;

namespace WinOpti.Tests;

public class TweakDefinitionTests
{
    [Fact]
    public void AllTweaks_HaveRollbackAndBeforeCommand()
    {
        var path = Path.Combine("..", "..", "..", "..", "src", "WinOpti.App", "Tweaks", "tweaks.json");
        var json = File.ReadAllText(path);
        var tweaks = JsonSerializer.Deserialize<List<TweakDefinition>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        })!;

        foreach (var tweak in tweaks)
        {
            Assert.False(string.IsNullOrWhiteSpace(tweak.BeforeValueCommand));
            Assert.False(string.IsNullOrWhiteSpace(tweak.RollbackCommand));
            TweakEngine.Validate(tweak);
        }
    }
}
