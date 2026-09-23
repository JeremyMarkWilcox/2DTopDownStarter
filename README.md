# 2DTopDownStarter

**Status: in development—overhead prototype implemented; isometric example planned.**

A starting point for 2D games where the player moves around a world viewed from above. The goal is a small, understandable movement and interaction foundation that can grow into exploration, adventure, action or other top-down games.

## Purpose and planned direction

- Build an overhead example first: keyboard/controller movement, consistent diagonal speed, collisions, facing and idle/move animation hooks.
- Add a following camera with bounds and tunable behavior, a simple interaction prompt, and a small room with obstacles, an interactable and an exit.
- Connect spawn/respawn and player progress to the existing menus, scene flow and save backend.
- Add an isometric example using the same systems, with appropriate tiles, ground-footprint collisions, draw ordering and documented movement directions. Keep it in this repository rather than duplicating the controller.

The initial scope is movement on one plane. Combat, inventory, dialogue, click-to-move, tactical grids and multiple elevations are possible extensions, not promised starter features. The goals can evolve as the owner tests the example.

## Presentation

Pixel art is the default presentation for this 2D starter. The project uses a 640 × 360 base viewport, 1280 × 720 development window, viewport stretching, keep aspect, integer scaling and nearest texture filtering. Raster art remains a documented conversion of this same starter; it does not require a second repository.

Raster art will use the same starter with documented resolution, stretching and filtering changes. See the [setup guide](docs/PROJECT_SETUP.md). The overhead example is implemented; the isometric example will reuse its movement and interaction systems.

## What will demonstrate readiness

Both example scenes should have reliable movement, correct collision/draw order, camera bounds and usable interactions. Verify keyboard/controller play, pause isolation, respawn, save/continue and a clean desktop export before presenting this as a ready-to-use perspective starter.

## Available now: the inherited foundation

New Game launches the overhead test trail in Game/TopDown/OverheadDemo.tscn. The prototype includes a camera-following player, solid obstacles, a sign interaction, a checkpoint, a hazard and a save/continue exit. Isometric presentation and art/animation replacement remain planned work.

The copied foundation includes title/pause/settings menus, keyboard and controller menu confirmation/back, scene transitions, persistent volume/fullscreen and Save-action bindings, a versioned save backend and Continue, optional UI audio, and a shared Theme. Its regression scene is included. Existing validation records describe foundation checks, not certification of this starter's future gameplay.

## Run the overhead prototype

Requirements: the .NET build of Godot 4.7.2 and .NET 8 SDK for desktop. Android configuration targets .NET 9 but has not been validated here. Gameplay movement bindings will be documented when implemented; the current controls below operate the foundation demo.

1. Import `project.godot` into the **.NET** build of Godot 4.7.2.
2. Click **Build**, then **F5**. The title scene includes Settings as a sibling menu.
3. Choose New Game to enter the overhead test trail. Move with WASD, arrow keys or the left stick. Use E or controller X near the sign, blue checkpoint or gold exit. Escape / controller Menu (Start, hamburger) / B opens Pause; pressing it again resumes.
4. **F5 / controller Y** saves the current position and checkpoint. The gold exit returns to Title; Continue restores the saved prototype position. Settings includes a Save-action rebind button; Escape/B cancels listening.

The solution currently retains its original `Main Menu.sln` filename. This does not affect the project name or behavior.

## Assign references in the Inspector

- **Main menu:** assign its gameplay `PackedScene`, sibling `SettingsMenu`, and button nodes.
- **Pause menu:** assign its Settings menu and buttons in the gameplay scene.
- **Settings:** its controls are direct exported node fields. Renaming/rearranging UI no longer requires editing script paths.
- **Return to Title:** open `Configuration/SceneFlow.tscn`, expand `MainMenuScene`, and select a scene file. The custom `SceneReference` resource is lazy to avoid title → game → pause → title circular PackedScene dependencies.
- **Audio:** optional UI sound streams can be assigned in `addons/CoreUI/Scenes/Audio.tscn`. Empty slots are silent by design.
- **Input:** actions are defined in Project Settings → Input Map. Scripts expose `StringName` action fields. These remain names because Input Map actions are named engine identifiers.
- **Theme:** edit `addons/CoreUI/Resources/FoundationTheme.tres` or assign a replacement Theme on your menu roots.

