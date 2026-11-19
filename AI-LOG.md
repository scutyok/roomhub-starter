# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| 09:06 | Asked the AI to identify starter contradictions and inspect the assignment | I kept the API contract and business-rule summaries | I rejected a proposed blanket rewrite until the concrete starter behavior was verified against the assignment. |
| 09:08 | Asked the AI to implement the vertical-slice and rich-domain architecture | Kept the five feature areas, private domain properties, `Result<T>`, and EF Core repository approach | Rejected initial package versions incompatible with net10.0 and corrected them to EF Core 10.0.0. |
| 09:10 | Asked the AI to fix compile errors and test the HTTP contract | Kept strict overlap rules, `TimeProvider`, and SQLite | Corrected an ambiguous `RoomResult` name and missing namespace import discovered by the compiler. |
