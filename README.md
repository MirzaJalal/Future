# Future Bangla Project

## Overview
The **Future Bangla Project** is a microservices-based application that uses Azure Service Bus for asynchronous communication between various services, including email, order, and user registration functionalities. The system implements an API Gateway using Ocelot, integrates Stripe for coupons, and leverages Azure Service Bus queues and topics for messaging.

This project is built using **.NET**, **Azure**, **Stripe payments**, and **Ocelot**, focusing on a modular, scalable architecture where each service operates independently.

---

## Project Structure

### **Solution Files**
- **Future.sln**: Main solution file that organizes all projects and their dependencies.

---

### **Services**
The project follows a modular architecture, with each service handling a specific domain or feature:

1. **Bangla.Services.AuthenticationAPI**  
   Handles user authentication and integrates with Azure Service Bus to send and receive messages.

2. **Bangla.Services.CouponAPI**  
   Manages Stripe coupons and integrates with the payment system.

3. **Bangla.Services.EmailAPI**  
   Handles email functionality with multiple queues and topics in Azure Service Bus for asynchronous messaging. The service processes messages from:
   - **Email Shopping Cart Queue**: Receives shopping cart emails.
   - **User Registration Queue**: Receives user registration emails.
   - **Order Created Topic**: Processes order-related notifications for emails.
   
   *Key Feature*: Integration with Azure Service Bus Queue and Topic for message processing.

4. **Bangla.Services.OrderAPI**  
   Manages order-related functionality, including uploading images from the web application.

5. **Bangla.Services.ProductAPI**  
   Handles product-related operations, including image uploads from the web application.

6. **Bangla.Services.RewardAPI**  
   Manages reward-related functionality and integrates with Azure Service Bus for message handling.

7. **Builder.Services.ShoppingCartAPI**  
   Handles shopping cart functionality and integrates with the checkout process.

---

### **Middleware**
- **Bangla.GatewayMiddleware**  
  Implements an API Gateway using **Ocelot** to route and manage requests between the client and microservices.

—
## Key Features 
- **Order Management API**:
 - **Create Order**: Creates a new order based on the shopping cart data and stores it in the database. 
- **Get Orders**: Retrieves a list of orders based on user or admin roles.
- - **Update Order Status**: Allows updating the status of an order, including handling cancellations and refunds via Stripe. 

- **Stripe Payment Integration**:
 - **Create Stripe Checkout Session**: Generates a checkout session for the user to make a payment. 
- **Validate Stripe Session**: Verifies the payment status by checking the session details.
- **Refunds**: Initiates refunds for cancelled orders through Stripe's API. 

- **Asynchronous Messaging**: 
- **Azure Service Bus**: The application uses Azure Service Bus to send and receive messages for communication between services, such as order status updates and rewards processing. 

### **Frontend**
- **Future.Bangla.Web**  
  A web application that interacts with backend services, such as displaying products, managing orders, and interacting with the shopping cart.

---

### **Shared Infrastructure**
- **Bangla.MessageBus**  
  A shared library that contains utilities and configurations for interacting with Azure Service Bus.

---

## Azure Service Bus Integration in EmailAPI

The **Bangla.Services.EmailAPI** project demonstrates the usage of **Azure Service Bus** to handle messaging and communication between services.

- **Service Bus Queue**:
  - **EmailShoppingCart_Queue**: Receives messages related to shopping carts.
  - **RegistrationUserQueue**: Receives messages for user registration.

- **Service Bus Topic**:
  - **OrderCreated_Topic**: A topic used to manage order-related messages, such as order creation notifications.

### Key Components:
1. **AzureServiceBusReceiver**: The main service responsible for receiving messages from the queues and topics.
   - **MessageHandler**: Handles messages from the shopping cart queue.
   - **OnUserRegistrationMessageHandler**: Handles user registration messages.
   - **OnOrderPlacedMessageHandler**: Handles order-related messages.
   
2. **Message Processing**:
   - When the application starts, it begins listening to the queues and topics, processing messages asynchronously.
   - Messages are processed in the `StartReceiveMessagesAsync()` method and logged via **ILogger** for tracking.

3. **Message Completion**:
   - Once a message is successfully processed, it is marked as complete using `CompleteMessageAsync()`, ensuring that the message is removed from the queue.

---

## Prerequisites
To run this project, you will need:
- .NET 8.0.
- An Azure account with an active Service Bus instance.
- Azure Service Bus configuration keys for connecting the services (queues and topics).
- Stripe account for coupon integration.
- A local or cloud-based SQL database for storing application data.

---

## Getting Started

1. **Clone the Repository**  
   ```bash
   git clone https://github.com/MirzaJalal/Future.git
   cd Future-Bangla


