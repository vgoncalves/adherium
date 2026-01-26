# Adherium - Senior Backend Engineer Take-Home Project

## Patient Adherence Data Sync Service

### Context

You're joining a healthcare technology company that builds remote patient monitoring solutions. Our platform collects inhaler usage data from Bluetooth-connected sensors and calculates adherence metrics for clinical dashboards. A common challenge we face is ensuring data consistency between mobile clients and our backend when patients have intermittent connectivity.

### The Problem

A patient's mobile app collects inhaler usage events while offline. When connectivity is restored, the app sends a batch of events to your API.

Build a small .NET service that:

1. Accepts a batch of usage events via a REST endpoint (each event has: timestamp, device ID, patient ID, event type)
2. Handles idempotency (the same events may be sent multiple times due to network issues)
3. Calculates a simple "adherence score" for the patient (care plan suggests 2 puffs, 2x per day)
4. Returns the updated adherence score and confirmation of which events were processed

### Requirements

- .NET 8+ and C#
- Include basic unit tests for your adherence calculation logic
- Provide a README explaining your design decisions, especially around idempotency and data consistency
- In-memory storage is fine (no database required)

### Evaluation Criteria

- Clean, readable code structure
- Thoughtful handling of edge cases (duplicate events, out-of-order timestamps, missing data)
- Clear explanation of tradeoffs in your README
- Test coverage of critical business logic
- Communication with the Adherium Product team

### Time Expectation

2-3 hours

### Communication

Communication with team is encouraged. Please liaise with:
- **Johnny Soares** (johnnyl@adherium.com) for engineering questions
- **Luke Allera** for subject matter questions

### Deadline

Please submit within 5 days of receipt. Send a link to a GitHub repo or a zip file to davidh@adherium.com

Should you need more time please reach out.

Upon review of your submission you may be invited to present your work to the rest of the engineering and product team.