using System.Collections;
using UnityEngine;

/// <summary>
/// Tetua Rahmat (P6): pilih partner -> "Yakin?" -> Ya / Pilih lagi. Partner baru di Lv 3.
/// Setelah memberi partner, flag has_starter membuat Penjaga Batas bergeser.
/// </summary>
public class StarterGiver : Npc
{
    [SerializeField] private MonsterData[] starters;
    [SerializeField] private int starterLevel = 3;
    [SerializeField] private AudioClip getClip;          // SFX_LevelUp
    [SerializeField] private GameObject questMarker;     // tanda ! di atas Tetua sebelum punya partner

    protected override void OnConstructed()
    {
        Flags.Changed += OnFlag;
        RefreshMarker();
    }

    protected override void OnDestroy()
    {
        if (Flags != null) Flags.Changed -= OnFlag;
    }

    private void OnFlag(string key) => RefreshMarker();

    private void RefreshMarker()
    {
        if (questMarker != null) questMarker.SetActive(!Flags.Has(StoryFlags.HasStarter));
    }

    protected override IEnumerator Talk()
    {
        if (Flags.Has(StoryFlags.HasStarter))
        {
            yield return AfterStarter();
            yield break;
        }

        yield return Dialogue.Say(NpcName, "Selamat pagi, pelatih muda! Hutan di utara menyimpan banyak monster liar.");
        yield return Dialogue.Say(NpcName, "Sebelum melangkah keluar desa, pilihlah satu dari tiga monster ini sebagai partnermu.");

        string[] options = new string[starters.Length];
        for (int i = 0; i < starters.Length; i++)
            options[i] = $"{starters[i].DisplayName} ({starters[i].Element})";

        MonsterData pick = null;
        while (pick == null)              // ulangi sampai "Ya"
        {
            int choice = 0, sure = 0;
            yield return Dialogue.Choose(NpcName, "Monster mana yang ingin kamu ajak berpetualang?", options, c => choice = c);
            MonsterData m = starters[choice];
            yield return Dialogue.Choose(NpcName, $"Kamu memilih {m.DisplayName} berelemen {m.Element}. Yakin?",
                new[] { "Ya", "Pilih lagi" }, c => sure = c);
            if (sure == 0) pick = m;
        }

        Services.Party.SetPartner(new MonsterInstance(pick, starterLevel)); // Lv 3
        Flags.Set(StoryFlags.HasStarter);                                   // Penjaga bergeser
        Services.Audio?.PlaySfx(getClip, 0f);

        yield return Dialogue.Say("", $"Kamu mendapat {pick.DisplayName}!");
        yield return Dialogue.Say(NpcName, "Latih dia dengan bijak. Setiap kemenangan akan menambah EXP hingga dia siap berevolusi.");
        yield return Dialogue.Say(NpcName, "Sentuh Batu Simpan di sebelahku kapan saja untuk memulihkan HP dan menyimpan progresmu.");
        Services.Toast?.Show("Penjaga Batas sudah membuka jalan ke hutan");
    }

    private IEnumerator AfterStarter()
    {
        MonsterInstance partner = Services.Party.Partner;
        string name = partner != null ? partner.Data.DisplayName : "partnermu";
        if (Flags.Has(StoryFlags.BossDefeated))
        {
            yield return Dialogue.Say(NpcName, "Luar biasa! Raijugar saja berhasil kamu taklukkan. Kamu kebanggaan Desa Awal!");
            yield break;
        }
        yield return Dialogue.Say(NpcName, $"{name} tampak makin bersemangat! Teruslah berlatih di Hutan Rimbun.");
        yield return Dialogue.Say(NpcName, "Jika sudah siap, tantanglah Raijugar yang bersemayam di Gua Petir sebelah timur laut.");
    }

    /// <summary>Warna khas elemen; dipakai juga oleh BattleFeel (P7) untuk percikan.</summary>
    public static Color ElementColor(Element element)
    {
        switch (element)
        {
            case Element.Pyro: return new Color32(255, 122, 61, 255);
            case Element.Aqua: return new Color32(79, 180, 255, 255);
            case Element.Sprout: return new Color32(124, 192, 97, 255);
            default: return new Color32(255, 216, 74, 255);   // Volt
        }
    }
}
