using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinOpti.App.Models;
using WinOpti.App.Services;

namespace WinOpti.App.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly TweakEngine _engine;
    private readonly List<TweakDefinition> _allTweaks;
    public ObservableCollection<TweakDefinition> Tweaks { get; } = [];
    public ObservableCollection<string> Output { get; } = [];
    public RelayCommand ApplySelectedCommand { get; }
    public RelayCommand UndoAllCommand { get; }
    public RelayCommand ApplyPresetCommand { get; }

    public SystemSnapshot Snapshot { get; }

    private bool _dryRun = true;
    public bool DryRun { get => _dryRun; set { _dryRun = value; OnPropertyChanged(); } }

    public string SelectedPreset { get; set; } = "Safe Optimization";
    public string[] Presets { get; } = ["Safe Optimization", "Gaming Optimization", "Low-End PC Optimization", "Privacy-Focused", "Custom Mode"];

    private TweakDefinition? _selected;
    public TweakDefinition? SelectedTweak { get => _selected; set { _selected = value; OnPropertyChanged(); } }

    public MainViewModel()
    {
        var repo = new TweakRepository();
        var appDir = AppDomain.CurrentDomain.BaseDirectory;
        _allTweaks = repo.Load(Path.Combine(appDir, "Tweaks", "tweaks.json")).ToList();
        foreach (var t in _allTweaks) Tweaks.Add(t);
        _engine = new TweakEngine(new PowerShellService(), new LoggerService(Path.Combine(appDir, "Logs")));
        Snapshot = new SystemInfoService().GetSnapshot();
        ApplySelectedCommand = new RelayCommand(_ => ApplySelected(), _ => SelectedTweak is not null);
        UndoAllCommand = new RelayCommand(_ => UndoAll());
        ApplyPresetCommand = new RelayCommand(_ => ApplyPreset());
    }

    private void ApplySelected()
    {
        if (SelectedTweak is null) return;
        Output.Add(_engine.CreateRestorePoint(DryRun));
        Output.Add($"Applied {SelectedTweak.Name}: {_engine.Apply(SelectedTweak, DryRun)}");
    }

    private void ApplyPreset()
    {
        Output.Add($"Running preset: {SelectedPreset}");
        Output.Add(_engine.CreateRestorePoint(DryRun));
        foreach (var tweak in _allTweaks.Where(t => t.Presets.Contains(SelectedPreset)))
            Output.Add($"{tweak.Name}: {_engine.Apply(tweak, DryRun)}");
    }

    private void UndoAll()
    {
        foreach (var line in _engine.UndoAll(DryRun)) Output.Add(line);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
