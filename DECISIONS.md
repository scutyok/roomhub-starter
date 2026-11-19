# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

Am mutat R7, din metoda Create a clasei BookingService din starter in metoda Booking.Overlaps din Domain/Booking.cs. Aceasta regula se afla acum in domeniu pentru ca apartine conceptului de rezervare. Metoda verifica strict daca intervalele se suprapun, astfel incat intervalele care se ating exact nu sunt considerate suprapuse.

Am mutat si R11 in Domain/Booking.Cancel(DateTime now), nu in endpoint. CancelBookingHandler transmite timpul curent obtinut din TimeProvider, iar anularea este permisa numai daca mai sunt cel putin 60 de minute pana la start.

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

Result<T> este mai potrivit decat o exceptie pentru conflictul de rezervare pentru ca conflictul este un rezultat posibil si predictibil al afacerii, nu o eroare tehnica. Cand CreateBookingHandler returneaza un Error("RoomOccupied", ..., 409), endpointul RoomEndpoints il transforma intr-un raspuns ProblemDetails cu codul HTTP 409.

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

Clasele care stiu despre EF Core sunt RoomHubDbContext, RoomRepository, BookingRepository si UnitOfWork, toate din Infrastructure/Persistence. Entitatile din Domain si interfetele repository nu depind de EF Core. Acest lucru separa baza de date de logica aplicatiei, dar presupune cod suplimentar pentru configurarea contextului, repository-urilor si salvarea tranzactiilor.

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

POST /api/rooms/{roomId}/bookings

RoomEndpoints -> CreateBookingHandler -> IRoomRepository.GetAsync -> Booking.Create -> IBookingRepository.ListActiveForRoomAsync -> Booking.Overlaps -> IBookingRepository.CountActiveForParticipantOnUtcDateAsync -> IBookingRepository.AddAsync -> IUnitOfWork.SaveChangesAsync -> roomhub.db

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

Am respins folosirea proprietatilor publice cu set in clasele Room si Booking, deoarece regulile domeniului trebuie protejate prin constructori si metode. Am respins si pastrarea clasei BusinessException cu middleware-ul sau o inlocuire pe baza de exceptii, pentru ca cerinta necesita Result<T> si Error pentru erorile de afaceri.

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

Daca as avea inca 30 de minute, as adauga teste automatizate pentru intervalele care se ating, startul in trecut, durata exacta de patru ore si anulare. De asemenea, as inlocui EnsureCreated() cu migrari EF Core. Risc principal ramane posibilitatea ca doua cereri concurente sa treaca simultan verificarea suprapunerilor fara protectie la nivel de baza de date.
