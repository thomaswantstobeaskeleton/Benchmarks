using System.Text.Json;
using WinOpti.App.Models;

namespace WinOpti.App.Services;

public sealed class TweakRepository
{
    public IReadOnlyList<TweakDefinition> Load(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<TweakDefinition>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        }) ?? [];
    }
}
