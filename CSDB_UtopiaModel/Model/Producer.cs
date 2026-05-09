using CSDB_UtopiaModel.Persistence;

namespace CSDB_UtopiaModel.Model;

public abstract class Producer : Building, ITickable
{
    protected int yield;
    private readonly Model _model;
    protected Stop? stop;
    protected Field? location;

    private IResource? _requiredResourceCache;
    private IResource? _producedResourceCache;

    public abstract int RequiredAmount { get; }
    
    public abstract int ProducedAmount { get; }

    protected Producer(Field f, int yield, Model model) : base(f)
    {
        this.yield = yield;
        _model = model;

        var timeControl = TimeControl.Instance();

        timeControl += (this, yield);

        // Elõre lekérjük a típusokat
        _requiredResourceCache = Require();
        _producedResourceCache = Produce();
    }
    public Stop ConnectsTo { get; set; }
    public abstract IResource Produce();

    public abstract IResource Require();

    public Task Tick()
    {
        if (Owner is null) return Task.CompletedTask;

        // 1. Megálló ellenõrzése (marad a régi, jól mûködõ logika)
        if (stop is null)
        {
            stop = FindAdjacentStop();
            if (stop is null) return Task.CompletedTask;
        }

        // 2. Lelõhely frissítése: Ha nincs, vagy kimerült, keressünk újat a 2x2-es területen
        if (location is null || location.DepletionLevel <= 0)
        {
            location = FindResourceLocation();
        }

        // Ha még így sincs (már mind a 4 mezõ kimerült), akkor leáll a termelés
        if (location is null || location.DepletionLevel <= 0)
            return Task.CompletedTask;

        // 3. Mennyiség kiszámítása (Hangulat-módosítóval)
        int mood = _model.Mood;
        int pa = (int)(ProducedAmount * Math.Sqrt(Math.Abs(mood)) * Math.Sign(mood));

        // 4. Termelés végrehajtása
        if (pa > 0)
        {
            // Ne bányásszunk többet, mint amennyi az adott mezõn van
            int actualProduced = Math.Min(pa, location.DepletionLevel);

            if (actualProduced > 0)
            {
                // LEVONÁS a konkrét mezõrõl
                location.DepletionLevel -= actualProduced;

                // BETÖLTÉS a megállóba
                stop.Load(_producedResourceCache!, actualProduced);

                // UI FRISSÍTÉS
                _model.OnFieldsUpdated(location); // A mezõ kimerültsége miatt
                _model.OnFieldsUpdated(stop.Owner); // A megálló raktára miatt

                // GLOBÁLIS RAKTÁR FRISSÍTÉSE
                // Lekérjük az adott típus aktuális mennyiségét a modell raktárából
                int currentGlobalAmount = _model.GetResourceCount(_producedResourceCache!);
                int newGlobalAmount = currentGlobalAmount + actualProduced;

                // Frissítjük a modell raktárát
                _model.SetResourceAmount(_producedResourceCache!, newGlobalAmount);

            }
        }

        return Task.CompletedTask;
    }

    private Stop? FindAdjacentStop()
    {
        // 2x2-es épület esetén:
        // i: -1-tõl 2-ig (szélesség)
        // k: -1-tõl 2-ig (magasság)
        // Ez egy 4x4-es gyûrût néz át az épület körül
        for (int i = -1; i <= area.Width; i++)
        {
            for (int k = -1; k <= area.Height; k++)
            {
                // Kihagyjuk a belsõ részeket, ahol az épület maga áll (0,0-tól Width,Height-ig)
                // Csak a széleken (a kerítésen) keresünk megállót
                if (i >= 0 && i < area.Width && k >= 0 && k < area.Height) continue;

                int nx = Owner.Coordinates.X + i;
                int ny = Owner.Coordinates.Y - k; // Figyelj a jelre! Ha a modelledben Y felfelé nõ, legyen +, ha lefelé, akkor -

                if (nx < 0 || nx >= _model.Width || ny < 0 || ny >= _model.Height) continue;

                Field f = _model.GetField(nx, ny);
                if (f.Buildable is Stop foundStop) return foundStop;
            }
        }
        return null;
    }

    private Field? FindResourceLocation()
    {
        // A bánya azt termeli, amit PRODUCE-ol (pl. Diamond)
        Type producedType = _producedResourceCache!.GetType();

        // Végigkérdezzük a 2x2-es területet
        for (int i = 0; i < area.Width; i++)
        {
            for (int k = 0; k < area.Height; k++)
            {
                int nx = Owner.Coordinates.X + i;
                int ny = Owner.Coordinates.Y - k; // Y felfelé nõ, tehát +k

                if (nx < 0 || nx >= _model.Width || ny < 0 || ny >= _model.Height) continue;

                Field f = _model.GetField(nx, ny);

                // CSAK akkor fogadjuk el, ha van is benne mit bányászni!
                // Ha a DepletionLevel 0, akkor ez a mezõ már "kimerült"
                if (f.Resource.GetType() == producedType && f.DepletionLevel > 0)
                {
                    return f;
                }
            }
        }
        return null; // Ha nincs alatta pozitív lelõhely, null-t adunk vissza
    }
}