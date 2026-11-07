using System.Collections;
using UnityEngine;

/// <summary>Pak Dagang (P6): hadiah 3 Ramuan sekali saja (flag gift).</summary>
public class MerchantNpc : Npc
{
    [SerializeField] private int giftPotions = 3;
    [SerializeField] private AudioClip giftClip;          // SFX_Heal

    protected override IEnumerator Talk()
    {
        if (Flags.Has(StoryFlags.Gift))
        {
            yield return Dialogue.Say(NpcName, "Gunakan Ramuan saat darurat di tengah battle untuk memulihkan 40% HP partnermu!");
            yield break;
        }

        yield return Dialogue.Say(NpcName, "Hendak berangkat ke utara? Bawalah persediaan obat ini sebagai bekal dariku!");
        Services.Party.AddPotions(giftPotions);
        Flags.Set(StoryFlags.Gift);
        Services.Audio?.PlaySfx(giftClip, 0f);
        yield return Dialogue.Say("", $"Kamu menerima {giftPotions} Ramuan!");
        Services.Toast?.Show($"Ramuan +{giftPotions}");
    }
}
