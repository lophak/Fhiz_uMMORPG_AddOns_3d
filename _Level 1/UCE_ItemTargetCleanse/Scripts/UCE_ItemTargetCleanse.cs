// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// * Instructions.......................: https://indie-mmo.net/knowledge-base/
// =======================================================================================
using UnityEngine;

// TARGET CLEANSE ITEM

[CreateAssetMenu(menuName = "uMMORPG Item/UCE Item Target Cleanse", order = 999)]
public class UCE_ItemTargetCleanse : UsableItem
{
    [Header("-=-=-=- UCE Target Cleanse Item -=-=-=-")]
    [Tooltip("% Chance to remove each Nerf on the target (Nerf = Buff set to 'disadvantegous')")]
    [Range(0, 1)] public float successChance;

    public float range;
    public string successMessage = "You cleansed: ";

    [Header("-=-=-=- Buff on Target -=-=-=-")]
    public BuffSkill applyBuff;

    public int buffLevel;
    [Range(0, 1)] public float buffChance;

    [Tooltip("Decrease amount by how many each use (can be 0)?")]
    public int decreaseAmount = 1;

    // -----------------------------------------------------------------------------------
    // CheckTarget
    // -----------------------------------------------------------------------------------
    public bool CheckTarget(Entity caster)
    {
        return caster.target != null && caster.CanAttack(caster.target);
    }

    // -----------------------------------------------------------------------------------
    // CheckDistance
    // -----------------------------------------------------------------------------------
    public bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
    {
        if (caster.target != null)
        {
            destination = caster.target.collider.ClosestPointOnBounds(caster.transform.position);
            return Utils.ClosestDistance(caster.collider, caster.target.collider) <= range;
        }
        destination = caster.transform.position;
        return false;
    }

    // -----------------------------------------------------------------------------------
    // Use
    // -----------------------------------------------------------------------------------
    public override void Use(Player player, int inventoryIndex)
    {
        ItemSlot slot = player.inventory[inventoryIndex];

        // -- Only activate if enough charges left
        if (decreaseAmount == 0 || slot.amount >= decreaseAmount)
        {
            if (player.target != null && player.target is Player && player.target.isAlive)
            {
                // always call base function too
                base.Use(player, inventoryIndex);

                player.target.UCE_CleanupStatusNerfs(successChance);
                player.target.UCE_ApplyBuff(applyBuff, buffLevel, buffChance);
                player.UCE_TargetAddMessage(successMessage + player.target.name);

                // decrease amount
                slot.DecreaseAmount(decreaseAmount);
                player.inventory[inventoryIndex] = slot;
            }
        }
    }

    // -----------------------------------------------------------------------------------
}
