# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| 09:06 | I-am cerut AI-ului sa inspecteze cerinta data | Am pastrat contractul API si regulile de afaceri | Am respins o rescriere completa pana cand comportamentul concret al starterului a fost verificat fata de cerinta. |
| 09:08 | I-am cerut AI sa implementeze arhitectura pe vertical slice si modelul de domeniu bogat | Am pastrat cinci zone de features, proprietatile private ale domeniului, `Result<T>` si abordarea EF Core prin repository | Am respins versiunile initiale ale pachetelor incompatibile cu net10.0 si am corectat la EF Core 10.0.0. |
| 09:10 | I-am cerut AI sa corecteze erorile de compilare si sa testeze contractul HTTP | Am pastrat regulile stricte de suprapunere, `TimeProvider` si SQLite | Am corectat numele ambiguu `RoomResult` si namespace-ul lipsa descoperite de compilator. |
| 09:20 | I-am cerut AI sa adauge regula noii cerinte pentru maximum doua rezervari active pe zi pentru acelasi participant | Am pastrat zilele in UTC, contarea rezervarilor active in toate camerele si anularea ca exceptie | Am adaugat o metoda in repository si verificarea in `CreateBookingHandler`, nu in endpoint. |
| 09:26 | I-am cerut AI sa adauge R10 pentru durata rezervarilor in camere cu capacitate mai mare decat 10 | Am pastrat limita de patru ore pentru camerele cu capacitate cel mult 10 | Am mutat calculul duratei maxime in `Booking.Create`, astfel incat doua ore sa fie permise exact iar durate mai mari sa returneze 400. |