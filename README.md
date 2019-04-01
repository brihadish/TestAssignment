# Test assignment

Create a solution for the following use case:
A customer brings a suit that needs altering (shorten sleeves, shorten trousers). A salesrep needs a
way to enter these alterations. Once the payment for the alteration has been received, a tailor will
pick up the suit and do the alterations. When he is finished, the customer will be notified that the
suit is ready to be picked up.

Business requirements:

2 possible alterations:
- Shorten sleeves (left and/or right), can be up to + or – 5 cm
- Shorten trousers (left and/or right), can be up to + or – 5 cm

A tailor can only start after payment has been received. Payment is done on a register by looking up
the customer and then select an alteration from a list of unpaid alterations for that customer.
The salesrep or tailor should be able to get an overview of what alterations are created, paid or
done.

Prerequisites:

An OrderPaid message is published to an Azure Topic, containing the identifier of the alteration. You
may assume that it’s there, just provide a method/function/something that will process the message.
The notification is done by an email service. It will listen for an AlterationFinished event on an Azure
Service Bus. No need to actually publish it.
Customer data is not part of this application.

Technical Requirements
- Use best practices and design patterns
- The solution will be deployed on Azure, use any Azure technology you like
- You may use any publicly Nuget package available
- Add logging to Azure Insights
- UI can be simple, just some interface for entering an alteration and an overview of
alterations
- Automation tests (Unit tests, integration tests, etc.)
