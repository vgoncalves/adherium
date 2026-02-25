using Adherium.Domain.Patients;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;

namespace Adherium.Functions.Mailing;

public class WelcomeEmailSender
{
    [Function(nameof(WelcomeEmailSender))]
    public async Task Run(
        [ServiceBusTrigger("PatientRegistered", "Subscription", Connection = "")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        // Deserialize
        var patientRegisteredEvent = message.Body.ToObjectFromJson<NewPatientRegisteredEvent>()!;

        var emailPayload = new
        {
            To = patientRegisteredEvent.Email,
            Subject = "Welcome to Adherium!",
            Body = $"Dear {patientRegisteredEvent.Name},\n\nWelcome to Adherium! We're excited to have you on board."
        };
        
        //Send email
        
        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}