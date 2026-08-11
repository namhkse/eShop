# Introduction

The most popular framework fo rbuilding distrubuted, message-based application in .NET.

## Understand the concepts

- How messages are used to represent commands and events
- How consumers subcribe to messsages and how they are executed
- How producers send or publish messages to consumers
- How workflows (saga state machines, routing slips, job consumers)
- How transports differ in capabliities and when one is a better fit than another
- How durability, retires, scheduling and fault handling influence system behavior

# Messages

A message is a piece of data that represents something that needs to happen (a command) or something that happend(an event).

It is the fundamental unit of work in message-driven architecture: a shape of data, serialized, sent from one service
to another, consumed and acted upon.

MassTransit wraps that data in a rich envelope that carries metadata(MessageId, CorrelationId, source ...) so you get the
robustness of distrubte system without reinventing the plumbing

> Note
> MassTransit uses the full type name, including namespace for message contract

## Message Names

When choosing a name for message, the type of message should dictate the tense of the message name.

## Commands

A command tells a service to do soemthing, a command should only be consumed by a single consumer.
If you have `SubmitOrder` command, you should have only oen cusommer that implements `IConsumer<SubmitOrder>` or one
state machien with the `Event<SubmitOrder>`.

When using RabitMQ, there is not overhead using this approach.
Both Azure Service Bus and Amazon SQS have more complicated because messages need to be forwarded from topics to queues.

Commands should be expressed in a verd-noun sequence:
- UpdateCustomerAddress
- UpgradeCustomerAccount
- SubmitOrder

## Events

An evetn signifies that soemthing has happened. Events are published (using `Publish`)
Events should be expressed in a nound-verb (past tense):
- CustomerAddressUpdated
- CustomerAccountUpgraded
- OrderSubmtted, OrderAccepted

## Requests

A request is a message sent from a client to a service, and the service responds with a response message.
- GetCusotmerAddress
- GetJobStatus

## Responses

Responses are messages sent from a service to a client and typically contain the result of a request:
- CustomerAddress
- JobStatus

## Message Headers

TODO: Skip

## Message Correlation

Messages are part of a converstaion, and identifiers are used to connect messages to that conversation.
The headers supported: ConversationId, CorrelationId, InitiatorId are used to combine separate messages into a
conversation.

TODO: Skip

# Saga State Machines

The ability to coordinate a long-runnign process across multiple messages and services is a cor part of building
distributed systems.
MassTransit suports:
- consumer sageas
- saga state machines

A saga s a long-livved transaction managed by a coordinatior. 
It is started by an event, reacts to later events, stores progress between those event,
and drives the next steops in the process by publshing events or sending commands.

Sagas are sesigned to manage ditributed workflows without locking resources or requiring immedate consistency.

> Note
> In MassTransit, sagas are a from of orchstration.

## Why use a saga ?

Saga state machines are usefull when a rpocess:
- spans multipel messages over time
- depends on wokr done by other services
- needs to remember prior decisions or data
- must handle timeouts, retries, duplicate deliever
- 