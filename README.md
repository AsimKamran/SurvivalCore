# SurvivalCore

Small Unity 2022.3 LTS project demonstrating testable architecture.

## Branch 1: feature/01-health
- `Game.Runtime`: pure C# `Health` model + thin `PlayerHealthBehaviour` MonoBehaviour adapter.
- EditMode tests (NUnit) for the model, PlayMode tests (`[UnityTest]`) for the component.
- GitHub Actions workflow running both suites via GameCI.

## Branch 2: feature/02-inventory
- Pure C# `Inventory` (fixed slots, stacking, overflow returned to caller, atomic removal) and `ItemDefinition`.
- `InventoryTests` covering stacking, overflow, atomic removal, change events, argument validation.

## Run tests
Unity: Window > General > Test Runner > Run All.
CLI: `Unity -batchmode -projectPath . -runTests -testPlatform EditMode -testResults TestResults/edit.xml`

## CI secrets
UNITY_LICENSE, UNITY_EMAIL, UNITY_PASSWORD (see game.ci docs).
