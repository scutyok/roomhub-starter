# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R1 | `src/RoomHub.Api/Services/RoomService.cs`, around Create | Reading the starter | The starter did validate name and capacity, so R1 was mostly present but the room used public properties and was not responsible for enforcing its own construction. I moved this invariant to `Domain/Room.cs`. |
| R5 | `src/RoomHub.Api/Services/BookingService.cs`, Create | Reading the starter and comparing R5 | The starter had no past-time check and used no `TimeProvider`. I added the check in `Domain/Booking.cs` using the provider passed from `CreateBookingHandler`. |
| R6 | `src/RoomHub.Api/Services/BookingService.cs`, Create | Reading the starter | The starter allowed bookings longer than four hours. I enforce the maximum duration in `Domain/Booking.cs`. |
| R7 | `src/RoomHub.Api/Services/BookingService.cs`, Create | Reading the starter | The starter considered touching intervals overlapping and did not check active status explicitly. I use the strict overlap formula in `Domain/Booking.cs` and query only active bookings. |
| R8 | `src/RoomHub.Api/Services/BookingService.cs`, Cancel | Reading the starter | The starter changed status directly and allowed repeated cancellation. `Booking.Cancel()` now rejects an already cancelled booking and the handler saves the change. |
| General architecture | The starter Models, Services, Repositories, and Endpoints folders | Reading `ASSIGNMENT.md` | The starter used in-memory repositories and business exceptions. It was replaced with vertical slices, rich domain objects, `Result<T>`, interfaces, and EF Core SQLite persistence. |
