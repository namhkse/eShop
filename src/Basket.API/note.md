# GRPC

`Protos/greet.proto`: define the `Greeter` gRPC and is used to generate the gRPC server
assets.

`Services/GreeterService`: the implementation of the `Greeter` service.

# Ordering.API

## Idempotency request

Implement request manager


# Project Ordering.Domain

```json
{
  "userId": "user-001",
  "userName": "namhk",
  "city": "Hanoi",
  "street": "123 Nguyen Trai",
  "state": "Hoan Kiem",
  "country": "Vietnam",
  "zipCode": "100000",
  "cardNumber": "41111111111",
  "cardHolderName": "Nam Ha",
  "cardExpiration": "2026-08-14",
  "cardSecurityNumber": "123",
  "cardTypeId": 1,
  "buyer": "",
  "items": [
    {
      "id": "item-001",
      "productId": 1,
      "productName": "iPhone 16 Pro",
      "unitPrice": 999.99,
      "oldUnitPrice": 1099.99,
      "quantity": 1,
      "pictureUrl": "https://example.com/images/iphone-16-pro.jpg"
    },
    {
      "id": "item-002",
      "productId": 2,
      "productName": "AirPods Pro",
      "unitPrice": 199.99,
      "oldUnitPrice": 249.99,
      "quantity": 2,
      "pictureUrl": "https://example.com/images/airpods-pro.jpg"
    }
  ]
}
```