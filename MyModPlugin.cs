using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using HarmonyLib;
using Stone_and_Wood_Generator;

[BepInPlugin("com.blakhart.stoneandwoodgenerator", "Stone and Wood Generator Mod", "1.0.0")]
[BepInDependency(Jotunn.Main.ModGuid)]
public class MyModPlugin : BaseUnityPlugin
{
    private GameObject generatorPrefab;

    private void Awake()
    {
        Harmony harmony = new Harmony("com.blakhart.stoneandwoodgenerator");
        harmony.PatchAll();

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

        // Setup Triggers
        // Create left trigger
        var leftTrigger = new GameObject("LeftTrigger");
        leftTrigger.transform.SetParent(generatorPrefab.transform);
        leftTrigger.transform.localPosition = new Vector3(-1f, 0.5f, 0f); // adjust offset
        var leftCollider = leftTrigger.AddComponent<BoxCollider>();
        leftCollider.isTrigger = true;
        leftCollider.size = new Vector3(0.5f, 1f, 0.5f); // adjust to shape

        var leftScript = leftTrigger.AddComponent<InputTriggerZone>();
        leftScript.inputType = InputTriggerZone.InputType.Resource;

        // Right trigger
        var rightTrigger = new GameObject("RightTrigger");
        rightTrigger.transform.SetParent(generatorPrefab.transform);
        rightTrigger.transform.localPosition = new Vector3(1f, 0.5f, 0f);
        var rightCollider = rightTrigger.AddComponent<BoxCollider>();
        rightCollider.isTrigger = true;
        rightCollider.size = new Vector3(0.5f, 1f, 0.5f);

        var rightScript = rightTrigger.AddComponent<InputTriggerZone>();
        rightScript.inputType = InputTriggerZone.InputType.Catalyst;

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
