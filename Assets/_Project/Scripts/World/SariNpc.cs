using System.Collections;

/// <summary>
/// Sari (tugas P6 no. 2): sebelum boss kalah memakai DialogueData tips elemen (versi dasar Npc),
/// setelah flag boss_defeated isi Talk() berubah.
/// </summary>
public class SariNpc : Npc
{
    protected override IEnumerator Talk()
    {
        if (Flags.Has(StoryFlags.BossDefeated))
        {
            yield return Dialogue.Say(NpcName, "Hebat sekali, kamu berhasil menaklukkan Raijugar di Gua Petir!");
            yield return Dialogue.Say(NpcName, "Suatu hari nanti aku juga ingin jadi pelatih sekuat kamu.");
            yield break;
        }
        yield return base.Talk();
    }
}
