# RentACar
a demo project


Highlights: 
+ Modelling a service to aid managing cars in a car rental company
+ Used ASP Core 3.1. WebAPI 
+ Endpoint with filtering by make/model, availability (date range) and price / price range
+ Endpoint to book a rental car.
+ No authentication / authorization in this exercise.
+ CRUD for cars, a pending reservation prevents deleting a car
+ REST, SOLID, onion architecture with clear separation of layers 
+ Used ValueObject (DDD) for Car.Specification.
+ Entity Framework In-Memory (server and tests can be run without setup / migrations)
+ Example unit tests for a controller method. 
+ Example unit tests for a business service method. 
+ Example integration tests 
+ Example functional tests using WebApplicationFactory + highlighting limitation of these for checking i.e. cache header settings
+ Example functional tests using Postman/Newman CLI (including better visibility of the final setting of cache headers from WebAPI)
