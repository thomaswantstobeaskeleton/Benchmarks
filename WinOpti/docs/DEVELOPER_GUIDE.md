# Developer Guide

## Add a tweak
1. Open `src/WinOpti.App/Tweaks/tweaks.json`.
2. Add a new JSON object with unique `Id`.
3. Provide `BeforeValueCommand`, `ApplyCommand`, `RollbackCommand`.
4. Set `RiskLevel` to `Safe`, `Moderate`, or `Advanced`.
5. Set `RequiresAdmin` appropriately.
6. Run tests.

## Safety contract
Every tweak must include:
- user-facing description
- risk level
- before value check
- rollback command

## Testing
```bash
dotnet test WinOpti.sln
```

## Architecture
- `Models/`: tweak schema
- `Services/`: execution, logging, repo, validation
- `ViewModels/`: UI workflow and commands
- `Tweaks/`: modular tweak definition store
