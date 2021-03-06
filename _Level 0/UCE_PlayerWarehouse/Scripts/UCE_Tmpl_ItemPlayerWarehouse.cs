// =======================================================================================
// Created and maintained by iMMO
// Usable for both personal and commercial projects, but no sharing or re-sale
// * Discord Support Server.............: https://discord.gg/YkMbDHs
// * Public downloads website...........: https://www.indie-mmo.net
// * Pledge on Patreon for VIP AddOns...: https://www.patreon.com/IndieMMO
// * Instructions.......................: https://indie-mmo.net/knowledge-base/
// =======================================================================================
using System.Text;
using UnityEngine;
using Mirror;

// PLAYER WAREHOUSE - ITEM

[CreateAssetMenu(menuName = "UCE Item/UCE Warehouse Item", order = 998)]
public class UCE_Tmpl_ItemPlayerWarehouse : UsableItem
{
    [Header("[-=-=-=- Warehouse Item -=-=-=-]")]
    [Tooltip("Decrease amount by how many each use (can be 0)?")]
    public int decreaseAmount = 1;

    // -----------------------------------------------------------------------------------
    // Use
    // @Server
    // -----------------------------------------------------------------------------------
    public override void Use(Player player, int inventoryIndex)
    {
        ItemSlot slot = player.inventory[inventoryIndex];

        // -- Only activate if enough charges left
        if (decreaseAmount == 0 || slot.amount >= decreaseAmount)
        {
            // always call base function too
            base.Use(player, inventoryIndex);

            // -- Decrease Amount
            if (decreaseAmount != 0)
            {
                slot.DecreaseAmount(decreaseAmount);
                player.inventory[inventoryIndex] = slot;
            }
        }
    }

    // -----------------------------------------------------------------------------------
    // OnUsed
    // @Client
    // -----------------------------------------------------------------------------------
    public override void OnUsed(Player player)
    {
        UCE_UI_PlayerWarehouse.singleton.Show();
    }

    // -----------------------------------------------------------------------------------
}
