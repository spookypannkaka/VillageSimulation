using UnityEngine;
using UnityEngine.U2D.Animation;

public class VillagerAppearanceController : MonoBehaviour
{
    public enum VillagerType { OldMan, Woman, Man, OldWoman, Boy, Girl }
    public VillagerType villagerType;

    // Assign each villager type’s Sprite Library in the Inspector
    public SpriteLibraryAsset oldManLibrary;
    public SpriteLibraryAsset womanLibrary;
    public SpriteLibraryAsset manLibrary;
    public SpriteLibraryAsset oldWomanLibrary;
    public SpriteLibraryAsset boyLibrary;
    public SpriteLibraryAsset girlLibrary;

    private SpriteLibrary spriteLibrary;

    private void Awake()
    {
        spriteLibrary = GetComponent<SpriteLibrary>();
        villagerType = GetRandomVillagerType();
    }

    private void Start()
    {
        // Dynamically assign the correct Sprite Library based on villager type
        switch (villagerType)
        {
            case VillagerType.OldMan:
                spriteLibrary.spriteLibraryAsset = oldManLibrary;
                break;
            case VillagerType.Woman:
                spriteLibrary.spriteLibraryAsset = womanLibrary;
                break;
            case VillagerType.Man:
                spriteLibrary.spriteLibraryAsset = manLibrary;
                break;
            case VillagerType.OldWoman:
                spriteLibrary.spriteLibraryAsset = oldWomanLibrary;
                break;
            case VillagerType.Boy:
                spriteLibrary.spriteLibraryAsset = boyLibrary;
                break;
            case VillagerType.Girl:
                spriteLibrary.spriteLibraryAsset = girlLibrary;
                break;
            default:
                Debug.LogWarning("Villager type not specified!");
                break;
        }
    }

    private VillagerType GetRandomVillagerType()
    {
        // Get all values of the VillagerType enum
        var types = System.Enum.GetValues(typeof(VillagerType));
        // Randomly pick one of the enum values
        return (VillagerType)types.GetValue(Random.Range(0, types.Length));
    }
}