Fields are the team convention for Inspector configuration. Godot also supports exported properties; properties are not universally unsafe.

## Ownership and dependencies

`addons/CoreUI/Scripts` and `Scenes` contain reusable code. They do not depend on example controllers. `Examples` demonstrates composition; `Configuration` holds this project's title destination. Create a `Game` folder for each real project’s scenes and scripts.

Required autoloads are preconfigured: MenuManager, SceneFlowManager, SettingsManager, AudioManager, PauseController, InputManager, SaveManager. The previously installed Phantom Camera 0.11.0.3 files and license are retained, but its editor plugin and autoload are disabled. The foundation does not require it. See `docs/OPTIONAL_SYSTEMS.md` before enabling or adding plugins.

Use `MenuManager.OpenMenu(assignedMenu)` for normal UI composition. Menu IDs and UIEventBus string opening remain an optional compatibility API; duplicate IDs produce a diagnostic. The active menu owns focus and input. Device detection does not change gameplay mouse mode; menus temporarily choose cursor visibility and restore the previous mode when closed. Title menus cannot be dismissed with Back. PauseController's manual pause is combined with menu pause requirements.

## Persistence

Preferences and keybindings share `user://settings.cfg` using read/modify/write. Bindings reload at startup. One binding per device family (keyboard/mouse or gamepad) is supported for each action. Duplicate/conflicting bindings, analog-axis rebinding UI, and a reset-to-default UI are not included.

SaveManager supports numbered slots, schema version 1, temporary-file replacement and a previous-save backup. Continue loads the saved scene. The demo saves its scene; its optional Player field can also record and restore a Node2D/Node3D position when assigned. New Game resets memory; it does not delete the previous save until the next successful save. Restart reloads the current scene and restores its last saved position when available. Each game must decide its actual checkpoint and autosave policy.

Save data strings (JSON keys, paths stored on disk), log messages, labels, and engine property names are intentional strings. Editable scene, node and menu dependencies are Inspector references.

## Development and handoff

Build and test one small playable example before expanding the feature list. The overhead prototype currently passes its 20-check smoke test plus the shared 30-check GDScript and 46-check foundation suites. The owner will validate this starter before handing it to other developers. Update this README as features move from planned to implemented, including exact controls, Inspector assignments, screenshots and limitations.

The [GDD](https://docs.google.com/document/d/1Om7zLuNNLW-n-AbYMff3WZQE9F3F2vL3bwv_7Om-bNg/edit) is the source of truth for scope and decisions; this README describes what this repository currently runs. The copied [starter roadmap](docs/STARTER_ROADMAP.md) is planning background and may predate repository creation. See [acceptance checks](docs/SMOKE_TESTS.md), [foundation scope](docs/FRAMEWORK_STATUS.md), and the [new-game GDD outline](docs/GDD_TEMPLATE.md).

Keep common fixes in [GameFoundation](https://github.com/JeremyMarkWilcox/GameFoundation), then deliberately bring the relevant changes into this starter and rerun its checks. Template-generated projects do not receive those changes automatically. The verified starting version is [GameFoundation b2e2268](https://github.com/JeremyMarkWilcox/GameFoundation/commit/b2e2268a29befda4821949256e8182c8b67f2c3b). See [the foundation version record](docs/FOUNDATION_VERSION.md) for the exact source, verification and update procedure.

When creating a game from this starter, rename the Godot application before saving, replace the demo with the game's own scene, and give it its own repository. Preserve third-party licenses. Add optional plugins only after checking compatibility and documenting their setup and dependencies.

## GDScript integration

Gameplay can use GDScript with the shared C# globals. See [the integration guide](docs/GDSCRIPT_INTEGRATION.md) for Inspector composition, save access, signals and the runnable example. Godot .NET and a C# build remain required.
