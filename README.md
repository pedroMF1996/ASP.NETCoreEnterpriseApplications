# ASP.NETCoreEnterpriseApplications
## ASP.NET Core Enterprise Applications - DesenvolvedorIO
This GitHub repository contains a Microservices Solution built on .NET Core 6, consisting of 7 APIs that collectively form a robust and scalable architecture for various business operations. The solution leverages Microservices architecture, Repository Pattern, JWT Authentication, and AMQP-based integration services for seamless communication between the APIs. Additionally, it includes an Identity and JWT generation Authentication API and a Back-For-Front API, along with a client application following the MVC pattern.

### Table of Contents
<ul>
  <li>Project Structure</li>
  <li>APIs</li>
  <li>Authentication</li>
  <li>Back-For-Front</li>
  <li>Client Application</li>
</ul>

### Project Structure
The ASP.NETCoreEnterpriseApplications Solution has a modular structure, with each API residing in its own project. The core components are:

<ul>
  <li>NSE.Carrinho.API: Handles shopping cart-related operations.</li>
  <li>NSE.Catalogo.API: Manages product catalog and related functionalities.</li>
  <li>NSE.Cliente.API: Handles customer information and accounts.</li>
  <li>NSE.Pagamento.API: Provides payment processing services.</li>
  <li>NSE.Pedido.API: Manages order processing and fulfillment.</li>
  <li>Authentication API: Provides user identity management and JWT token generation.</li>
  <li>Back-For-Front API: Serves as a middleware layer to simplify communication between the client and the backend microservices.</li>
  <li>Client Application: An MVC-based client application to interact with the microservices.</li>
</ul>

### APIs

#### Business APIs
<ul>
  <li>NSE.Carrinho.API: Manages shopping cart operations and related data.</li>
  <li>NSE.Catalogo.API: Manages product catalog and information.</li>
  <li>NSE.Cliente.API: Manages customer information and authentication.</li>
  <li>NSE.Pagamento.API: Handles payment processing and transactions.</li>
  <li>NSE.Pedido.API: Manages the order processing, status, and fulfillment.</li>
</ul>

#### Authentication API
Authentication API: Provides user authentication, identity management, and JWT token generation for secure access to the microservices.

#### Back-For-Front API
Back-For-Front API: Acts as an intermediary layer to simplify communication between the client application and the microservices, offering a single entry point for client interactions.

### Client Application
The client application is built using the Model-View-Controller (MVC) pattern. It interacts with the APIs to enable users to browse products, manage their shopping cart, place orders, and perform other relevant operations.
