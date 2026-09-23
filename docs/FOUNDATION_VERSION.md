# Foundation version

## Verified origin

- Source repository: [GameFoundation](https://github.com/JeremyMarkWilcox/GameFoundation).
- Source commit: [b2e2268a29befda4821949256e8182c8b67f2c3b](https://github.com/JeremyMarkWilcox/GameFoundation/commit/b2e2268a29befda4821949256e8182c8b67f2c3b).
- Source commit title: Overhaul on the Menu, Ready to clone and make Starters.
- Starter: 2DTopDownStarter.
- Starter initial commit: `69bbc6de79f3807f8c1e8f67480fbe94cae564c3`.
- Verified Git tree: `02cb1befa4badeda762cfff387e4a2b66ba71336`.

The starter's initial commit and the source commit have identical Git tree IDs. This verifies the complete tracked file snapshot, including paths, file modes and contents, despite their independent commit histories. It records origin, not a claim that the current working tree is unchanged or that perspective gameplay is complete. Later application-name and README changes belong to this starter.

## Foundation updates applied

Baseline: `b2e2268a29befda4821949256e8182c8b67f2c3b`. No later committed foundation version is recorded; pending local updates are listed below.

Keep the verified origin above unchanged. When bringing in a shared fix, append an entry containing its source commit, files or behavior adopted, the resulting starter commit, and validation performed. A partial update must be described as partial; do not claim the entire starter matches a newer foundation version unless that has been verified.

## Update procedure

1. Review the foundation change and its dependencies against this starter's customization.
2. Apply the relevant changes deliberately. Preserve the starter's name, input settings, scene assignments and gameplay.
3. Build and run the [foundation checks](SMOKE_TESTS.md), then test affected perspective behavior and exported builds where relevant.
4. Record the applied update here and commit the change with its validation evidence.

Template-generated repositories do not automatically receive foundation updates. Use this file as the repository's version record and the GDD for scope and design decisions.

## Pending local update: GDScript interface

Applied from the local GameFoundation working tree based on the verified origin above. Source commit and receiving starter commit are pending; nothing is committed or published yet. See [the file manifest](FOUNDATION_INTEROP_MANIFEST.md) for exact SHA-256 fingerprints.

Scope: device-change signal, UI-audio entry point, save-value accessors, distinct scene requests and completion signal; GDScript example, integration tests and documentation. Existing application name, project settings and perspective README content are preserved. This is a partial shared update, not a replacement baseline.

Validation: Godot 4.7.2 .NET build and editor import passed; headless GDScript integration passed 30 checks; windowed C# foundation regression passed 46 checks. Applied shared files match the foundation byte-for-byte. Physical-controller and exported-build certification remain release checks.
