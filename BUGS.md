# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R1 | `src/RoomHub.Api/Services/RoomService.cs`, aproximativ Create | Citind codul initial | Starterul valida numele si capacitatea, dar camera avea proprietati publice si nu se construi interna respectand invarianta. Am mutat validarea in `Domain/Room.cs`. |
| R5 | `src/RoomHub.Api/Services/BookingService.cs`, Create | Citind codul initial si comparandu-l cu R5 | Starterul nu verifica daca rezervarea incepe in trecut si nu folosea `TimeProvider`. Am adaugat verificarea in `Domain/Booking.cs`, folosind providerul transmis de `CreateBookingHandler`. |
| R6 | `src/RoomHub.Api/Services/BookingService.cs`, Create | Citind codul initial | Starterul permitea rezervari mai lungi de patru ore. Am impus durata maxima in `Domain/Booking.cs`. |
| R7 | `src/RoomHub.Api/Services/BookingService.cs`, Create | Citind codul initial | Starterul considera intervalele care se ating ca suprapuse si nu verifica explicit statusul activ. Folosesc o comparatie stricta in `Domain/Booking.cs` si interoghez doar rezervarile active. |
| R8 | `src/RoomHub.Api/Services/BookingService.cs`, Cancel | Citind codul initial | Starterul modifica statusul direct si permitea anularea repetata. `Booking.Cancel()` respinge acum o rezervare deja anulata, iar handlerul salveaza schimbarea. |
| Arhitectura generala | Folderele Models, Services, Repositories si Endpoints ale starterului | Citind `ASSIGNMENT.md` | Starterul folosea repository-uri in memorie si exceptii pentru erori de afaceri. Le-am inlocuit cu vertical slices, rich models, `Result<T>`, interfete si persistenta EF Core SQLite. |
| R10 | Starterul nu avea aceasta regula in `BookingService.cs` sau in alt loc | Citind cerinta noua si verificand durata rezervarii | Pentru camere cu capacitate peste 10, `Booking.Create` permite acum exact maximum doua ore si respinge rezervarile mai lungi cu 400; capacitatea maxima 10 pastreaza limita de patru ore. |
