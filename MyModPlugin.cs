using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

[BepInPlugin("com.blakhart.stoneandwoodgenerator", "Stone and Wood Generator Mod", "1.0.0")]
[BepInDependency(Jotunn.Main.ModGuid)]
public class MyModPlugin : BaseUnityPlugin
{
    private GameObject generatorPrefab;

    private void Awake()
    {
        PrefabManager.OnVanillaPrefabsAvailable += SetupGeneratorPrefab;
    }

    private void SetupGeneratorPrefab()
    {
        // Clone vanilla smelter
        generatorPrefab = PrefabManager.Instance.CreateClonedPrefab("Stone and Wood Generator", "smelter");

        // Rename internal name to avoid collisions
        generatorPrefab.name = "StoneWoodGenerator";

        // Remove the original Smelter component
        //Object.DestroyImmediate(generatorPrefab.GetComponent<Smelter>());

        // Attach your custom DuplicatorSmelter script
        generatorPrefab.AddComponent<StoneWoodGenerator>();

        // Optional: clean up leftover components (if needed)
        // Object.DestroyImmediate(generatorPrefab.GetComponent<WearNTear>()); // Only if misbehaving

        // Register as placeable piece
        var pieceConfig = new PieceConfig
        {
            Name = "Stone and Wood Generator",
            Description = "Duplicates basic materials using catalyst tiers.",
            PieceTable = "Hammer",
            Category = PieceCategories.Crafting,
            Requirements = new[]
            {
                new RequirementConfig("Stone", 10),
                new RequirementConfig("FineWood", 5),
                new RequirementConfig("Resin", 0) // Optional: set to 0 or remove if not needed
            }
        };

        var customPiece = new CustomPiece(generatorPrefab, true, pieceConfig);
        PieceManager.Instance.AddPiece(customPiece);
    }
}
